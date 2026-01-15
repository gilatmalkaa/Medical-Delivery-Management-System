using Helpers;

namespace BO;

/// <summary>
/// Represents a full business order entity.
/// Contains customer details, delivery information,
/// timing constraints, and scheduling status.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Service level of the order.
    /// </summary>
    public OrderType Type { get; init; }

    /// <summary>
    /// Optional textual description of the order.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Delivery destination address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Latitude coordinate of the delivery location.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    /// Longitude coordinate of the delivery location.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    /// Calculated air distance between origin and destination.
    /// </summary>
    public double AirDistance { get; init; }

    /// <summary>
    /// Name of the customer who placed the order.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Customer contact phone number.
    /// </summary>
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// Additional details about the package or delivery contents.
    /// </summary>
    public string? PackageDetails { get; set; }

    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Expected delivery completion time.
    /// </summary>
    public DateTime? ExpectedDeliveryTime { get; set; }

    /// <summary>
    /// Latest allowed delivery time according to business rules.
    /// </summary>
    public DateTime? MaxDeliveryTime { get; set; }

    /// <summary>
    /// Current logical status of the order.
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// Current schedule status of the order
    /// relative to expected delivery time.
    /// </summary>
    public ScheduleStatus ScheduleStatus { get; set; }

    /// <summary>
    /// Remaining time until delivery deadline,
    /// if applicable.
    /// </summary>
    public TimeSpan? TimeRemaining { get; set; }

    /// <summary>
    /// Collection of deliveries associated with this order.
    /// </summary>
    public List<DeliveryPerOrderInList>? Deliveries { get; init; }

    /// <summary>
    /// Returns a string representation of the order
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
