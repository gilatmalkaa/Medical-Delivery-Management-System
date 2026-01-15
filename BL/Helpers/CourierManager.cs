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
    internal static ObserverManager Observers = new();

    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new courier in the system.
    /// </summary>
    /// <param name="courier">Courier business object.</param>
    internal static void Create(BO.Courier courier)
    {
        int newId =
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
            StartWorkDate = courier.StartWorkDate == default
                ? DateTime.Now
                : courier.StartWorkDate
        };

        s_dal.Courier.Create(newDo);
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
                    expectedDeliveryTime - DateTime.Now;

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
                    OpenDate = order.OpenDate ?? DateTime.Now,
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
        DO.Courier? c = s_dal.Courier.Read(courier.Id);
        if (c == null)
            throw new BO.BlDoesNotExistException($"Courier with ID={courier.Id} not found");

        if (courier.MaxPersonalDeliveryDistance != c.MaxPersonalDeliveryDistance &&
            courier.MaxPersonalDeliveryDistance > s_dal.Config.MaxRange)
        {
            throw new BO.BlInvalidInputException(
                "Personal delivery distance exceeds company limit");
        }

        bool hasActiveDelivery =
            s_dal.Delivery.ReadAll(d =>
                d.CourierId == courier.Id &&
                d.CompletionStatus == DO.DeliveryStatus.InProgress).Any();

        if (hasActiveDelivery && c.Type != (DO.CourierType)courier.Type)
            throw new BO.BlInvalidOperationException(
                "Cannot change courier type while delivery is in progress");

        DO.Courier updated = c with
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
        if (s_dal.Delivery.ReadAll(d => d.CourierId == id).Any())
            throw new BO.BlPermissionException(
                "Cannot delete courier with deliveries");

        s_dal.Courier.Delete(id);
        Observers.NotifyItemUpdated(id);
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Deletes all couriers from the system.
    /// </summary>
    internal static void DeleteAll()
    {
        s_dal.Courier.DeleteAll();
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Retrieves open orders that a courier can accept.
    /// </summary>
    internal static IEnumerable<BO.OpenOrderInList> GetOpenOrdersForCourier(int courierId)
    {
        var courier = Get(courierId);
        var hubCoords = Tools.GetCoordinates("Main Logistics Center");

        return
            from order in s_dal.Order.ReadAll()
            where order.Status == DO.OrderStatus.Created
            where !s_dal.Delivery.ReadAll().Any(d => d.OrderId == order.Id)
            let orderCoords = Tools.GetCoordinates(order.Address)
            let airDistance =
                (orderCoords.Latitude == 0 && orderCoords.Longitude == 0)
                    ? 0
                    : Tools.CalcAirDistance(
                        hubCoords.Latitude,
                        hubCoords.Longitude,
                        orderCoords.Latitude,
                        orderCoords.Longitude)
            where courier.MaxPersonalDeliveryDistance == null
               || airDistance <= courier.MaxPersonalDeliveryDistance.Value
            select new BO.OpenOrderInList
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
            };
    }

    /// <summary>
    /// Assigns an open order to a courier.
    /// </summary>
    internal static void AssignOrder(int courierId, int orderId)
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
            StartDeliveryDate = DateTime.Now,
            CompletionStatus = DO.DeliveryStatus.InProgress
        });

        s_dal.Order.Update(order with { Status = DO.OrderStatus.InDelivery });
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Completes the active delivery of a courier.
    /// </summary>
    internal static void CompleteDelivery(int courierId)
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
            EndDeliveryDate = DateTime.Now
        });

        Observers.NotifyListUpdated();
    }
}
