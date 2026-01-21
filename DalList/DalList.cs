namespace Dal;
using DalApi;

/// <summary>
/// Main implementation of the <see cref="IDal"/> interface using in-memory lists.
/// Provides access to all DAL entities: Orders, Couriers, Deliveries, and Config.
/// This class serves as the central entry point for CRUD operations in the DAL layer.
/// </summary>
sealed internal class DalList : IDal
{

    private static readonly Lazy<IDal> _instance =
     new(() => new DalList(), true);

    // Singleton instance
    public static IDal Instance => _instance.Value;

    /// <summary>
    /// Gets the <see cref="IOrder"/> implementation for managing orders.
    /// Allows CRUD operations on Order entities.
    /// </summary>
    public IOrder Order { get; } = new OrderImplementation();

    /// <summary>
    /// Gets the <see cref="ICourier"/> implementation for managing couriers.
    /// Allows CRUD operations on Courier entities.
    /// </summary>
    public ICourier Courier { get; } = new CourierImplementation();

    /// <summary>
    /// Gets the <see cref="IDelivery"/> implementation for managing deliveries.
    /// Allows CRUD operations on Delivery entities.
    /// </summary>
    public IDelivery Delivery { get; } = new DeliveryImplementation();

    /// <summary>
    /// Gets the <see cref="IConfig"/> implementation for managing configuration data.
    /// Allows resetting and updating global configuration settings.
    /// </summary>
    public IConfig Config { get; } = new ConfigImplementation();

    // Private constructor
    private DalList() { }


    /// <summary>
    /// Resets the entire in-memory database to its initial state.
    /// Deletes all orders, couriers, deliveries, and resets the configuration.
    /// Useful for testing or re-initializing demo data.
    /// </summary>
    public void ResetDB()
    {
        Order.DeleteAll();
        Courier.DeleteAll();
        Delivery.DeleteAll();
        Config.Reset();
    }
}
