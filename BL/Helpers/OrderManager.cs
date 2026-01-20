using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers;

/// <summary>
/// Provides business logic operations for managing orders,
/// including creation, retrieval, update, cancellation,
/// status calculation, and delivery coordination.
/// Acts as the main BL layer facade for order-related workflows.
/// </summary>
internal static class OrderManager
{
    /// <summary>
    /// Observer manager responsible for notifying UI components
    /// when order-related data changes.
    /// </summary>
    internal static ObserverManager Observers = new();

    /// <summary>
    /// Mutex used to prevent overlapping periodic order updates
    /// triggered by system clock changes.
    /// </summary>
    private static readonly AsyncMutex s_periodicMutex = new(); // stage 7

    /// <summary>
    /// Mutex used to ensure that order simulation logic
    /// is executed by a single thread at a time.
    /// </summary>
    private static readonly AsyncMutex s_simulationMutex = new(); // stage 7



    /// <summary>
    /// Data access layer instance used by the order manager
    /// to perform CRUD operations on orders and deliveries.
    /// </summary>
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Assumed courier average speed (in kilometers per hour),
    /// used for calculating expected delivery times.
    /// </summary>
    private const double CourierSpeedKmPerHour = 40.0;

    /// <summary>
    /// Retrieves a full Order business object by ID, including deliveries,
    /// calculated distances, timing, status, and scheduling information.
    /// </summary>
    internal static async Task<BO.Order> GetAsync(int id)
    {
        DO.Order doOrder = s_dal.Order.Read(id)
            ?? throw new BlDoesNotExistException($"Order with ID={id} not found");

        List<BO.DeliveryPerOrderInList> deliveries =
            (await DeliveryManager.GetDeliveriesByOrderAsync(id)).ToList();

        DO.Delivery? activeDelivery =
            s_dal.Delivery.ReadAll(d => d.OrderId == id && d.EndDeliveryDate == null)
            .FirstOrDefault();

        BO.OrderStatus orderStatus =
            CalcOrderStatus(
                s_dal.Delivery.ReadAll(d => d.OrderId == id).ToList());

        var (lat, lon) =
           await Tools.GetCoordinatesCachedAsync(doOrder.Address ?? "");

        double airDistance =
            Tools.CalcAirDistance(lat, lon, 32.0853, 34.7818);

        DateTime createdAt = doOrder.OpenDate ?? DateTime.Now;
        DateTime expectedDeliveryTime = createdAt;

        DateTime maxDeliveryTime =
            Tools.CalcMaxDeliveryTime(
                expectedDeliveryTime,
                (BO.OrderType)doOrder.Type);

        TimeSpan timeRemaining =
            maxDeliveryTime > DateTime.Now
                ? maxDeliveryTime - DateTime.Now
                : TimeSpan.Zero;

        if (activeDelivery != null)
        {
            expectedDeliveryTime =
                Tools.CalcExpectedDeliveryTime(
                    activeDelivery.StartDeliveryDate,
                    airDistance,
                    CourierSpeedKmPerHour);

            maxDeliveryTime =
                Tools.CalcMaxDeliveryTime(
                    expectedDeliveryTime,
                    (BO.OrderType)doOrder.Type);

            timeRemaining =
                maxDeliveryTime > DateTime.Now
                    ? maxDeliveryTime - DateTime.Now
                    : TimeSpan.Zero;
        }

        DateTime? deliveredAt =
            deliveries.LastOrDefault(d =>
                d.CompletionStatus == BO.DeliveryStatus.Delivered)
            ?.EndDeliveryDate;

        ScheduleStatus scheduleStatus =
            Tools.CalcScheduleStatus(
                orderStatus,
                maxDeliveryTime,
                deliveredAt);

        return new BO.Order
        {
            Id = doOrder.Id,
            Type = (BO.OrderType)doOrder.Type,
            Description = doOrder.Description,
            Address = doOrder.Address,
            Latitude = lat,
            Longitude = lon,
            AirDistance = airDistance,
            CustomerName = doOrder.CustomerName,
            CustomerPhone = doOrder.CustomerPhone,
            PackageDetails = $"Weight: {doOrder.Weight}",
            CreatedAt = doOrder.OpenDate ?? DateTime.MinValue,
            ExpectedDeliveryTime = expectedDeliveryTime,
            MaxDeliveryTime = maxDeliveryTime,
            TimeRemaining = timeRemaining,
            OrderStatus = orderStatus,
            ScheduleStatus = scheduleStatus,
            Deliveries = deliveries
        };
    }

