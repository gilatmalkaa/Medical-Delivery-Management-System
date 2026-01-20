using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers;

/// <summary>
/// Internal manager responsible for delivery-related business logic,
/// including creation, updates, retrieval and reporting.
/// </summary>
internal static class DeliveryManager
{
    /// <summary>
    /// Observer manager for delivery-related updates.
    /// Notifies subscribed UI components when delivery data changes.
    /// </summary>
    internal static ObserverManager Observers = new();

    /// <summary>
    /// Mutex used to prevent overlapping periodic delivery updates.
    /// </summary>
    private static readonly AsyncMutex s_periodicMutex = new(); // stage 7

    /// <summary>
    /// Mutex used to ensure single execution of delivery simulation logic.
    /// </summary>
    private static readonly AsyncMutex s_simulationMutex = new(); // stage 7


    /// <summary>
    /// Data access layer instance used by the order manager
    /// to perform CRUD operations on orders and deliveries.
    /// </summary>
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Retrieves a delivery summary by its unique identifier.
    /// </summary>
    internal static BO.DeliveryPerOrderInList Get(int id)
    {
        DO.Delivery doDelivery = s_dal.Delivery.Read(id)
            ?? throw new BO.BlDoesNotExistException($"Delivery with ID={id} not found");

        DO.Courier? courier =
            doDelivery.CourierId != 0
                ? s_dal.Courier.Read(doDelivery.CourierId)
                : null;

        return new BO.DeliveryPerOrderInList
        {
            DeliveryId = doDelivery.Id,
            CourierId = doDelivery.CourierId,
            CourierName = courier?.FullName ?? "Unknown",
            CourierType = courier != null
                ? (BO.CourierType)courier.Type
                : BO.CourierType.Unknown,
            StartDeliveryDate = doDelivery.StartDeliveryDate,
            CompletionStatus = (BO.DeliveryStatus?)doDelivery.CompletionStatus,
            EndDeliveryDate = doDelivery.EndDeliveryDate
        };
    }

    /// <summary>
    /// Retrieves all deliveries associated with a specific order.
    /// </summary>
    internal static IEnumerable<BO.DeliveryPerOrderInList>
        GetDeliveriesByOrder(int orderId)
    {
        return s_dal.Delivery
            .ReadAll(d => d.OrderId == orderId)
            .Select(d => Get(d.Id));
    }

    /// <summary>
    /// Async wrapper for retrieving deliveries by order.
    /// </summary>
    internal static Task<IEnumerable<BO.DeliveryPerOrderInList>>
        GetDeliveriesByOrderAsync(int orderId)
        => Task.FromResult(GetDeliveriesByOrder(orderId));

