using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// DAL implementation for managing Courier entities using XML serialization.
/// Provides CRUD operations and supports filtering and full list management.
/// </summary>
internal class CourierImplementation : ICourier
{
    /// <summary>
    /// Creates a new courier in the XML data store.
    /// Throws <see cref="DalAlreadyExistsException"/> if a courier with the same ID already exists.
    /// </summary>
    /// <param name="item">The courier to add.</param>
    /// 

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Courier item)
{
    List<Courier> list =
        XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);

    if (list.Any(c => c.Id == item.Id))
        throw new DalAlreadyExistsException(
            $"Courier with ID={item.Id} already exists");

    list.Add(item);

    XMLTools.SaveListToXMLSerializer(list, Config.s_couriers_xml);
}

    /// <summary>
    /// Reads a courier by its unique ID.
    /// Returns null if the courier does not exist.
    /// </summary>
    /// <param name="id">The unique courier ID.</param>
    /// <returns>The courier with the specified ID or null if not found.</returns>
    /// 

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Courier? Read(int id)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);
        return list.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Reads the first courier matching the specified filter.
    /// Returns null if no courier satisfies the filter.
    /// </summary>
    /// <param name="filter">A predicate function to filter couriers.</param>
    /// <returns>The first matching courier or null if not found.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Courier? Read(Func<Courier, bool> filter)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);
        return list.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all couriers from the XML data store.
    /// Optionally applies a filter to return a subset of couriers.
    /// </summary>
    /// <param name="filter">Optional predicate to filter the couriers.</param>
    /// <returns>An enumerable of couriers.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]

    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);
        return filter == null ? list : list.Where(filter);
    }

    /// <summary>
    /// Updates an existing courier in the XML data store.
    /// Throws <see cref="DalDoesNotExistException"/> if the courier does not exist.
    /// </summary>
    /// <param name="item">The courier with updated information.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void Update(Courier item)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);

        int index = list.FindIndex(c => c.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Courier with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_couriers_xml);
    }

    /// <summary>
    /// Deletes a courier by its unique ID.
    /// Throws <see cref="DalDoesNotExistException"/> if the courier does not exist.
    /// </summary>
    /// <param name="id">The unique courier ID to delete.</param>
    /// 

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);

        if (list.RemoveAll(c => c.Id == id) == 0)
            throw new DalDoesNotExistException($"Courier with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_couriers_xml);
    }

    /// <summary>
    /// Deletes all couriers from the XML data store.
    /// </summary>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Courier>(), Config.s_couriers_xml);
    }
}
