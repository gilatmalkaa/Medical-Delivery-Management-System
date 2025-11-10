namespace Dal;
using DalApi;
using DO;
using System.Linq;



internal class DeliveryImplementation : IDelivery
{
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
                throw new Exception($"Delivery with ID {item.Id} already exists.");
            DataSource.Deliveries.Add(item);
        }
    }

    public Delivery? Read(int id)
    {
        return DataSource.Deliveries.FirstOrDefault(d => d.Id == id);
    }

    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null)
    => filter == null
            ? DataSource.Deliveries.Select(item => item)
            : DataSource.Deliveries.Where(filter);


    public void Update(Delivery item)
    {
        Delivery? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Delivery with ID={item.Id} does not exist");

        DataSource.Deliveries.Remove(existing);
        DataSource.Deliveries.Add(item);
    }

    public void Delete(int id)
    {
        Delivery? delivery = Read(id);
        if (delivery == null)
            throw new Exception($"Delivery with ID={id} does not exist");

        DataSource.Deliveries.Remove(delivery);
    }

    public void DeleteAll()
    {
        DataSource.Deliveries.Clear();
    }

    Delivery? ICrud<Delivery>.Read(Func<Delivery, bool> filter)
    {
        return DataSource.Deliveries.FirstOrDefault(filter);
    }
}
