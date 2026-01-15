using Helpers;

namespace BO;

/// <summary>
/// Represents an order that is currently being handled by a courier.
/// Used in courier active-delivery screens to display
/// real-time delivery and scheduling information.
/// </summary>
public class OrderInProgress
{
    /// <summary>
    /// Unique identifier of the delivery instance.
    /// </summary>
    public int DeliveryId { get; init; }

    /// <summary>
    /// Identifier of the related order.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Service level of the order.
    /// </summary>
    public OrderType Type { get; init; }

    /// <summary>
    /// Optional textual description of the order.
    /// </summary>
    public string? Description { get; init; } = "";

    /// <summary>
    /// Delivery destination address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Estimated air distance between origin and destination.
    /// </summary>
    public double AirDistance { get; init; }

    /// <summary>
    /// Actual distance traveled so far during the delivery,
    /// if available.
    /// </summary>
    public double? ActualDistance { get; init; }

    /// <summary>
    /// Name of the customer receiving the order.
    /// </summary>
    public string CustomerName { get; init; } = "";

    /// <summary>
    /// Customer contact phone number.
    /// </summary>
    public string CustomerPhone { get; init; } = "";

    /// <summary>
    /// Date and time when the order was opened.
    /// </summary>
    public DateTime OpenDate { get; init; }

    /// <summary>
    /// Date and time when the delivery process started.
    /// </summary>
    public DateTime StartDeliveryDate { get; init; }

    /// <summary>
    /// Expected arrival time based on distance and courier speed.
    /// </summary>
    public DateTime ExpectedArrivalTime { get; init; }

    /// <summary>
    /// Latest allowed supply time according to business constraints.
    /// </summary>
    public DateTime LatestSupplyTime { get; init; }

    /// <summary>
    /// Current logical status of the order.
    /// </summary>
    public OrderStatus OrderStatus { get; init; }

    /// <summary>
    /// Current schedule status of the delivery
    /// relative to expected arrival time.
    /// </summary>
    public ScheduleStatus ScheduleStatus { get; init; }

    /// <summary>
    /// Remaining time until the order must be completed.
    /// </summary>
    public TimeSpan RemainingTimeToFinishOrder { get; init; }

    /// <summary>
    /// Returns a string representation of the active order
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
