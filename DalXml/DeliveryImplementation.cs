using DalApi;
using DO;

namespace Dal;

/// <summary>
/// DAL implementation for managing Delivery entities using XML serialization.
/// Provides CRUD operations and supports filtering and full list management.
/// </summary>
internal class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Creates a new delivery in the XML data store.
    /// Throws <see cref="DalAlreadyExistsException"/> if a delivery with the same ID already exists.
    /// </summary>
    /// <param name="item">The delivery to add.</param>
    public void Create(Delivery item)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        if (list.Any(d => d.Id == item.Id))
            throw new DalAlreadyExistsException($"Delivery with ID={item.Id} already exists");

        list.Add(item);
        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    /// <summary>
    /// Reads a delivery by its unique ID.
    /// Returns null if the delivery does not exist.
    /// </summary>
    /// <param name="id">The unique delivery ID.</param>
    /// <returns>The delivery with the specified ID or null if not found.</returns>
    public Delivery? Read(int id)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return list.FirstOrDefault(d => d.Id == id);
    }

    /// <summary>
    /// Reads the first delivery matching the specified filter.
    /// Returns null if no delivery satisfies the filter.
    /// </summary>
    /// <param name="filter">A predicate function to filter deliveries.</param>
    /// <returns>The first matching delivery or null if not found.</returns>
    public Delivery? Read(Func<Delivery, bool> filter)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return list.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all deliveries from the XML data store.
    /// Optionally applies a filter to return a subset of deliveries.
    /// </summary>
    /// <param name="filter">Optional predicate to filter the deliveries.</param>
    /// <returns>An enumerable of deliveries.</returns>
    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return filter is null ? list : list.Where(filter);
    }

    /// <summary>
    /// Updates an existing delivery in the XML data store.
    /// Throws <see cref="DalDoesNotExistException"/> if the delivery does not exist.
    /// </summary>
    /// <param name="item">The delivery with updated information.</param>
    public void Update(Delivery item)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        int index = list.FindIndex(d => d.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Delivery with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    /// <summary>
    /// Deletes a delivery by its unique ID.
    /// Throws <see cref="DalDoesNotExistException"/> if the delivery does not exist.
    /// </summary>
    /// <param name="id">The unique delivery ID to delete.</param>
    public void Delete(int id)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        if (list.RemoveAll(d => d.Id == id) == 0)
            throw new DalDoesNotExistException($"Delivery with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    /// <summary>
    /// Deletes all deliveries from the XML data store.
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Delivery>(), Config.s_deliveries_xml);
    }


}
