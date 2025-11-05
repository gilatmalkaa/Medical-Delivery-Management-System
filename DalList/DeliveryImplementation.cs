namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implements the IDelivery interface to manage delivery entities.
/// Provides CRUD operations using the in-memory data source (DataSource.Deliveries).
/// </summary>
public class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Creates a new delivery entity.
    /// If the ID is 0, assigns an auto-generated running ID.
    /// Throws an exception if the delivery already exists.
    /// </summary>
    /// <param name="item">The delivery entity to create.</param>
    /// <exception cref="Exception">Thrown if a delivery with the same ID already exists.</exception>
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
                throw new Exception($"Delivery with ID={item.Id} already exists");

            DataSource.Deliveries.Add(item);
        }
    }

    /// <summary>
    /// Reads a delivery entity by its ID.
    /// </summary>
    /// <param name="id">Delivery ID.</param>
    /// <returns>The delivery if found, otherwise null.</returns>
    public Delivery? Read(int id)
    {
        foreach (var delivery in DataSource.Deliveries)
        {
            if (delivery.Id == id)
                return delivery;
        }
        return null;
    }

    /// <summary>
    /// Returns all deliveries currently stored in the data source.
    /// </summary>
    /// <returns>List of all delivery entities.</returns>
    public List<Delivery> ReadAll()
    {
        return new List<Delivery>(DataSource.Deliveries);
    }

    /// <summary>
    /// Updates an existing delivery entity.
    /// </summary>
    /// <param name="item">Updated delivery entity.</param>
    /// <exception cref="Exception">Thrown if the delivery does not exist.</exception>
    public void Update(Delivery item)
    {
        Delivery? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Delivery with ID={item.Id} does not exist");

        DataSource.Deliveries.Remove(existing);
        DataSource.Deliveries.Add(item);
    }

    /// <summary>
    /// Deletes a delivery entity by ID.
    /// </summary>
    /// <param name="id">Delivery ID to delete.</param>
    /// <exception cref="Exception">Thrown if the delivery does not exist.</exception>
    public void Delete(int id)
    {
        Delivery? delivery = Read(id);
        if (delivery == null)
            throw new Exception($"Delivery with ID={id} does not exist");

        DataSource.Deliveries.Remove(delivery);
    }

    /// <summary>
    /// Clears all deliveries from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Deliveries.Clear();
    }

    /// <summary>
    /// Explicit interface implementation for Create (not implemented yet).
    /// </summary>
    /// <param name="item">Delivery entity.</param>
    /// <returns>New delivery ID (not implemented).</returns>
    /// <exception cref="NotImplementedException">Always thrown for now.</exception>
    int IDelivery.Create(Delivery item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Explicit interface implementation for ReadAll returning IEnumerable.
    /// </summary>
    /// <returns>Enumerable of delivery entities.</returns>
    IEnumerable<Delivery?> IDelivery.ReadAll()
    {
        return ReadAll();
    }
}
