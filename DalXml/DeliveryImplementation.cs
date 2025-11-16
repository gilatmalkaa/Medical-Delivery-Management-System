using DalApi;
using DO;

namespace Dal;

internal class DeliveryImplementation : IDelivery
{
    public void Create(Delivery item)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        if (list.Any(d => d.Id == item.Id))
            throw new DalAlreadyExistsException($"Delivery with ID={item.Id} already exists");

        list.Add(item);
        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    public Delivery? Read(int id)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return list.FirstOrDefault(d => d.Id == id);
    }

    public Delivery? Read(Func<Delivery, bool> filter)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return list.FirstOrDefault(filter);
    }

    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return filter == null ? list : list.Where(filter);
    }

    public void Update(Delivery item)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        int index = list.FindIndex(d => d.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Delivery with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    public void Delete(int id)
    {
        List<Delivery> list = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);

        if (list.RemoveAll(d => d.Id == id) == 0)
            throw new DalDoesNotExistException($"Delivery with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Delivery>(), Config.s_deliveries_xml);
    }
}
