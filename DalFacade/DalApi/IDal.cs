namespace DalApi;

/// <summary>
/// Defines the root interface of the data access layer,
/// exposing access to all domain-specific repositories
/// and system configuration services.
/// </summary>
public interface IDal
{
    /// <summary>
    /// Provides access to courier-related data operations.
    /// </summary>
    ICourier Courier { get; }

    /// <summary>
    /// Provides access to order-related data operations.
    /// </summary>
    IOrder Order { get; }

    /// <summary>
    /// Provides access to delivery-related data operations.
    /// </summary>
    IDelivery Delivery { get; }

    /// <summary>
    /// Provides access to system configuration data.
    /// </summary>
    IConfig Config { get; }

    /// <summary>
    /// Clears all stored data and resets the data source to its initial state.
    /// </summary>
    void ResetDB();
}
