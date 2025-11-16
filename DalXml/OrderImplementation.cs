using DalApi;
using DO;

namespace Dal;

internal class OrderImplementation : IOrder
{
    public void Create(Order item)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        if (list.Any(o => o.Id == item.Id))
            throw new DalAlreadyExistsException($"Order with ID={item.Id} already exists");

        list.Add(item);
        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    public Order? Read(int id)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return list.FirstOrDefault(o => o.Id == id);
    }

    public Order? Read(Func<Order, bool> filter)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return list.FirstOrDefault(filter);
    }

    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return filter == null ? list : list.Where(filter);
    }

    public void Update(Order item)
    {   
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        int index = list.FindIndex(o => o.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Order with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    public void Delete(int id)
    {
        List<Order> list = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);

        if (list.RemoveAll(o => o.Id == id) == 0)
            throw new DalDoesNotExistException($"Order with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_orders_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Order>(), Config.s_orders_xml);
    }
}

