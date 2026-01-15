namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
/// Provides an in-memory implementation for managing courier data,
/// including creation, retrieval, update, and deletion operations.
/// </summary>
internal class CourierImplementation : ICourier
{
    /// <summary>
    /// Creates a new courier entity.
    /// Generates a new identifier if the provided ID is zero,
    /// and throws an exception if a courier with the same ID already exists.
    /// </summary>
    public void Create(Courier item)
    {
        if (item.Id == 0)
        {
            int newId = DataSource.Config.NextCourierId;
            Courier newCourier = item with { Id = newId };
            DataSource.Couriers.Add(newCourier);
        }
        else
        {
            if (Read(item.Id) != null)
                throw new DalAlreadyExistsException($"Courier with ID={item.Id} already exists.");
            DataSource.Couriers.Add(item);
        }
    }

    /// <summary>
    /// Retrieves a courier by its unique identifier.
    /// Returns null if no matching courier is found.
    /// </summary>
    public Courier? Read(int id)
    {
        return DataSource.Couriers.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Retrieves all couriers, optionally filtered by a predicate.
    /// </summary>
    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
        => filter == null
            ? DataSource.Couriers.Select(item => item)
            : DataSource.Couriers.Where(filter);

    /// <summary>
    /// Updates an existing courier.
    /// Throws an exception if the courier does not exist.
    /// </summary>
    public void Update(Courier item)
    {
        Courier? existing = Read(item.Id);
        if (existing == null)
            throw new DalDoesNotExistException($"Courier with ID={item.Id} does not exist.");

        DataSource.Couriers.Remove(existing);
        DataSource.Couriers.Add(item);
    }

    /// <summary>
    /// Deletes a courier by its unique identifier.
    /// Throws an exception if the courier does not exist.
    /// </summary>
    public void Delete(int id)
    {
        Courier? courier = Read(id);
        if (courier == null)
            throw new DalDoesNotExistException($"Courier with ID={id} does not exist.");

        DataSource.Couriers.Remove(courier);
    }

    /// <summary>
    /// Deletes all courier entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    /// <summary>
    /// Retrieves a single courier that matches the specified predicate.
    /// </summary>
    Courier? ICrud<Courier>.Read(Func<Courier, bool> filter)
    {
        return DataSource.Couriers.FirstOrDefault(filter);
    }
}
