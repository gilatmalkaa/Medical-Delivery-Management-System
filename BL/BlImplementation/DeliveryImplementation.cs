namespace BlImplementation;

using BlApi;
using BO;
using Helpers;
using System.Collections.Generic;


internal class DeliveryImplementation : IDelivery
{
    public DeliveryPerOrderInList Get(int id)
        => DeliveryManager.Get(id);

    public IEnumerable<DeliveryPerOrderInList> GetByOrder(int orderId)
        => DeliveryManager.GetDeliveriesByOrder(orderId);

    public void Create(int orderId, int courierId)
        => DeliveryManager.Create(orderId, courierId);

    public void UpdateStatus(int deliveryId, DeliveryStatus status)
        => DeliveryManager.UpdateStatus(deliveryId, status);
}
