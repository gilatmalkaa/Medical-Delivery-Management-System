namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Implements the IOrder interface to manage order entities.
/// Provides CRUD operations on in-memory list DataSource.Orders.
/// </summary>
public class OrderImplementation : IOrder
{
    /// <summary>
    /// Creates a new order in the data source.
    /// If the ID is 0, assigns a running ID automatically.
    /// Throws an exception if an order with the same ID already exists.
    /// </summary>
    /// <param name="item">Order to create.</param>
    /// <exception cref="Exception">Thrown if an order with the same ID already exists.</exception>
    public void Create(Order item)
    {
        if (item.Id == 0)
        {
            int newId = DataSource.Config.NextOrderId;
            Order newOrder = item with { Id = newId };
            DataSource.Orders.Add(newOrder);
        }
        else
        {
            if (Read(item.Id) != null)
                throw new Exception($"Order with ID {item.Id} already exists.");

            DataSource.Orders.Add(item);
        }
    }

    /// <summary>
    /// Reads an order entity by ID.
    /// </summary>
    /// <param name="id">Order ID.</param>
    /// <returns>Order entity if found.</returns>
    /// <exception cref="Exception">Thrown if the order does not exist.</exception>
    public Order? Read(int id)
    {
        Order? order = DataSource.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new Exception($"Order with ID={id} does not exist");
        return order;
    }

    /// <summary>
    /// Returns all orders currently in the data source.
    /// </summary>
    /// <returns>List of all orders.</returns>
    public List<Order> ReadAll()
    {
        return new List<Order>(DataSource.Orders);
    }

    /// <summary>
    /// Updates an existing order by replacing it in the data source.
    /// </summary>
    /// <param name="item">Updated order entity.</param>
    /// <exception cref="Exception">Thrown if the order does not exist.</exception>
    public void Update(Order item)
    {
        Order? existing = DataSource.Orders.FirstOrDefault(o => o.Id == item.Id);
        if (existing == null)
            throw new Exception($"Order with ID={item.Id} does not exist");

        DataSource.Orders.Remove(existing);
        DataSource.Orders.Add(item);
    }

    /// <summary>
    /// Deletes an order entity by ID.
    /// </summary>
    /// <param name="id">Order ID to delete.</param>
    /// <exception cref="Exception">Thrown if the order does not exist.</exception>
    public void Delete(int id)
    {
        Order? order = DataSource.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new Exception($"Order with ID={id} does not exist");
        DataSource.Orders.Remove(order);
    }

    /// <summary>
    /// Clears all orders from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Orders.Clear();
    }

    /// <summary>
    /// Explicit implementation for Create (not implemented yet).
    /// </summary>
    /// <param name="item">Order entity.</param>
    /// <returns>Order ID (not implemented).</returns>
    /// <exception cref="NotImplementedException">Always thrown for now.</exception>
    int IOrder.Create(Order item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Explicit implementation for ReadAll returning IEnumerable.
    /// </summary>
    /// <returns>Enumerable of order entities.</returns>
    IEnumerable<Order?> IOrder.ReadAll()
    {
        return ReadAll();
    }
}
