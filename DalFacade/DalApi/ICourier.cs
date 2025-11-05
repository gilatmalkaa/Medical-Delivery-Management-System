namespace DalApi;
using DO;

/// <summary>
/// Data Access Layer (DAL) interface for managing Courier entities.
/// Defines CRUD operations for creating, reading, updating, and deleting couriers.
/// </summary>
public interface ICourier
{
    /// <summary>
    /// Adds a new courier to the data source.
    /// Returns the unique ID assigned to the new courier.
    /// </summary>
    int Create(Courier item);

    /// <summary>
    /// Retrieves a specific courier by its unique ID.
    /// Returns null if the courier is not found.
    /// </summary>
    Courier? Read(int id);

    /// <summary>
    /// Retrieves all couriers from the data source.
    /// Returns an enumerable collection of courier objects.
    /// </summary>
    IEnumerable<Courier?> ReadAll();

    /// <summary>
    /// Updates the details of an existing courier.
    /// </summary>
    void Update(Courier item);

    /// <summary>
    /// Deletes a courier with the specified ID from the data source.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Deletes all couriers from the data source.
    /// Typically used during system reset or initialization.
    /// </summary>
    void DeleteAll();
}
