using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// DAL implementation for managing Order entities using XML serialization.
/// Provides CRUD operations and supports filtering and full list management.
/// </summary>
internal class OrderImplementation : IOrder
{
    /// <summary>
    /// Creates a new order in the XML data store.
    /// Throws <see cref="DalAlreadyExistsException"/> if an order with the same ID already exists.
    /// </summary>
    /// <param name="item">The order to add.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Order item)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        if (list.Any(o => o.Id == item.Id))
            throw new DalAlreadyExistsException($"Order with ID={item.Id} already exists");

        list.Add(item);
        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    /// <summary>
    /// Reads an order by its unique ID.
    /// Returns null if the order does not exist.
    /// </summary>
    /// <param name="id">The unique order ID.</param>
    /// <returns>The order with the specified ID or null if not found.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order? Read(int id)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return list.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Reads the first order matching the specified filter.
    /// Returns null if no order satisfies the filter.
    /// </summary>
    /// <param name="filter">A predicate function to filter orders.</param>
    /// <returns>The first matching order or null if not found.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order? Read(Func<Order, bool> filter)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return list.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all orders from the XML data store.
    /// Optionally applies a filter to return a subset of orders.
    /// </summary>
    /// <param name="filter">Optional predicate to filter the orders.</param>
    /// <returns>An enumerable of orders.</returns>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return filter == null ? list : list.Where(filter);
    }

    /// <summary>
    /// Updates an existing order in the XML data store.
    /// Throws <see cref="DalDoesNotExistException"/> if the order does not exist.
    /// </summary>
    /// <param name="item">The order with updated information.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order item)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        int index = list.FindIndex(o => o.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Order with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    /// <summary>
    /// Deletes an order by its unique ID.
    /// Throws <see cref="DalDoesNotExistException"/> if the order does not exist.
    /// </summary>
    /// <param name="id">The unique order ID to delete.</param>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        if (list.RemoveAll(o => o.Id == id) == 0)
            throw new DalDoesNotExistException($"Order with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    /// <summary>
    /// Deletes all orders from the XML data store.
    /// </summary>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Order>(), Config.s_orders_xml);
    }
}
