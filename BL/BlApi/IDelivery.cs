namespace BlApi;

/// <summary>
/// Business logic service for managing deliveries.
/// </summary>
public interface IDelivery : IObservable
{
    /// <summary>
    /// Creates a new delivery for given order and courier.
    /// </summary>
    void Create(int orderId, int courierId);

    /// <summary>
    /// Updates the status of an existing delivery.
    /// </summary>
    void UpdateStatus(int deliveryId, BO.DeliveryStatus status);

    /// <summary>
    /// Returns a delivery by its identifier.
    /// </summary>
    BO.DeliveryPerOrderInList Get(int id);

    /// <summary>
    /// Reads deliveries with optional filter.
    /// </summary>
    IEnumerable<BO.DeliveryPerOrderInList> ReadAll(
        Func<BO.DeliveryPerOrderInList, bool>? filter = null);

    /// <summary>
    /// Retrieves all completed deliveries
    /// assigned to a specific courier.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of closed deliveries for the courier.</returns>
    IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesByCourier(int courierId);

}
