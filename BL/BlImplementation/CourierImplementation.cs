using BlApi;
using BO;
using Helpers;

internal class CourierImplementation : ICourier
{
    public void Create(Courier item) => CourierManager.Create(item);
    public void Delete(int id) => CourierManager.Delete(id);
    public Courier Get(int id) => CourierManager.Get(id);
    public IEnumerable<CourierInList> GetAll() => CourierManager.GetAll();
    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null) => CourierManager.ReadAll(filter);
    public void Update(Courier item) => CourierManager.Update(item);
}