    /// <summary>
    /// Retrieves a summarized list of all orders for list display,
    /// including status, timing, and delivery counts.
    /// </summary>
    internal static IEnumerable<BO.OrderInList> GetAll()
    {
        DateTime now = DateTime.Now;

        return s_dal.Order.ReadAll().Select(o =>
        {
            List<DO.Delivery> deliveries =
                s_dal.Delivery.ReadAll(d => d.OrderId == o.Id).ToList();

            DO.Delivery? last = deliveries.LastOrDefault();

            if (o.OpenDate == null)
                throw new BlInvalidInputException(
                    $"Order {o.Id} has no OpenDate");

            TimeSpan handlingTime = now - o.OpenDate.Value;
            TimeSpan timeRemaining = TimeSpan.Zero;

            if (last?.ExpectedDistance != null &&
                last.CompletionStatus == DO.DeliveryStatus.InProgress)
            {
                double totalMinutes = last.ExpectedDistance.Value * 5;
                TimeSpan elapsed = now - last.StartDeliveryDate;
                timeRemaining =
                    TimeSpan.FromMinutes(totalMinutes) - elapsed;
            }

            return new BO.OrderInList
            {
                OrderId = o.Id,
                DeliveryId = last?.Id,
                Type = (BO.OrderType)o.Type,
                OrderStatus = CalcOrderStatus(deliveries),
                ScheduleStatus = BO.ScheduleStatus.Scheduled,
                DeliveriesCount = deliveries.Count,
                AirDistance = last?.ExpectedDistance ?? 0,
                HandlingTime = handlingTime,
                TimeRemaining =
                    timeRemaining > TimeSpan.Zero
                        ? timeRemaining
                        : TimeSpan.Zero,
                CanCancel =
                    last == null ||
                    last.CompletionStatus == DO.DeliveryStatus.InProgress
            };
        });
    }

    /// <summary>
    /// Retrieves all orders, optionally filtered.
    /// </summary>
    internal static async Task<IEnumerable<BO.Order>> ReadAllAsync(
        Func<BO.Order, bool>? filter = null)
    {
        var result = new List<BO.Order>();

        foreach (var o in GetAll())
        {
            var fullOrder = await GetAsync(o.OrderId);
            result.Add(fullOrder);
        }

        return filter == null ? result : result.Where(filter);
    }

    /// <summary>
    /// Wrapper for reading a single order by ID.
    /// </summary>
    internal static Task<BO.Order> ReadAsync(int id) => GetAsync(id);

