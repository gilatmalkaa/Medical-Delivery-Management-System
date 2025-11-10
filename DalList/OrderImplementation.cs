namespace Dal;
using DalApi;
using DO;
using System.Linq;



internal class OrderImplementation : IOrder
{
    public void Create(Order item)
    {
        if (item.Id == 0)
        {
            int newId = DataSource.Config.NextOrderId;
            Order newOrder = item with { Id = newId };
            DataSource.Orders.Add(newOrder);
        }
        else
        {
            if (Read(item.Id) != null)
                throw new Exception($"Order with ID {item.Id} already exists.");
            DataSource.Orders.Add(item);
        }
    }

    public Order? Read(int id)
    {
        return DataSource.Orders.FirstOrDefault(o => o.Id == id);
    }

    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null)
    => filter == null
            ? DataSource.Orders.Select(item => item)
            : DataSource.Orders.Where(filter);

    public void Update(Order item)
    {
        Order? existing = Read(item.Id);
        if (existing == null)
            throw new Exception($"Order with ID={item.Id} does not exist");

        DataSource.Orders.Remove(existing);
        DataSource.Orders.Add(item);
    }

    public void Delete(int id)
    {
        Order? order = Read(id);
        if (order == null)
            throw new Exception($"Order with ID={id} does not exist");

        DataSource.Orders.Remove(order);
    }

    public void DeleteAll()
    {
        DataSource.Orders.Clear();
    }

    Order? ICrud<Order>.Read(Func<Order, bool> filter)
    {
        return DataSource.Orders.FirstOrDefault(filter);
    }
}

