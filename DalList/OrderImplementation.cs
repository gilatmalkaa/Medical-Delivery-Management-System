namespace Dal;
using DalApi;
using DO;

public class OrderImplementation : IOrder
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
        Order? order = DataSource.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new Exception($"Order with ID={id} does not exist");
        return order;
    }

    public List<Order> ReadAll()
    {
        return new List<Order>(DataSource.Orders);
    }

    public void Update(Order item)
    {
        Order? existing = DataSource.Orders.FirstOrDefault(o => o.Id == item.Id);
        if (existing == null)
            throw new Exception($"Order with ID={item.Id} does not exist");

        DataSource.Orders.Remove(existing);
        DataSource.Orders.Add(item);
    }

    public void Delete(int id)
    {
        Order? order = DataSource.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            throw new Exception($"Order with ID={id} does not exist");
        DataSource.Orders.Remove(order);
    }

    public void DeleteAll()
    {
        DataSource.Orders.Clear();
    }

    int IOrder.Create(Order item)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Order?> IOrder.ReadAll()
    {
        return ReadAll();
    }
}
