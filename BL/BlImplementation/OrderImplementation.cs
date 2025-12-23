using BlApi;
using Helpers;

internal class OrderImplementation : IOrder
{
    // --- Fetch a single order by ID ---
    public BO.Order Get(int id)
    {
        return OrderManager.Get(id); // wraps the helper
    }

    // --- Create a new order ---
    public void Create(BO.Order order)
    {
        OrderManager.Create(order); // implement Create in OrderManager
    }

    // --- Update existing order ---
    public void Update(BO.Order order)
    {
        OrderManager.Update(order); // implement Update in OrderManager
    }

    // --- Delete a single order ---
    public void Delete(int id)
    {
        OrderManager.Delete(id); // implement Delete in OrderManager
    }

    // --- Delete all orders ---
    public void DeleteAll()
    {
        OrderManager.DeleteAll(); // implement DeleteAll in OrderManager
    }

    // --- Get all orders ---
    public IEnumerable<BO.Order> ReadAll(Func<BO.Order, bool>? filter = null)
    {
        return OrderManager.ReadAll(filter);
    }


    // --- Read with filter (single order) ---
    public BO.Order Read(Func<BO.Order, bool> filter)
    {
        return OrderManager.Read(filter);
    }

    // --- Optional: summary list of orders ---
    public IEnumerable<BO.OrderInList> GetAll()
    {
        return OrderManager.GetAll(); // implement GetAll in OrderManager
    }

   //region Stage 5 - Observer
    public void AddObserver(Action listObserver) =>
        OrderManager.Observers.AddListObserver(listObserver);

    public void AddObserver(int id, Action observer) =>
        OrderManager.Observers.AddObserver(id, observer);

    public void RemoveObserver(Action listObserver) =>
        OrderManager.Observers.RemoveListObserver(listObserver);

    public void RemoveObserver(int id, Action observer) =>
        OrderManager.Observers.RemoveObserver(id, observer);
}