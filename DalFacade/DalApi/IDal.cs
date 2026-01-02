namespace DalApi;

/// <summary>
/// Root DAL interface providing access to all DAL modules.
/// Represents the entry point for the data layer.
/// </summary>
public interface IDal
{
    ICourier Courier { get; }
    IOrder Order { get; }
    IDelivery Delivery { get; }

    IConfig Config { get; }

    /// <summary>
    /// Deletes all persistent data and resets the database.
    /// </summary>
    void ResetDB();
}
