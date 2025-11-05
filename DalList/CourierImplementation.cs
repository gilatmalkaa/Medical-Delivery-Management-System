namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class CourierImplementation : ICourier
{
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

    public Courier? Read(int id)
    {
        foreach (var courier in DataSource.Couriers)
        {
            if (courier.Id == id)
                return courier;
        }
        return null;
    }

    public List<Courier> ReadAll()
    {
        return new List<Courier>(DataSource.Couriers);
    }

    public void Update(Courier item)
    {
        Courier? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Courier with ID={item.Id} does not exist");

        DataSource.Couriers.Remove(existing);
        DataSource.Couriers.Add(item);
    }

    public void Delete(int id)
    {
        Courier? courier = Read(id);
        if (courier == null)
            throw new Exception($"Courier with ID={id} does not exist");

        DataSource.Couriers.Remove(courier);
    }

    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    int ICourier.Create(Courier item)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Courier?> ICourier.ReadAll()
    {
        return ReadAll();
    }
}
