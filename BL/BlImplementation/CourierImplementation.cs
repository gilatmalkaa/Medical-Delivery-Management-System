using BlApi;
using BO;
using Helpers;

/// <summary>
/// Concrete implementation of the ICourier interface.
/// Acts as a facade that delegates all courier-related
/// business logic to the CourierManager helper class.
/// Supports observer notifications (Stage 5).
/// </summary>
internal class CourierImplementation : ICourier
{
    /// <summary>
    /// Creates a new courier in the system.
    /// </summary>
    /// <param name="item">Courier business object to create.</param>
    public void Create(Courier item) => CourierManager.Create(item);

    /// <summary>
    /// Deletes an existing courier by its identifier.
    /// </summary>
    /// <param name="id">Courier identifier.</param>
    public void Delete(int id) => CourierManager.Delete(id);

    /// <summary>
    /// Retrieves a courier by its unique identifier.
    /// </summary>
    /// <param name="id">Courier identifier.</param>
    /// <returns>The corresponding courier object.</returns>
    public Courier Get(int id) => CourierManager.Get(id);

    /// <summary>
    /// Retrieves a summarized list of all couriers
    /// for display and management purposes.
    /// </summary>
    /// <returns>A collection of courier summary objects.</returns>
    public IEnumerable<CourierInList> GetAll() => CourierManager.GetAll();

    /// <summary>
    /// Retrieves all couriers, optionally filtered
    /// by a specified predicate.
    /// </summary>
    /// <param name="filter">Optional filter function.</param>
    /// <returns>A collection of couriers.</returns>
    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
        => CourierManager.ReadAll(filter);

    /// <summary>
    /// Updates an existing courier with new data.
    /// </summary>
    /// <param name="item">Courier object containing updated information.</param>
    public void Update(Courier item) => CourierManager.Update(item);

    // ===== Stage 5 Observer Support =====

    /// <summary>
    /// Registers an observer that is notified when
    /// the courier list is updated.
    /// </summary>
    /// <param name="listObserver">Callback to invoke on list changes.</param>
    public void AddObserver(Action listObserver) =>
        CourierManager.Observers.AddListObserver(listObserver);

    /// <summary>
    /// Registers an observer that is notified when
    /// a specific courier is updated.
    /// </summary>
    /// <param name="id">Courier identifier.</param>
    /// <param name="observer">Callback to invoke on courier updates.</param>
    public void AddObserver(int id, Action observer) =>
        CourierManager.Observers.AddObserver(id, observer);

    /// <summary>
    /// Removes an observer from courier list notifications.
    /// </summary>
    /// <param name="listObserver">The observer callback to remove.</param>
    public void RemoveObserver(Action listObserver) =>
        CourierManager.Observers.RemoveListObserver(listObserver);

    /// <summary>
    /// Removes an observer from notifications
    /// of a specific courier.
    /// </summary>
    /// <param name="id">Courier identifier.</param>
    /// <param name="observer">The observer callback to remove.</param>
    public void RemoveObserver(int id, Action observer) =>
        CourierManager.Observers.RemoveObserver(id, observer);

    /// <summary>
    /// Assigns an open order to the specified courier.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <param name="orderId">Order identifier.</param>
    public void AssignOrder(int courierId, int orderId)
    {
        CourierManager.AssignOrder(courierId, orderId);
    }

    /// <summary>
    /// Completes the courier's currently active delivery.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    public void CompleteDelivery(int courierId) =>
        CourierManager.CompleteDelivery(courierId);

    /// <summary>
    /// Retrieves all open orders that the specified courier
    /// is eligible to choose from.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of open orders available to the courier.</returns>
    public IEnumerable<BO.OpenOrderInList> GetOpenOrdersForCourier(int courierId)
    {
        return Helpers.CourierManager.GetOpenOrdersForCourier(courierId);
    }
}
