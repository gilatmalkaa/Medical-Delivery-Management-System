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

    //region Stage 5 - Observer
    public void AddObserver(Action listObserver) =>
        DeliveryManager.Observers.AddListObserver(listObserver);

    public void AddObserver(int id, Action observer) =>
        DeliveryManager.Observers.AddObserver(id, observer);

    public void RemoveObserver(Action listObserver) =>
        DeliveryManager.Observers.RemoveListObserver(listObserver);

    public void RemoveObserver(int id, Action observer) =>
        DeliveryManager.Observers.RemoveObserver(id, observer);
}
