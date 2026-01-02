using BlApi;
using BO;
using Helpers;

/// <summary>
/// Implementation of courier related business logic service.
/// Delegates logic to <see cref="CourierManager"/>.
/// Supports observer mechanism (Stage 5).
/// </summary>
internal class CourierImplementation : ICourier
{
    public void Create(Courier item) => CourierManager.Create(item);

    public void Delete(int id) => CourierManager.Delete(id);

    public Courier Get(int id) => CourierManager.Get(id);

    public IEnumerable<CourierInList> GetAll() => CourierManager.GetAll();

    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
        => CourierManager.ReadAll(filter);

    public void Update(Courier item) => CourierManager.Update(item);

    // ===== Stage 5 Observer Support =====

    /// <summary>
    /// Registers observer for courier list changes.
    /// </summary>
    public void AddObserver(Action listObserver) =>
        CourierManager.Observers.AddListObserver(listObserver);

    /// <summary>
    /// Registers observer for specific courier updates.
    /// </summary>
    public void AddObserver(int id, Action observer) =>
        CourierManager.Observers.AddObserver(id, observer);

    /// <summary>
    /// Removes observer from list notifications.
    /// </summary>
    public void RemoveObserver(Action listObserver) =>
        CourierManager.Observers.RemoveListObserver(listObserver);

    /// <summary>
    /// Removes observer from specific courier notifications.
    /// </summary>
    public void RemoveObserver(int id, Action observer) =>
        CourierManager.Observers.RemoveObserver(id, observer);
}
