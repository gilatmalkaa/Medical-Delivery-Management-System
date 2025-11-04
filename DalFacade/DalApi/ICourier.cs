namespace DalApi;
using DO;

public interface ICourier
{
    int Create(Courier item);
    Courier? Read(int id);
    IEnumerable<Courier?> ReadAll();
    void Update(Courier item);
    void Delete(int id);
}