using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers;

internal static class OrderManager
{
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    internal static BO.Order Get(int id)
    {
        DO.Order? doOrder = s_dal.Order.Read(id);
        if (doOrder == null)
            throw new BO.BlDoesNotExistException($"Order with ID={id} not found");

        List<BO.DeliveryPerOrderInList> deliveriesList = DeliveryManager.GetDeliveriesByOrder(id);

        BO.OrderStatus orderStatus = deliveriesList.Any(d => d.CompletionStatus == BO.DeliveryStatus.Delivered)
                                     ? BO.OrderStatus.Delivered
                                     : BO.OrderStatus.Created;

        return new BO.Order()
        {
            Id = doOrder.Id,
            Type = (BO.OrderType)doOrder.Type,
            Description = doOrder.Description,
            Address = doOrder.Address,
            Latitude = doOrder.Latitude ?? 0,
            Longitude = doOrder.Longitude ?? 0,
            CustomerName = doOrder.CustomerName,
            CustomerPhone = doOrder.CustomerPhone,
            PackageDetails = $"Weight: {doOrder.Weight}",
            CreatedAt = doOrder.OpenDate ?? DateTime.MinValue,
            Deliveries = deliveriesList,
            OrderStatus = orderStatus,
            ScheduleStatus = BO.ScheduleStatus.Scheduled,
            AirDistance = 0, // אפשר לחשב אם רוצים
        };
    }

    internal static IEnumerable<BO.OrderInList> GetAll()
    {
        return s_dal.Order.ReadAll()
            .Select(o => new BO.OrderInList
            {
                OrderId = o.Id,
                Type = (BO.OrderType)o.Type,
                AirDistance = 0, // או חישוב אם יש לך
                OrderStatus = BO.OrderStatus.Created,
                ScheduleStatus = BO.ScheduleStatus.Scheduled,
                DeliveriesCount = s_dal.Delivery.ReadAll(d => d.OrderId == o.Id).Count()
            });
    }

    internal static void Create(BO.Order order)
    {
        DO.Order newDo = new()
        {
            Id = 0,
            Type = (DO.OrderType)order.Type,
            Description = order.Description,
            Address = order.Address,
            Latitude = order.Latitude,
            Longitude = order.Longitude,
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            Weight = 0,
            OpenDate = order.CreatedAt
        };
        s_dal.Order.Create(newDo);
    }

    internal static BO.Order Read(int id) => Get(id);

    internal static IEnumerable<BO.Order> ReadAll(Func<BO.Order, bool>? filter = null)
    {
        var all = s_dal.Order.ReadAll().Select(o => Get(o.Id));
        return filter == null ? all : all.Where(filter);
    }

    internal static void Update(BO.Order order)
    {
        DO.Order? doOrder = s_dal.Order.Read(order.Id);
        if (doOrder == null) throw new BO.BlDoesNotExistException($"Order with ID={order.Id} not found");

        DO.Order updated = doOrder with
        {
            Type = (DO.OrderType)order.Type,
            Description = order.Description,
            Address = order.Address,
            Latitude = order.Latitude,
            Longitude = order.Longitude,
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone
        };

        s_dal.Order.Update(updated);
    }

    internal static void Delete(int id) => s_dal.Order.Delete(id);
    internal static void DeleteAll() => s_dal.Order.DeleteAll();

    internal static BO.Order Read(Func<BO.Order, bool> filter)
    {
        throw new NotImplementedException();
    }
}
