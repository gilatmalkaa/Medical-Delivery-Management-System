namespace DalApi;
using DO;

/// <summary>
/// Data Access Layer (DAL) interface for managing Order entities.
/// Defines standard CRUD operations for creating, reading, updating, and deleting orders.
/// </summary>
public interface IOrder
{
    /// <summary>
    /// Adds a new order to the data source.
    /// Returns the unique ID assigned to the order.
    /// </summary>
    int Create(Order item);

    /// <summary>
    /// Retrieves a specific order by its unique ID.
    /// Returns null if the order does not exist.
    /// </summary>
    Order? Read(int id);

    /// <summary>
    /// Retrieves all orders currently stored in the data source.
    /// Returns an enumerable collection of orders.
    /// </summary>
    IEnumerable<Order?> ReadAll();

    /// <summary>
    /// Updates the details of an existing order.
    /// </summary>
    void Update(Order item);

    /// <summary>
    /// Deletes the order with the specified ID from the data source.
    /// </summary>
    void Delete(int id);
}