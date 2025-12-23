namespace BlApi;

public interface ICourier : IObservable
{
    public void Create(BO.Courier courier);
    public BO.Courier Get(int id);
    public IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null);
    public void Update(BO.Courier courier);
    public void Delete(int id);
    public IEnumerable<BO.CourierInList> GetAll();
}