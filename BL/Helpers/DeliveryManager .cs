using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers;

/// <summary>
/// Internal manager responsible for delivery-related business logic,
/// including creation, updates, retrieval and reporting.
/// </summary>
internal static class DeliveryManager
{
    internal static ObserverManager Observers = new();

    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Retrieves a delivery summary by its unique identifier.
    /// </summary>
    internal static BO.DeliveryPerOrderInList Get(int id)
    {
        DO.Delivery doDelivery = s_dal.Delivery.Read(id)
            ?? throw new BO.BlDoesNotExistException($"Delivery with ID={id} not found");

        DO.Courier? courier = null;
        if (doDelivery.CourierId != 0)
            courier = s_dal.Courier.Read(doDelivery.CourierId);

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
    internal static List<BO.DeliveryPerOrderInList> GetDeliveriesByOrder(int orderId)
    {
        return s_dal.Delivery.ReadAll(d => d.OrderId == orderId)
            .Select(d => Get(d.Id))
            .ToList();
    }

    /// <summary>
    /// Creates a new delivery that assigns an order to a courier.
    /// </summary>
    internal static void Create(int orderId, int courierId)
    {
        DO.Order? order = s_dal.Order.Read(orderId);
        if (order == null)
            throw new BO.BlDoesNotExistException("Order not found");

        DO.Courier? courier = s_dal.Courier.Read(courierId);
        if (courier == null)
            throw new BO.BlDoesNotExistException("Courier not found");

        DO.Delivery newDelivery = new(
            Id: 0,
            OrderId: orderId,
            CourierId: courierId,
            Type: (DO.OrderType)courier.Type,
            StartDeliveryDate: AdminManager.Now,
            ActualDistance: 0,
            ExpectedDistance: null,
            CompletionStatus: DO.DeliveryStatus.InProgress,
            EndDeliveryDate: null
        );

        s_dal.Delivery.Create(newDelivery);

        Observers.NotifyListUpdated();
        Observers.NotifyItemUpdated(orderId);
    }

    /// <summary>
    /// Updates the status of an existing delivery.
    /// </summary>
    internal static void UpdateStatus(int deliveryId, BO.DeliveryStatus newStatus)
    {
        DO.Delivery? d = s_dal.Delivery.Read(deliveryId);
        if (d == null)
            throw new BO.BlDoesNotExistException("Delivery not found");

        DateTime start = d.StartDeliveryDate;
        DateTime? end = d.EndDeliveryDate;

        if (newStatus == BO.DeliveryStatus.InProgress && start == DateTime.MinValue)
            start = AdminManager.Now;

        if (newStatus == BO.DeliveryStatus.Delivered)
            end = AdminManager.Now;

        DO.Delivery updated = d with
        {
            CompletionStatus = (DO.DeliveryStatus)newStatus,
            StartDeliveryDate = start,
            EndDeliveryDate = end
        };

        s_dal.Delivery.Update(updated);

        Observers.NotifyItemUpdated(deliveryId);
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Deletes a delivery by its identifier.
    /// </summary>
    internal static void Delete(int id)
    {
        s_dal.Delivery.Delete(id);

        Observers.NotifyItemUpdated(id);
        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Deletes all deliveries from the system.
    /// </summary>
    internal static void DeleteAll()
    {
        s_dal.Delivery.DeleteAll();

        Observers.NotifyListUpdated();
    }

    /// <summary>
    /// Retrieves all completed deliveries handled by a specific courier.
    /// </summary>
    internal static IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesByCourier(int courierId)
    {
        return s_dal.Delivery.ReadAll(d =>
                d.CourierId == courierId &&
                d.CompletionStatus == DO.DeliveryStatus.Delivered)
            .Select(d =>
            {
                var order = s_dal.Order.Read(d.OrderId)
                    ?? throw new BO.BlDoesNotExistException(
                        $"Order {d.OrderId} not found");

                return new BO.ClosedDeliveryInList
                {
                    DeliveryId = d.Id,
                    OrderId = d.OrderId,
                    Type = (BO.DeliveryType)d.Type,
                    Address = order.Address,
                    OrderType = (BO.OrderType)order.Type,
                    ActualDistance = d.ActualDistance,
                    TreatmentTime =
                        d.EndDeliveryDate!.Value - d.StartDeliveryDate,
                    CompletionStatus =
                        (BO.DeliveryStatus?)d.CompletionStatus
                };
            });
    }
}
