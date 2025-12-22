namespace BlApi;

public interface ICourier
{
    public void Create(BO.Courier courier);
    public BO.Courier Get(int id);
    public IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null);
    public void Update(BO.Courier courier);
    public void Delete(int id);

    // מתודה להחזרת רשימה מצומצמת לתצוגה
    public IEnumerable<BO.CourierInList> GetAll();
}