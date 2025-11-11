namespace Dal;
using DalApi;
using DO;
using System.Linq;


/// <summary>
/// Implements ICourier — manages Courier data in memory.
/// </summary>
internal class CourierImplementation : ICourier
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
                throw new DalAlreadyExistsException($"Courier with ID={item.Id} already exists.");
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
    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null) //stage 2
    => filter == null
            ? DataSource.Couriers.Select(item => item)
            : DataSource.Couriers.Where(filter);

    /// <summary>
    /// Updates an existing courier. Throws exception if not found.
    /// </summary>
    public void Update(Courier item)
    {
        Courier? existing = Read(item.Id);
        if (existing == null)
            throw new DalDoesNotExistException($"Courier with ID={item.Id} does not exist.");

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
            throw new DalDoesNotExistException($"Courier with ID={id} does not exist.");

        DataSource.Couriers.Remove(courier);
    }

    /// <summary>
    /// Deletes all couriers.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    Courier? ICrud<Courier>.Read(Func<Courier, bool> filter)
    {
        return DataSource.Couriers.FirstOrDefault(filter);
    }
}

  