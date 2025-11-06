namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Implements the IOrder interface to manage order entities.
/// Provides CRUD operations on in-memory list DataSource.Orders.
/// </summary>
public class OrderImplementation : IOrder
{
    /// <summary>
    /// Creates a new order in the data source.
    /// If the ID is 0, assigns a running ID automatically.
    /// Throws an exception if an order with the same ID already exists.
    /// </summary>
    /// <param name="item">Order to create.</param>
    /// <exception cref="Exception">Thrown if an order with the same ID already exists.</exception>
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


public class OrderImplementation : IOrder
{

}
