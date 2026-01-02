namespace BlApi;

/// <summary>
/// Business logic service for managing couriers.
/// Supports CRUD operations and observer mechanism.
/// </summary>
public interface ICourier : IObservable
{
    void Create(BO.Courier courier);

    BO.Courier Get(int id);

    IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null);

    void Update(BO.Courier courier);

    void Delete(int id);

    IEnumerable<BO.CourierInList> GetAll();
}
