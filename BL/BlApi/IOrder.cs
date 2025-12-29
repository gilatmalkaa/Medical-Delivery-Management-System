namespace BlApi;

public interface IOrder : IObservable
{
    BO.Order Create(BO.Order order);  
    BO.Order Update(BO.Order order);  
    public BO.Order Get(int id);
    public IEnumerable<BO.OrderInList> GetAll();
}