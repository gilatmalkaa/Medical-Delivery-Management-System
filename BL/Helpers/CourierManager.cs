using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers;

/// <summary>
/// Internal business logic manager responsible for
/// courier-related operations, availability handling,
/// delivery assignment, and courier statistics.
/// </summary>
internal static class CourierManager
{
    /// <summary>
    /// Observer manager for courier-related updates.
    /// Notifies subscribed UI components about changes.
    /// </summary>
    internal static ObserverManager Observers = new();

    /// <summary>
    /// Mutex used to prevent overlapping periodic courier updates.
    /// </summary>
    private static readonly AsyncMutex s_periodicMutex = new(); // stage 7

    /// <summary>
    /// Mutex used to ensure single execution of courier simulation logic.
    /// </summary>
    private static readonly AsyncMutex s_simulationMutex = new(); // stage 7


    /// <summary>
    /// Data access layer instance used by the order manager
    /// to perform CRUD operations on orders and deliveries.
    /// </summary>
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new courier in the system.
    /// </summary>
    /// <param name="courier">Courier business object.</param>
    internal static void Create(BO.Courier courier)
    {
        int newId;

        lock (AdminManager.BlMutex)
        {
            newId =
                s_dal.Courier.ReadAll().Any()
                    ? s_dal.Courier.ReadAll().Max(c => c.Id) + 1
                    : 1;

            DO.Courier newDo = new()
            {
                Id = newId,
                FullName = courier.Name ?? "",
                Password = courier.Password ?? "",
                Phone = courier.Phone,
                Email = courier.Email,
                Signature = courier.Signature,
                MaxPersonalDeliveryDistance = courier.MaxPersonalDeliveryDistance ?? 0,
                Type = (DO.CourierType)courier.Type,
                IsActive = courier.IsActive,
                StartWorkDate = AdminManager.Now
            };

            s_dal.Courier.Create(newDo);
        }

        Observers.NotifyListUpdated(); 
    }



    /// <summary>
    /// Retrieves a courier with full calculated status and statistics.
    /// </summary>
    /// <param name="id">Courier identifier.</param>
    /// <returns>Courier business object.</returns>
    internal static BO.Courier Get(int id)
    {
        const double KM_PER_MINUTE = 0.5;

        DO.Courier? c = s_dal.Courier.Read(id);
        if (c == null)
            throw new BO.BlDoesNotExistException($"Courier with ID={id} not found");

        var deliveries = s_dal.Delivery
            .ReadAll(d => d.CourierId == id)
            .ToList();

        int deliveredOnTime = deliveries.Count(d =>
            d.CompletionStatus == DO.DeliveryStatus.Delivered &&
            d.EndDeliveryDate != null &&
            d.ExpectedDistance != null &&
            d.EndDeliveryDate <=
                d.StartDeliveryDate +
                TimeSpan.FromMinutes(d.ExpectedDistance.Value / KM_PER_MINUTE)
        );

        int deliveredLate = deliveries.Count(d =>
            d.CompletionStatus == DO.DeliveryStatus.Delivered &&
            d.EndDeliveryDate != null &&
            d.ExpectedDistance != null &&
            d.EndDeliveryDate >
                d.StartDeliveryDate +
                TimeSpan.FromMinutes(d.ExpectedDistance.Value / KM_PER_MINUTE)
        );

        var activeDelivery = deliveries
            .FirstOrDefault(d => d.CompletionStatus == DO.DeliveryStatus.InProgress);

        BO.OrderInProgress? currentOrder = null;

        if (activeDelivery != null)
        {
            var order = s_dal.Order.Read(activeDelivery.OrderId);
            if (order != null)
            {
                double distance = activeDelivery.ExpectedDistance ?? 0;

                TimeSpan expectedDuration =
                    TimeSpan.FromMinutes(distance / KM_PER_MINUTE);

                DateTime expectedDeliveryTime =
                    activeDelivery.StartDeliveryDate + expectedDuration;

                TimeSpan remaining =
                    expectedDeliveryTime - AdminManager.Now;

                currentOrder = new BO.OrderInProgress
                {
                    DeliveryId = activeDelivery.Id,
                    OrderId = order.Id,
                    Type = (BO.OrderType)order.Type,
                    Description = order.Description,
                    Address = order.Address,
                    AirDistance = activeDelivery.ExpectedDistance ?? 0,
                    ActualDistance = activeDelivery.ActualDistance,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    OpenDate = order.OpenDate ?? AdminManager.Now,
                    StartDeliveryDate = activeDelivery.StartDeliveryDate,
                    ExpectedArrivalTime = expectedDeliveryTime,
                    LatestSupplyTime = expectedDeliveryTime,
                    OrderStatus = BO.OrderStatus.InDelivery,
                    ScheduleStatus =
                        remaining > TimeSpan.Zero
                            ? BO.ScheduleStatus.OnTime
                            : BO.ScheduleStatus.Late,
                    RemainingTimeToFinishOrder =
                        remaining > TimeSpan.Zero
                            ? remaining
                            : TimeSpan.Zero
                };
            }
        }

        bool isAvailable =
            c.IsActive &&
            !deliveries.Any(d =>
                d.CompletionStatus == DO.DeliveryStatus.InProgress ||
                d.CompletionStatus == DO.DeliveryStatus.Pending);

        return new BO.Courier
        {
            Id = c.Id,
            Name = c.FullName,
            Password = c.Password,
            Phone = c.Phone,
            Email = c.Email,
            Signature = c.Signature,
            MaxPersonalDeliveryDistance = c.MaxPersonalDeliveryDistance,
            Type = (BO.DeliveryType)c.Type,
            IsActive = c.IsActive,
            IsAvailable = isAvailable,
            StartWorkDate = c.StartWorkDate,
            DeliveredOnTimeCount = deliveredOnTime,
            DeliveredLateCount = deliveredLate,
            CurrentOrder = currentOrder
        };
    }

