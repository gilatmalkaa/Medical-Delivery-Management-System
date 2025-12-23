namespace BlApi;

public interface IOrder : IObservable
{
    public BO.Order Get(int id);
    public IEnumerable<BO.OrderInList> GetAll();
}