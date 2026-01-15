namespace BlApi;

/// <summary>
/// Business logic service for managing orders.
/// Handles order creation, updates, retrieval,
/// cancellation, and order listing operations.
/// </summary>
public interface IOrder : IObservable
{
    /// <summary>
    /// Creates a new order in the system.
    /// </summary>
    /// <param name="order">Order object to create.</param>
    /// <returns>The newly created order.</returns>
    BO.Order Create(BO.Order order);

    /// <summary>
    /// Updates an existing order with new details.
    /// </summary>
    /// <param name="order">Order object containing updated data.</param>
    /// <returns>The updated order.</returns>
    BO.Order Update(BO.Order order);

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>The corresponding order object.</returns>
    BO.Order Get(int id);

    /// <summary>
    /// Cancels an existing order and updates its status accordingly.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    void Cancel(int orderId);

    /// <summary>
    /// Retrieves a summarized list of all orders
    /// for display and management purposes.
    /// </summary>
    /// <returns>A collection of order summary objects.</returns>
    IEnumerable<BO.OrderInList> GetAll();

    /// <summary>
    /// Returns all open orders that a specific courier
    /// may choose from.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of open orders available to the courier.</returns>
    IEnumerable<BO.OpenOrderInList> GetOpenOrdersForCourier(int courierId);
}