    /// <summary>
    /// Retrieves all couriers with optional filtering.
    /// </summary>
    internal static IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null)
    {
        var all = s_dal.Courier.ReadAll().Select(c => Get(c.Id));
        return filter == null ? all : all.Where(filter);
    }

    /// <summary>
    /// Updates an existing courier's details.
    /// </summary>
    internal static void Update(BO.Courier courier)
    {
        DO.Courier updated;

        lock (AdminManager.BlMutex) 
        {
            DO.Courier? c = s_dal.Courier.Read(courier.Id);
            if (c == null)
                throw new BO.BlDoesNotExistException($"Courier with ID={courier.Id} not found");

            bool hasActiveDelivery =
                s_dal.Delivery.ReadAll(d =>
                    d.CourierId == courier.Id &&
                    d.CompletionStatus == DO.DeliveryStatus.InProgress).Any();

            if (hasActiveDelivery && c.Type != (DO.CourierType)courier.Type)
                throw new BO.BlInvalidOperationException(
                    "Cannot change courier type while delivery is in progress");

            updated = c with
            {
                FullName = courier.Name ?? "",
                Password = courier.Password,
                Phone = courier.Phone,
                Email = courier.Email,
                Signature = courier.Signature,
                MaxPersonalDeliveryDistance = courier.MaxPersonalDeliveryDistance ?? 0,
                Type = (DO.CourierType)courier.Type,
                IsActive = courier.IsActive,
                StartWorkDate = courier.StartWorkDate
            };

            s_dal.Courier.Update(updated);
        }

        Observers.NotifyItemUpdated(courier.Id);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Retrieves lightweight courier summaries for list displays.
    /// </summary>
    internal static IEnumerable<BO.CourierInList> GetAll()
    {
        return s_dal.Courier.ReadAll()
            .Select(c =>
            {
                var deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == c!.Id).ToList();

                return new BO.CourierInList
                {
                    Id = c!.Id,
                    FullName = c.FullName,
                    Type = (BO.CourierType)c.Type,
                    IsActive = c.IsActive,
                    StartWorkDate = c.StartWorkDate,
                    TotalDeliveriesOnTime = deliveries.Count(d => d.CompletionStatus == DO.DeliveryStatus.Delivered),
                    TotalDeliveriesLate = deliveries.Count(d => d.CompletionStatus == DO.DeliveryStatus.Canceled),
                    TotalDeliveries = deliveries.Count,
                    CanDelete = !deliveries.Any()
                };
            })
            .ToList();
    }

    /// <summary>
    /// Deletes a courier if no deliveries exist.
    /// </summary>
    internal static void Delete(int id)
    {
        lock (AdminManager.BlMutex)
        {
            if (s_dal.Delivery.ReadAll(d => d.CourierId == id).Any())
                throw new BO.BlPermissionException(
                    "Cannot delete courier with deliveries");

            s_dal.Courier.Delete(id);
        }

        Observers.NotifyItemUpdated(id);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Deletes all couriers from the system.
    /// </summary>
    internal static void DeleteAll()
    {
        lock (AdminManager.BlMutex)
        {
            s_dal.Courier.DeleteAll();
        }

        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Retrieves open orders that a courier can accept.
    /// </summary>
    internal static async Task<IEnumerable<BO.OpenOrderInList>>
     GetOpenOrdersForCourierAsync(int courierId)
    {
        var courier = Get(courierId);
        var hubCoords =
            await Tools.GetCoordinatesCachedAsync("Main Logistics Center");

        var result = new List<BO.OpenOrderInList>();

        foreach (var order in s_dal.Order.ReadAll())
        {
            if (order.Status != DO.OrderStatus.Created)
                continue;

            if (s_dal.Delivery.ReadAll().Any(d => d.OrderId == order.Id))
                continue;

            var orderCoords =
                await Tools.GetCoordinatesCachedAsync(order.Address);

            double airDistance =
                (orderCoords.Latitude == 0 && orderCoords.Longitude == 0)
                    ? 0
                    : Tools.CalcAirDistance(
                        hubCoords.Latitude,
                        hubCoords.Longitude,
                        orderCoords.Latitude,
                        orderCoords.Longitude);

            if (courier.MaxPersonalDeliveryDistance != null &&
                airDistance > courier.MaxPersonalDeliveryDistance.Value)
                continue;

            result.Add(new BO.OpenOrderInList
            {
                CourierId = null,
                OrderId = order.Id,
                Type = (BO.OrderType)order.Type,
                ItemCategory = order.Description,
                Address = order.Address,
                AirDistance = airDistance,
                ScheduleStatus = BO.ScheduleStatus.Scheduled,
                RemainingTime = TimeSpan.Zero,
                EndTime = DateTime.MinValue
            });
        }

        return result;
    }


    /// <summary>
    /// Assigns an open order to a courier.
    /// </summary>
    internal static void AssignOrder(int courierId, int orderId)
    {
        lock (AdminManager.BlMutex) 
        {
            var courier = Get(courierId);
            if (!courier.IsAvailable)
                throw new InvalidOperationException("Courier is not available");

            var order = s_dal.Order.Read(orderId)
                ?? throw new InvalidOperationException("Order does not exist");

            if (order.Status != DO.OrderStatus.Created)
                throw new InvalidOperationException("Order is not open");

            int newDeliveryId =
                s_dal.Delivery.ReadAll().Any()
                    ? s_dal.Delivery.ReadAll().Max(d => d.Id) + 1
                    : 1;

            s_dal.Delivery.Create(new DO.Delivery
            {
                Id = newDeliveryId,
                OrderId = orderId,
                CourierId = courierId,
                StartDeliveryDate = AdminManager.Now,
                CompletionStatus = DO.DeliveryStatus.InProgress
            });

            s_dal.Order.Update(order with { Status = DO.OrderStatus.InDelivery });
        }

        Observers.NotifyItemUpdated(courierId);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Completes the active delivery of a courier.
    /// </summary>
    internal static void CompleteDelivery(int courierId)
    {
        lock (AdminManager.BlMutex)
        {
            var delivery = s_dal.Delivery.ReadAll(d =>
                d.CourierId == courierId &&
                d.CompletionStatus == DO.DeliveryStatus.InProgress)
                .FirstOrDefault();

            if (delivery == null)
                throw new BO.BlInvalidOperationException("No active delivery");

            s_dal.Delivery.Update(delivery with
            {
                CompletionStatus = DO.DeliveryStatus.Delivered,
                EndDeliveryDate = AdminManager.Now
            });
        }

        Observers.NotifyItemUpdated(courierId);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Synchronous wrapper for async open-orders retrieval.
    /// </summary>
    internal static IEnumerable<BO.OpenOrderInList> GetOpenOrdersForCourier(int courierId)
        => GetOpenOrdersForCourierAsync(courierId)
            .GetAwaiter()
            .GetResult();

    /// <summary>
    /// Async wrapper for Get (required by async BL workflows).
    /// </summary>
    internal static Task<BO.Courier> GetAsync(int id)
        => Task.FromResult(Get(id));

    /// <summary>
    /// Performs periodic updates for couriers based on system clock changes.
    /// Used by the simulator to randomly cancel deliveries.
    /// </summary>
    /// <param name="oldClock">Previous system time.</param>
    /// <param name="newClock">Updated system time.</param>
    internal static void PeriodicCouriersUpdates(DateTime oldClock, DateTime newClock)
    {
        if (s_periodicMutex.CheckAndSetInProgress())
            return;

        try
        {
            lock (AdminManager.BlMutex)
            {
                var couriers = s_dal.Courier.ReadAll().ToList();

                foreach (var courier in couriers)
                {
                    if (!courier.IsActive)
                        continue;

                    var activeDelivery =
                        s_dal.Delivery.ReadAll(d =>
                            d.CourierId == courier.Id &&
                            d.CompletionStatus == DO.DeliveryStatus.InProgress)
                            .FirstOrDefault();

                    if (activeDelivery == null)
                        continue;

                    if (Random.Shared.NextDouble() <= 0.15)
                    {
                        s_dal.Delivery.Update(activeDelivery with
                        {
                            CompletionStatus = DO.DeliveryStatus.Canceled,
                            EndDeliveryDate = newClock
                        });
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
    /// Simulates courier activity as part of the system simulator.
    /// </summary>
    internal static async Task SimulateCouriersAsync() // stage 7
    {
        if (s_simulationMutex.CheckAndSetInProgress())
            return;

        try
        {
            await Task.Delay(500);

            lock (AdminManager.BlMutex)
            {
                var courier =
                    s_dal.Courier.ReadAll()
                        .FirstOrDefault(c => c.IsActive);

                if (courier != null)
                {
                    s_dal.Courier.Update(courier with
                    {
                        IsActive = courier.IsActive 
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


}
