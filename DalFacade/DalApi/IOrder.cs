namespace DalApi;
using DO;

public interface IOrder
{
    int Create(Order item);
    Order? Read(int id);
    IEnumerable<Order?> ReadAll();
    void Update(Order item);
    void Delete(int id);
    void DeleteAll();

}