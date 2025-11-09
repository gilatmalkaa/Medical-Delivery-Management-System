namespace Dal;
using DalApi;



/// <summary>
/// Main DAL implementation that provides access to all entities
/// (Orders, Couriers, Deliveries, Config).
/// </summary>
sealed public class DalList : IDal
{
    public IOrder Order => new OrderImplementation();
    public ICourier Courier => new CourierImplementation();
    public IDelivery Delivery => new DeliveryImplementation();
    public IConfig Config => new ConfigImplementation();

    public void ResetDB()
    {
        DataSource.Orders.Clear();
        DataSource.Couriers.Clear();
        DataSource.Deliveries.Clear();
        DataSource.Config.Reset();
    }
}