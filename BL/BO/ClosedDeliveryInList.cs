using BL;

namespace BO;

/// <summary>
/// Represents a closed delivery in list view. 
/// Used for history display of courier deliveries.
/// </summary>
public class ClosedDeliveryInList
{
    /// <summary>
    /// Running ID of the delivery.
    /// </summary>
    public int DeliveryId { get; init; }

    /// <summary>
    /// Running ID of the related order.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Type of the delivery (standard, express, fragile, etc.).
    /// </summary>
    public DeliveryType Type { get; init; }

    /// <summary>
    /// Full address of the order.
    /// </summary>
    public string Address { get; init; } = "";

    /// <summary>
    /// Type of order (blood, samples, priority, etc.).
    /// </summary>
    public OrderType OrderType { get; init; }

    /// <summary>
    /// Actual distance traveled (nullable).
    /// </summary>
    public double? ActualDistance { get; init; }

    /// <summary>
    /// Total handling time until delivery completion.
    /// </summary>
    public TimeSpan TreatmentTime { get; init; }

    /// <summary>
    /// Status of delivery completion (Delivered / Failed / Canceled).
    /// Nullable if completion not known.
    /// </summary>
    public DeliveryStatus? CompletionStatus { get; init; }

    public override string ToString() => this.ToStringProperty();
}
