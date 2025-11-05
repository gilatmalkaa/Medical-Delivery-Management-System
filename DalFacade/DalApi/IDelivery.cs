namespace DalApi;
using DO;

public interface IDelivery
{
    int Create(Delivery item);
    Delivery? Read(int id);
    IEnumerable<Delivery?> ReadAll();
    void Update(Delivery item);
    void Delete(int id);
    void DeleteAll();

}