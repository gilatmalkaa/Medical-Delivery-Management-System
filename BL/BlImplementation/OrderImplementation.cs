using BlApi;
using BO;
using Helpers;

/// <summary>
/// Implementation of the order business logic service.
/// Delegates behavior to OrderManager.
/// Includes support for Stage 5 observer notifications.
/// </summary>
internal class OrderImplementation : IOrder
{
    /// <summary>
    /// Returns order by ID.
    /// </summary>
    public BO.Order Get(int id) => OrderManager.Get(id);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    public void Create(BO.Order order) => OrderManager.Create(order);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    public void Update(BO.Order order) => OrderManager.Update(order);

    public void Delete(int id) => OrderManager.Delete(id);

    public void DeleteAll() => OrderManager.DeleteAll();

    public IEnumerable<BO.Order> ReadAll(Func<BO.Order, bool>? filter = null)
        => OrderManager.ReadAll(filter);

    public BO.Order Read(Func<BO.Order, bool> filter)
        => OrderManager.Read(filter);

    public IEnumerable<BO.OrderInList> GetAll()
        => OrderManager.GetAll();

    // ===== Stage 5 Observer Support =====

    public void AddObserver(Action listObserver) =>
        OrderManager.Observers.AddListObserver(listObserver);

    public void AddObserver(int id, Action observer) =>
        OrderManager.Observers.AddObserver(id, observer);

    public void RemoveObserver(Action listObserver) =>
        OrderManager.Observers.RemoveListObserver(listObserver);

    public void RemoveObserver(int id, Action observer) =>
        OrderManager.Observers.RemoveObserver(id, observer);
}
