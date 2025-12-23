namespace BlApi;

public interface IDelivery : IObservable
{
    public void Create(int orderId, int courierId);
    public void UpdateStatus(int deliveryId, BO.DeliveryStatus status);
    public BO.DeliveryPerOrderInList Get(int id);
}