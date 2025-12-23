using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers;

internal static class DeliveryManager
{
    internal static ObserverManager Observers = new(); // stage 5

    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    internal static BO.DeliveryPerOrderInList Get(int id)
    {
        DO.Delivery? doDelivery = s_dal.Delivery.Read(id);
        if (doDelivery == null)
            throw new BO.BlDoesNotExistException($"Delivery with ID={id} not found");

        string courierName = "";
        if (doDelivery.CourierId != 0)
        {
            var c = s_dal.Courier.Read(doDelivery.CourierId);
            courierName = c?.FullName ?? "Unknown";
        }

        return new BO.DeliveryPerOrderInList
        {
            DeliveryId = doDelivery.Id,
            CourierId = doDelivery.CourierId,
            CourierName = courierName,
            Type = (BO.DeliveryType)doDelivery.Type,
            StartDeliveryDate = doDelivery.StartDeliveryDate,
            CompletionStatus = (BO.DeliveryStatus?)doDelivery.CompletionStatus,
            EndDeliveryDate = doDelivery.EndDeliveryDate
        };
    }

    internal static List<BO.DeliveryPerOrderInList> GetDeliveriesByOrder(int orderId)
    {
        return s_dal.Delivery.ReadAll(d => d.OrderId == orderId)
            .Select(d => Get(d.Id))
            .ToList();
    }

    internal static void Create(int orderId, int courierId)
    {
        DO.Order? order = s_dal.Order.Read(orderId);
        if (order == null) throw new BO.BlDoesNotExistException("Order not found");

        DO.Courier? courier = s_dal.Courier.Read(courierId);
        if (courier == null) throw new BO.BlDoesNotExistException("Courier not found");

        DO.Delivery newDelivery = new(
            Id: 0,
            OrderId: orderId,
            CourierId: courierId,
            Type: (DO.DeliveryType)courier.Type,
            StartDeliveryDate: DateTime.MinValue,
            ActualDistance: 0,
            ExpectedDistance: null,
            CompletionStatus: DO.DeliveryStatus.InProgress,
            EndDeliveryDate: null
        );

        s_dal.Delivery.Create(newDelivery);

        Observers.NotifyListUpdated();          // stage 5 
        Observers.NotifyItemUpdated(orderId);   // stage 5 
    }

    internal static void UpdateStatus(int deliveryId, BO.DeliveryStatus newStatus)
    {
        DO.Delivery? d = s_dal.Delivery.Read(deliveryId);
        if (d == null) throw new BO.BlDoesNotExistException("Delivery not found");

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

        Observers.NotifyItemUpdated(deliveryId); // stage 5 
        Observers.NotifyListUpdated();           // stage 5 
    }

    internal static void Delete(int id)
    {
        s_dal.Delivery.Delete(id);

        Observers.NotifyItemUpdated(id); // stage 5 
        Observers.NotifyListUpdated();   // stage 5 
    }
    internal static void DeleteAll()
    {
        s_dal.Delivery.DeleteAll();

        Observers.NotifyListUpdated(); // stage 5 
    }
}