    /// <summary>
    /// Creates a new order in the system and assigns a new incremental ID.
    /// </summary>
    internal static void Create(BO.Order order)
    {
        int id;

        lock (AdminManager.BlMutex) 
        {
            id = s_dal.Order.ReadAll().Any()
                ? s_dal.Order.ReadAll().Max(o => o.Id) + 1
                : 1;

            DO.Order doOrder = new()
            {
                Id = id,
                Type = (DO.OrderType)order.Type,
                Description = order.Description,
                Address = order.Address,
                Latitude = order.Latitude,
                Longitude = order.Longitude,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                Weight = 0,
                OpenDate = DateTime.Now
            };

            s_dal.Order.Create(doOrder);
        }

        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Updates an existing order's editable fields.
    /// </summary>
    internal static void Update(BO.Order order)
    {
        lock (AdminManager.BlMutex)
        {
            DO.Order doOrder = s_dal.Order.Read(order.Id)
                ?? throw new BlDoesNotExistException(
                    $"Order with ID={order.Id} not found");

            s_dal.Order.Update(doOrder with
            {
                Type = (DO.OrderType)order.Type,
                Description = order.Description,
                Address = order.Address,
                Latitude = order.Latitude,
                Longitude = order.Longitude,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone
            });
        }

        Observers.NotifyItemUpdated(order.Id);
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Deletes an order by ID.
    /// </summary>
    internal static void Delete(int id)
    {
        lock (AdminManager.BlMutex)
        {
            s_dal.Order.Delete(id);
        }

        Observers.NotifyItemUpdated(id);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Cancels an order by either canceling the active delivery
    /// or creating a canceled delivery record.
    /// </summary>
    internal static void Cancel(int orderId)
    {
        lock (AdminManager.BlMutex) 
        {
            DO.Order order = s_dal.Order.Read(orderId)
                ?? throw new BlDoesNotExistException(
                    $"Order with ID={orderId} not found");

            List<DO.Delivery> deliveries =
                s_dal.Delivery.ReadAll(d => d.OrderId == orderId)
                    .OrderBy(d => d.Id)
                    .ToList();

            BO.OrderStatus status = CalcOrderStatus(deliveries);

            if (status is BO.OrderStatus.Delivered or BO.OrderStatus.Failed)
                throw new BlInvalidInputException(
                    "Cannot cancel a closed order");

            DO.Delivery? active =
                deliveries.FirstOrDefault(d =>
                    d.CompletionStatus == DO.DeliveryStatus.InProgress);

            if (active != null)
            {
                s_dal.Delivery.Update(active with
                {
                    CompletionStatus = DO.DeliveryStatus.Canceled
                });
            }
            else
            {
                int newId = s_dal.Delivery.ReadAll().Any()
                    ? s_dal.Delivery.ReadAll().Max(d => d.Id) + 1
                    : 1;

                s_dal.Delivery.Create(new DO.Delivery
                {
                    Id = newId,
                    OrderId = orderId,
                    CourierId = 0,
                    StartDeliveryDate = DateTime.Now,
                    EndDeliveryDate = DateTime.Now,
                    ExpectedDistance = 0,
                    CompletionStatus = DO.DeliveryStatus.Canceled
                });
            }
        }

        Observers.NotifyItemUpdated(orderId);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Calculates the overall order status based on its deliveries.
    /// </summary>
    private static BO.OrderStatus CalcOrderStatus(
        List<DO.Delivery> deliveries)
    {
        if (!deliveries.Any())
            return BO.OrderStatus.Created;

        if (deliveries.Any(d =>
            d.CompletionStatus == DO.DeliveryStatus.InProgress))
            return BO.OrderStatus.InDelivery;

        if (deliveries.All(d =>
            d.CompletionStatus == DO.DeliveryStatus.Delivered))
            return BO.OrderStatus.Delivered;

        if (deliveries.Any(d =>
            d.CompletionStatus == DO.DeliveryStatus.Canceled))
            return BO.OrderStatus.Failed;

        return BO.OrderStatus.Assigned;
    }

    /// <summary>
    /// Deletes all orders from the system.
    /// </summary>
    internal static void DeleteAll()
    {
        lock (AdminManager.BlMutex)
        {
            s_dal.Order.DeleteAll();
        }

        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Retrieves all open orders that a courier can potentially accept
    /// based on delivery distance constraints.
    /// </summary>
    internal static async Task<IEnumerable<OpenOrderInList>>
        GetOpenOrdersForCourierAsync(int courierId)
    {
        var courier = await CourierManager.GetAsync(courierId);
        var hubCoords =
            await Tools.GetCoordinatesAsync("Main Logistics Center");

        return
            from order in s_dal.Order.ReadAll()
            let orderLat = order.Latitude ?? 0
            let orderLon = order.Longitude ?? 0
            let airDistance =
                Tools.CalcAirDistance(
                    hubCoords.Latitude,
                    hubCoords.Longitude,
                    orderLat,
                    orderLon)
            where courier.MaxPersonalDeliveryDistance != null
               && airDistance <= courier.MaxPersonalDeliveryDistance.Value
            select new OpenOrderInList
            {
                CourierId = null,
                OrderId = order.Id,
                Type = (BO.OrderType)order.Type,
                ItemCategory = order.Description,
                Address = order.Address,
                AirDistance = airDistance,
                ActualDistance = null,
                ActualTime = null,
                ScheduleStatus = BO.ScheduleStatus.Scheduled,
                RemainingTime = TimeSpan.Zero,
                EndTime = order.OpenDate ?? DateTime.Now
            };
    }

    /// <summary>
    /// Synchronous wrapper for GetAsync (required by BL interfaces).
    /// </summary>
    internal static BO.Order Get(int id)
        => GetAsync(id).GetAwaiter().GetResult();

    /// <summary>
    /// Synchronous wrapper for ReadAllAsync.
    /// </summary>
    internal static IEnumerable<BO.Order> ReadAll(Func<BO.Order, bool>? filter = null)
        => ReadAllAsync(filter).GetAwaiter().GetResult();

    /// <summary>
    /// Synchronous wrapper for ReadAsync (required by BL implementation).
    /// </summary>
    internal static BO.Order Read(int id)
        => GetAsync(id).GetAwaiter().GetResult();

    /// <summary>
    /// Synchronous wrapper for GetOpenOrdersForCourierAsync.
    /// </summary>
    internal static IEnumerable<BO.OpenOrderInList>
        GetOpenOrdersForCourier(int courierId)
        => GetOpenOrdersForCourierAsync(courierId)
            .GetAwaiter()
            .GetResult();


    /// <summary>
    /// Performs periodic updates on orders according to system time progression.
    /// Automatically cancels orders that exceeded their maximum allowed delivery time.
    /// </summary>
    internal static void PeriodicOrdersUpdates(DateTime oldClock, DateTime newClock)
    {
        if (s_periodicMutex.CheckAndSetInProgress())
            return;

        try
        {
            lock (AdminManager.BlMutex)
            {
                var orders = s_dal.Order.ReadAll().ToList();

                foreach (var order in orders)
                {
                    var deliveries = s_dal.Delivery
                        .ReadAll(d => d.OrderId == order.Id)
                        .ToList();

                    var status = CalcOrderStatus(deliveries);

                    if (status is BO.OrderStatus.Delivered or BO.OrderStatus.Failed)
                        continue;

                    if (order.OpenDate != null)
                    {
                        var maxTime =
                            Tools.CalcMaxDeliveryTime(
                                order.OpenDate.Value,
                                (BO.OrderType)order.Type);

                        if (newClock > maxTime)
                        {
                            int newId =
                                s_dal.Delivery.ReadAll().Any()
                                    ? s_dal.Delivery.ReadAll().Max(d => d.Id) + 1
                                    : 1;

                            s_dal.Delivery.Create(new DO.Delivery
                            {
                                Id = newId,
                                OrderId = order.Id,
                                CourierId = 0,
                                StartDeliveryDate = newClock,
                                EndDeliveryDate = newClock,
                                CompletionStatus = DO.DeliveryStatus.Canceled
                            });
                        }
                    }
                }
            }
        }
        finally
        {
            s_periodicMutex.UnsetInProgress();
        }
    }

    /// <summary>
    /// Simulates order-related activity during simulator runtime.
    /// Used to trigger observer updates and validate UI responsiveness.
    /// </summary>
    internal static async Task SimulateOrdersAsync() // stage 7
    {
        if (s_simulationMutex.CheckAndSetInProgress())
            return;

        try
        {
            await Task.Delay(500);

            lock (AdminManager.BlMutex)
            {
                var order =
                    s_dal.Order.ReadAll()
                        .FirstOrDefault(o => o.OpenDate != null);

                if (order != null)
                {
                    s_dal.Order.Update(order with
                    {
                        Description = order.Description
                    });
                }
            }

            Observers.NotifyListUpdated();
        }
        finally
        {
            s_simulationMutex.UnsetInProgress();
        }
    }

    /// <summary>
    /// Calculates how many orders are in each ScheduleStatus
    /// (OnTime / AtRisk / Late).
    /// </summary>
    internal static IDictionary<BO.ScheduleStatus, int>
        GetOrdersCountByScheduleStatus()
    {
        var result = new Dictionary<BO.ScheduleStatus, int>
    {
        { BO.ScheduleStatus.OnTime, 0 },
        { BO.ScheduleStatus.SlightDelay, 0 },
        { BO.ScheduleStatus.Late, 0 }
    };

        foreach (var orderInList in GetAll())
        {
            var order = Get(orderInList.OrderId);

            if (result.ContainsKey(order.ScheduleStatus))
                result[order.ScheduleStatus]++;
        }

        return result;
    }


}
