namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// In-memory implementation of the <see cref="IOrder"/> interface.
/// Provides CRUD operations for <see cref="Order"/> entities.
/// Uses <see cref="DataSource"/> as the underlying data store.
/// </summary>
internal class OrderImplementation : IOrder
{
    /// <summary>
    /// Creates a new <see cref="Order"/> in the data source.
    /// If the order has Id = 0, a new unique ID is assigned automatically.
    /// Throws <see cref="DalAlreadyExistsException"/> if an order with the same ID already exists.
    /// </summary>
    /// <param name="item">The order to create.</param>
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
                throw new DalAlreadyExistsException($"Order with ID={item.Id} already exists.");
            DataSource.Orders.Add(item);
        }
    }

    /// <summary>
    /// Reads an <see cref="Order"/> by its unique ID.
    /// Returns null if no order with the given ID exists.
    /// </summary>
    /// <param name="id">The ID of the order to read.</param>
    /// <returns>The order with the specified ID, or null if not found.</returns>
    public Order? Read(int id)
    {
        return DataSource.Orders.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Reads all orders, optionally filtered by a predicate.
    /// If no filter is provided, returns all orders.
    /// </summary>
    /// <param name="filter">Optional filter function.</param>
    /// <returns>An enumerable of orders matching the filter or all orders.</returns>
    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null)
        => filter == null
            ? DataSource.Orders.Select(item => item)
            : DataSource.Orders.Where(filter);

    /// <summary>
    /// Updates an existing <see cref="Order"/> in the data source.
    /// Throws <see cref="DalDoesNotExistException"/> if the order does not exist.
    /// </summary>
    /// <param name="item">The order with updated information.</param>
    public void Update(Order item)
    {
        Order? existing = Read(item.Id);
        if (existing == null)
            throw new DalDoesNotExistException($"Order with ID={item.Id} does not exist.");

        DataSource.Orders.Remove(existing);
        DataSource.Orders.Add(item);
    }

    /// <summary>
    /// Deletes an <see cref="Order"/> by its ID.
    /// Throws <see cref="DalDoesNotExistException"/> if the order does not exist.
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
    public void Delete(int id)
    {
        Order? order = Read(id);
        if (order == null)
            throw new DalDoesNotExistException($"Order with ID={id} does not exist.");

        DataSource.Orders.Remove(order);
    }

    /// <summary>
    /// Deletes all orders from the data source.
    /// Useful for resetting the order data during testing or initialization.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Orders.Clear();
    }

    /// <summary>
    /// Reads the first order matching the specified filter predicate.
    /// Part of the <see cref="ICrud{T}"/> interface implementation.
    /// Returns null if no order matches the filter.
    /// </summary>
    /// <param name="filter">Predicate to filter orders.</param>
    /// <returns>The first matching order or null if none match.</returns>
    Order? ICrud<Order>.Read(Func<Order, bool> filter)
    {
        return DataSource.Orders.FirstOrDefault(filter);
    }
}
