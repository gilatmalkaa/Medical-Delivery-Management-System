namespace BlImplementation;

using BlApi;

/// <summary>
/// Central Business Logic container.
/// Implements the IBl facade and provides
/// access to all business domain services.
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Provides access to courier-related business logic,
    /// including courier management and order assignment.
    /// </summary>
    public ICourier Couriers { get; } = new CourierImplementation();

    /// <summary>
    /// Provides access to order-related business logic,
    /// including order creation, updates, and queries.
    /// </summary>
    public IOrder Orders { get; } = new OrderImplementation();

    /// <summary>
    /// Provides access to delivery-related business logic,
    /// including delivery lifecycle management and tracking.
    /// </summary>
    public IDelivery Deliveries { get; } = new DeliveryImplementation();

    /// <summary>
    /// Provides access to administrative system operations,
    /// such as configuration management, system clock control,
    /// database initialization, and authentication.
    /// </summary>
    public IAdmin Admin { get; } = new AdminImplementation();
}
