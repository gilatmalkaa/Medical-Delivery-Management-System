namespace BlImplementation;

using BlApi;
using BO;
using Helpers;

/// <summary>
/// Delivery business logic implementation.
/// Handles creation, updating and retrieval of deliveries.
/// Supports Stage 5 observer pattern.
/// </summary>
internal class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Returns a single delivery by its identifier.
    /// </summary>
    public DeliveryPerOrderInList Get(int id)
        => DeliveryManager.Get(id);

    /// <summary>
    /// Returns all deliveries belonging to a specific order.
    /// </summary>
    public IEnumerable<DeliveryPerOrderInList> GetByOrder(int orderId)
        => DeliveryManager.GetDeliveriesByOrder(orderId);

    /// <summary>
    /// Returns deliveries, optionally filtered.
    /// </summary>
    public IEnumerable<DeliveryPerOrderInList> ReadAll(
        Func<DeliveryPerOrderInList, bool>? filter = null)
    {
        var deliveries =
            from order in OrderManager.ReadAll()
            from delivery in DeliveryManager.GetDeliveriesByOrder(order.Id)
            select delivery;

        return filter == null ? deliveries : deliveries.Where(filter);
    }

    /// <summary>
    /// Creates a new delivery for an order and courier.
    /// </summary>
    public void Create(int orderId, int courierId)
        => DeliveryManager.Create(orderId, courierId);

    /// <summary>
    /// Updates delivery status.
    /// </summary>
    public void UpdateStatus(int deliveryId, DeliveryStatus status)
        => DeliveryManager.UpdateStatus(deliveryId, status);

    // ===== Stage 5 Observer Support =====
    public void AddObserver(Action listObserver) =>
        DeliveryManager.Observers.AddListObserver(listObserver);

    public void AddObserver(int id, Action observer) =>
        DeliveryManager.Observers.AddObserver(id, observer);

    public void RemoveObserver(Action listObserver) =>
        DeliveryManager.Observers.RemoveListObserver(listObserver);

    public void RemoveObserver(int id, Action observer) =>
        DeliveryManager.Observers.RemoveObserver(id, observer);
}
