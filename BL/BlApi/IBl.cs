namespace BlApi;

/// <summary>
/// Business Logic main access point.
/// Serves as a facade that exposes all domain services
/// to the presentation layer.
/// </summary>
public interface IBl
{
    /// <summary>
    /// Provides access to courier-related business operations,
    /// including courier management, availability checks,
    /// and order assignment.
    /// </summary>
    ICourier Couriers { get; }

    /// <summary>
    /// Provides access to order-related business operations,
    /// such as order creation, updates, and status queries.
    /// </summary>
    IOrder Orders { get; }

    /// <summary>
    /// Provides access to delivery-related business operations,
    /// including delivery lifecycle management and tracking.
    /// </summary>
    IDelivery Deliveries { get; }

    /// <summary>
    /// Provides access to administrative system operations,
    /// such as configuration management, system clock control,
    /// database initialization, and authentication.
    /// </summary>
    IAdmin Admin { get; }
}
