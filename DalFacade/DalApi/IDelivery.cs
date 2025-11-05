namespace DalApi;
using DO;

/// <summary>
/// Data Access Layer (DAL) interface for managing Delivery entities.
/// Defines CRUD operations for creating, reading, updating, and deleting deliveries.
/// </summary>
public interface IDelivery
{
    /// <summary>
    /// Adds a new delivery record to the data source.
    /// Returns the unique ID assigned to the new delivery.
    /// </summary>
    int Create(Delivery item);

    /// <summary>
    /// Retrieves a specific delivery by its unique ID.
    /// Returns null if the delivery is not found.
    /// </summary>
    Delivery? Read(int id);

    /// <summary>
    /// Retrieves all deliveries from the data source.
    /// Returns an enumerable collection of delivery objects.
    /// </summary>
    IEnumerable<Delivery?> ReadAll();

    /// <summary>
    /// Updates an existing delivery record with new data.
    /// </summary>
    void Update(Delivery item);

    /// <summary>
    /// Deletes a delivery with the specified ID from the data source.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Deletes all deliveries from the data source.
    /// Typically used during system reset or testing.
    /// </summary>
    void DeleteAll();
}
