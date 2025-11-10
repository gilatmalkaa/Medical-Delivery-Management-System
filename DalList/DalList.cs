namespace Dal;
using DalApi;



/// <summary>
/// Main DAL implementation that provides access to all entities
/// (Orders, Couriers, Deliveries, Config).
/// </summary>
sealed public class DalList : IDal
{
    public IOrder Order { get; } = new OrderImplementation();
    public ICourier Courier { get; } = new CourierImplementation();
    public IDelivery Delivery { get; } = new DeliveryImplementation();
    public IConfig Config { get; } = new ConfigImplementation();

    public void ResetDB()
    {
        Order.DeleteAll();
        Courier.DeleteAll();
        Delivery.DeleteAll();
        Config.Reset();
    }
}