namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implements the ICourier interface to manage courier entities
/// within the in-memory data source (DataSource.Couriers).
/// Provides CRUD operations for couriers in the DAL layer.
/// </summary>
public class CourierImplementation : ICourier
{
    /// <summary>
    /// Creates a new courier and adds it to the data source.
    /// If the courier ID is 0, a new running ID will be assigned automatically.
    /// If the ID already exists, an exception will be thrown.
    /// </summary>
    /// <param name="item">The courier entity to create.</param>
    /// <exception cref="Exception">Thrown if a courier with the same ID already exists.</exception>
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
                throw new Exception($"Courier with ID {item.Id} already exists.");
            DataSource.Couriers.Add(item);
        }
    }

    /// <summary>
    /// Reads and returns a courier entity by its unique ID.
    /// </summary>
    /// <param name="id">Courier ID to look for.</param>
    /// <returns>The courier with the specified ID, or null if not found.</returns>
    public Courier? Read(int id)
    {
        foreach (var courier in DataSource.Couriers)
        {
            if (courier.Id == id)
                return courier;
        }
        return null;
    }

    /// <summary>
    /// Returns a new list containing all couriers currently stored in the data source.
    /// </summary>
    /// <returns>List of all courier entities.</returns>
    public List<Courier> ReadAll()
    {
        return new List<Courier>(DataSource.Couriers);
    }

    /// <summary>
    /// Updates an existing courier’s data.
    /// Replaces the old courier entity with the new one.
    /// </summary>
    /// <param name="item">Courier entity with updated data.</param>
    /// <exception cref="Exception">Thrown if the courier does not exist.</exception>
    public void Update(Courier item)
    {
        Courier? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Courier with ID={item.Id} does not exist");



public class CourierImplementation : ICourier
{
    public int Create(Courier item) => throw new NotImplementedException();
    public Courier? Read(int id) => throw new NotImplementedException();
    public IEnumerable<Courier?> ReadAll() => throw new NotImplementedException();
    public void Update(Courier item) => throw new NotImplementedException();
    public void Delete(int id) => throw new NotImplementedException();

}
