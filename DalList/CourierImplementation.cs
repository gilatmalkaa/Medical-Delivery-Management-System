namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Implements ICourier — manages Courier data in memory.
/// </summary>
public class CourierImplementation : ICourier
{
    /// <summary>
    /// Adds a new courier. Generates a new ID if Id == 0.
    /// Throws exception if courier with same ID exists.
    /// </summary>
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

    /// <summary>
    /// Returns a courier by ID, or null if not found.
    /// </summary>
    public Courier? Read(int id)
    {
        return DataSource.Couriers.FirstOrDefault(c => c.Id == id);
    }

    /// <summary>
    /// Returns a list of all couriers.
    /// </summary>
    public List<Courier> ReadAll()
    {
        return new List<Courier>(DataSource.Couriers);
    }

    /// <summary>
    /// Updates an existing courier. Throws exception if not found.
    /// </summary>
    public void Update(Courier item)
    {
        Courier? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Courier with ID={item.Id} does not exist");

        DataSource.Couriers.Remove(existing);
        DataSource.Couriers.Add(item);
    }

    /// <summary>
    /// Deletes a courier by ID. Throws exception if not found.
    /// </summary>
    public void Delete(int id)
    {
        Courier? courier = Read(id);
        if (courier == null)
            throw new Exception($"Courier with ID={id} does not exist");

        DataSource.Couriers.Remove(courier);
    }

    /// <summary>
    /// Deletes all couriers.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    /// <summary>
    /// Explicit interface Create method — wraps the public Create().
    /// </summary>
    int ICourier.Create(Courier item)
    {
        Create(item);
        return item.Id;
    }

    /// <summary>
    /// Explicit interface ReadAll method — returns all couriers.
    /// </summary>
    IEnumerable<Courier?> ICourier.ReadAll()
    {
        return ReadAll();
    }
}
