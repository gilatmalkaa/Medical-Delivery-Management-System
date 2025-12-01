using DalApi;

namespace Dal;

/// <summary>
/// DAL implementation that uses XML files as the data store.
/// Provides access to all entity implementations (Courier, Order, Delivery, Config)
/// and supports full database reset.
/// </summary>
public sealed class DalXml : IDal
{
    /// <summary>
    /// Gets the courier data access implementation.
    /// </summary>
    public ICourier Courier { get; } = new CourierImplementation();

    /// <summary>
    /// Gets the order data access implementation.
    /// </summary>
    public IOrder Order { get; } = new OrderImplementation();

    /// <summary>
    /// Gets the delivery data access implementation.
    /// </summary>
    public IDelivery Delivery { get; } = new DeliveryImplementation();

    /// <summary>
    /// Gets the configuration data access implementation.
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    /// <summary>
    /// Performs a full reset of the XML "database".
    /// Deletes all records from the Courier, Order, and Delivery data files
    /// and resets configuration values to their defaults.
    /// </summary>
    public void ResetDB()
    {
        Courier.DeleteAll();
        Order.DeleteAll();
        Delivery.DeleteAll();
        Config.Reset();
    }
}
