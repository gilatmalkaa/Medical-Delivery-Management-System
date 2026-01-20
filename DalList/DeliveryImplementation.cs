namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// In-memory implementation of the <see cref="IDelivery"/> interface.
/// Provides CRUD operations for <see cref="Delivery"/> entities.
/// Uses <see cref="DataSource"/> as the underlying data store.
/// </summary>
internal class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Creates a new <see cref="Delivery"/> in the data source.
    /// If the delivery has Id = 0, a new unique ID is assigned automatically.
    /// Throws <see cref="DalAlreadyExistsException"/> if a delivery with the same ID already exists.
    /// </summary>
    /// <param name="item">The delivery to create.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Delivery item)
    {
        if (item.Id == 0)
        {
            int newId = DataSource.Config.NextDeliveryId;
            Delivery newDelivery = item with { Id = newId };
            DataSource.Deliveries.Add(newDelivery);
        }
        else
        {
            if (Read(item.Id) != null)
                throw new DalAlreadyExistsException($"Delivery with ID={item.Id} already exists.");
            DataSource.Deliveries.Add(item);
        }
    }

    /// <summary>
    /// Reads a <see cref="Delivery"/> by its unique ID.
    /// Returns null if no delivery with the given ID exists.
    /// </summary>
    /// <param name="id">The ID of the delivery to read.</param>
    /// <returns>The delivery with the specified ID, or null if not found.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Delivery? Read(int id)
    {
        return DataSource.Deliveries.FirstOrDefault(d => d.Id == id);
    }

    /// <summary>
    /// Reads all deliveries, optionally filtered by a predicate.
    /// If no filter is provided, returns all deliveries.
    /// </summary>
    /// <param name="filter">Optional filter function.</param>
    /// <returns>An enumerable of deliveries matching the filter or all deliveries.</returns>
    /// 

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null)
        => filter == null
            ? DataSource.Deliveries.Select(item => item)
            : DataSource.Deliveries.Where(filter);

    /// <summary>
    /// Updates an existing <see cref="Delivery"/> in the data source.
    /// Throws <see cref="DalDoesNotExistException"/> if the delivery does not exist.
    /// </summary>
    /// <param name="item">The delivery with updated information.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Delivery item)
    {
        Delivery? existing = Read(item.Id);
        if (existing == null)
            throw new DalDoesNotExistException($"Delivery with ID={item.Id} does not exist.");

        DataSource.Deliveries.Remove(existing);
        DataSource.Deliveries.Add(item);
    }

    /// <summary>
    /// Deletes a <see cref="Delivery"/> by its ID.
    /// Throws <see cref="DalDoesNotExistException"/> if the delivery does not exist.
    /// </summary>
    /// <param name="id">The ID of the delivery to delete.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        Delivery? delivery = Read(id);
        if (delivery == null)
            throw new DalDoesNotExistException($"Delivery with ID={id} does not exist.");

        DataSource.Deliveries.Remove(delivery);
    }

    /// <summary>
    /// Deletes all deliveries from the data source.
    /// Useful for resetting the delivery data during testing or initialization.
    /// </summary>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        DataSource.Deliveries.Clear();
    }

    /// <summary>
    /// Reads the first delivery matching the specified filter predicate.
    /// Part of the <see cref="ICrud{T}"/> interface implementation.
    /// Returns null if no delivery matches the filter.
    /// </summary>
    /// <param name="filter">Predicate to filter deliveries.</param>
    /// <returns>The first matching delivery or null if none match.</returns>
    /// 

    [MethodImpl(MethodImplOptions.Synchronized)]
    Delivery? ICrud<Delivery>.Read(Func<Delivery, bool> filter)
    {
        return DataSource.Deliveries.FirstOrDefault(filter);
    }
}
