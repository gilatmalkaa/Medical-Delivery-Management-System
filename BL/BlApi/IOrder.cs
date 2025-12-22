namespace BlApi;

public interface IOrder
{
    public BO.Order Get(int id);
    public IEnumerable<BO.OrderInList> GetAll();
    // ניתן להוסיף מתודות לוגיות כמו CreateOrder או CancelOrder
}