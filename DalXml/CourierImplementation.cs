using DalApi;
using DO;

namespace Dal;

internal class CourierImplementation : ICourier
{

    public void Create(Courier item)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_deliveries_xml);

        if (list.Any(c => c.Id == item.Id))
            throw new DalAlreadyExistsException($"Courier with ID={item.Id} already exists");

        list.Add(item);
        XMLTools.SaveListToXMLSerializer(list, Config.s_deliveries_xml);
    }

    public Courier? Read(int id)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_deliveries_xml);
        return list.FirstOrDefault(c => c.Id == id);
    }

    public Courier? Read(Func<Courier, bool> filter)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);
        return list.FirstOrDefault(filter);
    }

    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);
        return filter == null ? list : list.Where(filter);
    }

    public void Update(Courier item)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);

        int index = list.FindIndex(c => c.Id == item.Id);
        if (index == -1)
            throw new DalDoesNotExistException($"Courier with ID={item.Id} does not exist");

        list[index] = item;
        XMLTools.SaveListToXMLSerializer(list, Config.s_couriers_xml);
    }

    public void Delete(int id)
    {
        List<Courier> list = XMLTools.LoadListFromXMLSerializer<Courier>(Config.s_couriers_xml);

        if (list.RemoveAll(c => c.Id == id) == 0)
            throw new DalDoesNotExistException($"Courier with ID={id} does not exist");

        XMLTools.SaveListToXMLSerializer(list, Config.s_couriers_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Courier>(), Config.s_couriers_xml);
    }
}
