using BlApi;
using BO;
using Helpers;

/// <summary>
/// Concrete implementation of the IOrder interface.
/// Acts as a facade that delegates all order-related
/// business logic to the OrderManager helper class.
/// Includes observer support (Stage 5).
/// </summary>
internal class OrderImplementation : IOrder
{
    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>The corresponding order object.</returns>
    public BO.Order Get(int id) => OrderManager.Get(id);

    /// <summary>
    /// Creates a new order in the system.
    /// </summary>
    /// <param name="order">Order object to create.</param>
    public void Create(BO.Order order)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Create(order);
    }

    /// <summary>
    /// Updates an existing order with new details.
    /// </summary>
    /// <param name="order">Order object containing updated data.</param>
    public void Update(BO.Order order)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Update(order);
    }

    /// <summary>
    /// Deletes an order by its identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    public void Delete(int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Delete(id);
    }

    /// <summary>
    /// Deletes all orders from the system.
    /// </summary>
    public void DeleteAll()
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.DeleteAll();
    }

    /// <summary>
    /// Retrieves all orders, optionally filtered
    /// by a specified predicate.
    /// </summary>
    /// <param name="filter">Optional filter function.</param>
    /// <returns>A collection of orders.</returns>
    public IEnumerable<BO.Order> ReadAll(Func<BO.Order, bool>? filter = null)
        => OrderManager.ReadAll(filter);

    /// <summary>
    /// Retrieves a single order matching the given predicate.
    /// </summary>
    /// <param name="filter">Filter condition.</param>
    /// <returns>The first matching order.</returns>
    public BO.Order Read(Func<BO.Order, bool> filter)
        => OrderManager
            .ReadAll(filter)
            .FirstOrDefault()
            ?? throw new BO.BlDoesNotExistException("Order not found");


    /// <summary>
    /// Retrieves a summarized list of all orders
    /// for display and management purposes.
    /// </summary>
    /// <returns>A collection of order summary objects.</returns>
    public IEnumerable<BO.OrderInList> GetAll()
        => OrderManager.GetAll();

    // ===== Stage 5 Observer Support =====

    /// <summary>
    /// Registers an observer that is notified
    /// when the order list is updated.
    /// </summary>
    /// <param name="listObserver">Callback to invoke on list changes.</param>
    public void AddObserver(Action listObserver) =>
        OrderManager.Observers.AddListObserver(listObserver);

    /// <summary>
    /// Registers an observer that is notified
    /// when a specific order is updated.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="observer">Callback to invoke on order updates.</param>
    public void AddObserver(int id, Action observer) =>
        OrderManager.Observers.AddObserver(id, observer);

    /// <summary>
    /// Removes an observer from order list notifications.
    /// </summary>
    /// <param name="listObserver">The observer callback to remove.</param>
    public void RemoveObserver(Action listObserver) =>
        OrderManager.Observers.RemoveListObserver(listObserver);

    /// <summary>
    /// Removes an observer from notifications
    /// of a specific order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="observer">The observer callback to remove.</param>
    public void RemoveObserver(int id, Action observer) =>
        OrderManager.Observers.RemoveObserver(id, observer);

    /// <summary>
    /// Explicit interface implementation for order creation.
    /// Returns the created order instance.
    /// </summary>
    BO.Order IOrder.Create(BO.Order order)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Create(order);
        return order;
    }

    /// <summary>
    /// Explicit interface implementation for order update.
    /// Returns the updated order instance.
    /// </summary>

    BO.Order IOrder.Update(BO.Order order)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Update(order);
        return OrderManager.Get(order.Id);
    }

    /// <summary>
    /// Cancels an existing order and updates its status.
    /// </summary>
    /// <param name="orderId">Order identifier.</param>
    public void Cancel(int orderId)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
        OrderManager.Cancel(orderId);
    }

    /// <summary>
    /// Retrieves all open orders that a specific courier
    /// may choose from.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of open orders available to the courier.</returns>
    public IEnumerable<OpenOrderInList> GetOpenOrdersForCourier(int courierId)
        => OrderManager.GetOpenOrdersForCourier(courierId);
}
