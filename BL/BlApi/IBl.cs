namespace BlApi;

/// <summary>
/// Business Logic main access point.
/// Provides access to domain services.
/// </summary>
public interface IBl
{
    ICourier Couriers { get; }
    IOrder Orders { get; }
    IDelivery Deliveries { get; }
    IAdmin Admin { get; }
}