    /// <summary>
    /// Creates a new delivery that assigns an order to a courier.
    /// </summary>
    internal static void Create(int orderId, int courierId)
    {
        int newId;

        lock (AdminManager.BlMutex) 
        {
            DO.Order order = s_dal.Order.Read(orderId)
                ?? throw new BO.BlDoesNotExistException("Order not found");

            DO.Courier courier = s_dal.Courier.Read(courierId)
                ?? throw new BO.BlDoesNotExistException("Courier not found");

            newId =
                s_dal.Delivery.ReadAll().Any()
                    ? s_dal.Delivery.ReadAll().Max(d => d.Id) + 1
                    : 1;

            DO.Delivery newDelivery = new(
                Id: newId,
                OrderId: orderId,
                CourierId: courierId,
                Type: order.Type,
                StartDeliveryDate: AdminManager.Now,
                ActualDistance: 0,
                ExpectedDistance: null,
                CompletionStatus: DO.DeliveryStatus.InProgress,
                EndDeliveryDate: null
            );

            s_dal.Delivery.Create(newDelivery);
        }

        Observers.NotifyItemUpdated(newId);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Updates the status of an existing delivery.
    /// </summary>
    internal static void UpdateStatus(int deliveryId, BO.DeliveryStatus newStatus)
    {
        lock (AdminManager.BlMutex)
        {
            DO.Delivery delivery = s_dal.Delivery.Read(deliveryId)
                ?? throw new BO.BlDoesNotExistException("Delivery not found");

            DateTime start = delivery.StartDeliveryDate;
            DateTime? end = delivery.EndDeliveryDate;

            if (newStatus == BO.DeliveryStatus.Delivered)
                end = AdminManager.Now;

            DO.Delivery updated = delivery with
            {
                CompletionStatus = (DO.DeliveryStatus)newStatus,
                StartDeliveryDate = start,
                EndDeliveryDate = end
            };

            s_dal.Delivery.Update(updated);
        }

        Observers.NotifyItemUpdated(deliveryId);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Deletes a delivery by its identifier.
    /// </summary>
    internal static void Delete(int id)
    {
        lock (AdminManager.BlMutex) 
        {
            s_dal.Delivery.Delete(id);
        }

        Observers.NotifyItemUpdated(id);
        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Deletes all deliveries from the system.
    /// </summary>
    internal static void DeleteAll()
    {
        lock (AdminManager.BlMutex)
        {
            s_dal.Delivery.DeleteAll();
        }

        Observers.NotifyListUpdated();
    }


    /// <summary>
    /// Retrieves all completed deliveries handled by a specific courier.
    /// </summary>
    internal static IEnumerable<BO.ClosedDeliveryInList>
        GetClosedDeliveriesByCourier(int courierId)
    {
        return s_dal.Delivery
            .ReadAll(d =>
                d.CourierId == courierId &&
                d.CompletionStatus == DO.DeliveryStatus.Delivered)
            .Select(d =>
            {
                DO.Order order = s_dal.Order.Read(d.OrderId)
                    ?? throw new BO.BlDoesNotExistException(
                        $"Order {d.OrderId} not found");

                return new BO.ClosedDeliveryInList
                {
                    DeliveryId = d.Id,
                    OrderId = d.OrderId,
                    OrderType = (BO.OrderType)order.Type,
                    Address = order.Address,
                    ActualDistance = d.ActualDistance,
                    TreatmentTime =
                        d.EndDeliveryDate!.Value - d.StartDeliveryDate,
                    CompletionStatus =
                        (BO.DeliveryStatus?)d.CompletionStatus
                };
            });
    }

    /// <summary>
    /// Performs periodic delivery status updates based on system clock progression.
    /// Used by the simulator to complete deliveries automatically.
    /// </summary>
    internal static void PeriodicDeliveriesUpdates(DateTime oldClock, DateTime newClock)
    {
        if (s_periodicMutex.CheckAndSetInProgress())
            return;

        try
        {
            lock (AdminManager.BlMutex)
            {
                var deliveries =
                    s_dal.Delivery
                        .ReadAll(d => d.CompletionStatus == DO.DeliveryStatus.InProgress)
                        .ToList();

                foreach (var delivery in deliveries)
                {
                    if (delivery.ExpectedDistance == null)
                        continue;

                    var expectedEnd =
                        delivery.StartDeliveryDate +
                        TimeSpan.FromMinutes(delivery.ExpectedDistance.Value * 5);

                    if (newClock >= expectedEnd)
                    {
                        s_dal.Delivery.Update(delivery with
                        {
                            CompletionStatus = DO.DeliveryStatus.Delivered,
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
    /// Simulates delivery progress by incrementing actual distance
    /// for active deliveries during simulation runtime.
    /// </summary>
    internal static async Task SimulateDeliveriesAsync() // stage 7
    {
        if (s_simulationMutex.CheckAndSetInProgress())
            return;

        try
        {
            await Task.Delay(500);

            lock (AdminManager.BlMutex)
            {
                var delivery =
                    s_dal.Delivery.ReadAll()
                        .FirstOrDefault(d => d.CompletionStatus == DO.DeliveryStatus.InProgress);

                if (delivery != null)
                {
                    s_dal.Delivery.Update(delivery with
                    {
                        ActualDistance = delivery.ActualDistance + 1
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

    internal static void AssignOrdersAutomatically(DateTime from, DateTime to)
    {
        lock (AdminManager.BlMutex)
        {
            var freeCouriers = s_dal.Courier
                .ReadAll(c => c.IsActive)
                .Where(c => !s_dal.Delivery
                    .ReadAll(d => d.CourierId == c.Id && d.EndDeliveryDate == null)
                    .Any())
                .ToList();

            foreach (var courier in freeCouriers)
            {
                var order = s_dal.Order
                    .ReadAll(o => o.Status == DO.OrderStatus.Created)
                    .OrderBy(o => o.OpenDate)
                    .FirstOrDefault();

                if (order == null)
                    break;

                bool alreadyAssigned =
                    s_dal.Delivery.ReadAll(d =>
                        d.OrderId == order.Id &&
                        d.EndDeliveryDate == null).Any();

                if (alreadyAssigned)
                    continue;

                Create(order.Id, courier.Id);

                s_dal.Order.Update(order with
                {
                    Status = DO.OrderStatus.InDelivery
                });
            }
        }

        Observers.NotifyListUpdated();
    }




}
