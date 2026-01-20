namespace BlImplementation;

using BlApi;
using BO;
using Helpers;

/// <summary>
/// Concrete implementation of the IDelivery interface.
/// Acts as a facade that delegates all delivery-related
/// business logic to the DeliveryManager helper class.
/// Supports observer notifications (Stage 5).
/// </summary>
internal class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Retrieves delivery details by its unique identifier.
    /// </summary>
    /// <param name="id">Delivery identifier.</param>
    /// <returns>Delivery details associated with an order.</returns>
    public DeliveryPerOrderInList Get(int id)
        => DeliveryManager.Get(id);

    /// <summary>
    /// Retrieves all deliveries associated with a specific order.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <returns>A collection of deliveries for the given order.</returns>
    public IEnumerable<DeliveryPerOrderInList> GetByOrder(int orderId)
        => DeliveryManager.GetDeliveriesByOrder(orderId);

    /// <summary>
    /// Retrieves all deliveries in the system,
    /// optionally filtered by a specified predicate.
    /// </summary>
    /// <param name="filter">Optional filter function.</param>
    /// <returns>A collection of delivery summary objects.</returns>
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
    /// Creates a new delivery by linking an order
    /// to a courier and initializing its lifecycle.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    /// <param name="courierId">Courier identifier.</param>
    public void Create(int orderId, int courierId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        DeliveryManager.Create(orderId, courierId);
    }

    /// <summary>
    /// Updates the status of an existing delivery.
    /// </summary>
    /// <param name="deliveryId">Delivery identifier.</param>
    /// <param name="status">New delivery status.</param>
    public void UpdateStatus(int deliveryId, DeliveryStatus status)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        DeliveryManager.UpdateStatus(deliveryId, status);
    }

    // ===== Stage 5 Observer Support =====

    /// <summary>
    /// Registers an observer that is notified
    /// when the delivery list is updated.
    /// </summary>
    /// <param name="listObserver">Callback to invoke on list changes.</param>
    public void AddObserver(Action listObserver) =>
        DeliveryManager.Observers.AddListObserver(listObserver);

    /// <summary>
    /// Registers an observer that is notified
    /// when a specific delivery is updated.
    /// </summary>
    /// <param name="id">Delivery identifier.</param>
    /// <param name="observer">Callback to invoke on delivery updates.</param>
    public void AddObserver(int id, Action observer) =>
        DeliveryManager.Observers.AddObserver(id, observer);

    /// <summary>
    /// Removes an observer from delivery list notifications.
    /// </summary>
    /// <param name="listObserver">The observer callback to remove.</param>
    public void RemoveObserver(Action listObserver) =>
        DeliveryManager.Observers.RemoveListObserver(listObserver);

    /// <summary>
    /// Removes an observer from notifications
    /// of a specific delivery.
    /// </summary>
    /// <param name="id">Delivery identifier.</param>
    /// <param name="observer">The observer callback to remove.</param>
    public void RemoveObserver(int id, Action observer) =>
        DeliveryManager.Observers.RemoveObserver(id, observer);

    /// <summary>
    /// Retrieves all completed deliveries
    /// assigned to a specific courier.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of closed deliveries for the courier.</returns>
    public IEnumerable<ClosedDeliveryInList> GetClosedDeliveriesByCourier(int courierId)
        => DeliveryManager.GetClosedDeliveriesByCourier(courierId);
}
