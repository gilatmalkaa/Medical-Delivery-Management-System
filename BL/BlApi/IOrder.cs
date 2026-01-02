namespace BlApi;

/// <summary>
/// Business logic service for managing orders.
/// </summary>
public interface IOrder : IObservable
{
    BO.Order Create(BO.Order order);

    BO.Order Update(BO.Order order);

    BO.Order Get(int id);

    IEnumerable<BO.OrderInList> GetAll();
}
