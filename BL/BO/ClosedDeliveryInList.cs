using Helpers;

namespace BO;

/// <summary>
/// Represents a summarized view of a completed delivery,
/// intended for display in lists and reports.
/// </summary>
public class ClosedDeliveryInList
{
    /// <summary>
    /// Unique identifier of the delivery.
    /// </summary>
    public int DeliveryId { get; init; }

    /// <summary>
    /// Identifier of the related order.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Delivery execution type (e.g., regular, express).
    /// </summary>
    public DeliveryType Type { get; init; }

    /// <summary>
    /// Destination address of the delivery.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Type/category of the original order.
    /// </summary>
    public OrderType OrderType { get; init; }

    /// <summary>
    /// Actual distance traveled during the delivery.
    /// </summary>
    public double? ActualDistance { get; init; }

    /// <summary>
    /// Total time spent handling the delivery,
    /// from start to completion.
    /// </summary>
    public TimeSpan TreatmentTime { get; init; }

    /// <summary>
    /// Final completion status of the delivery.
    /// </summary>
    public DeliveryStatus? CompletionStatus { get; init; }

    /// <summary>
    /// Returns a string representation of the delivery
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
