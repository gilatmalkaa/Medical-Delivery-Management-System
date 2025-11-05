namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DeliveryImplementation : IDelivery
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
                throw new Exception($"Delivery with ID={item.Id} already exists");

            DataSource.Deliveries.Add(item);
        }
    }

    public Delivery? Read(int id)
    {
        foreach (var delivery in DataSource.Deliveries)
        {
            if (delivery.Id == id)
                return delivery;
        }
        return null;
    }

    public List<Delivery> ReadAll()
    {
        return new List<Delivery>(DataSource.Deliveries);
    }

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

    int IDelivery.Create(Delivery item)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Delivery?> IDelivery.ReadAll()
    {
        return ReadAll();
    }
}
