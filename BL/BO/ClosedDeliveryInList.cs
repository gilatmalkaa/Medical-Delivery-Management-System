using Helpers;

namespace BO;

/// <summary>
/// Represents a closed delivery displayed in lists.
/// Contains summary information about a completed delivery.
/// </summary>
public class ClosedDeliveryInList
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }

    public DeliveryType Type { get; init; }
    public string? Address { get; init; }

    public OrderType OrderType { get; init; }

    public double? ActualDistance { get; init; }

    /// <summary>
    /// Total handling time of the delivery.
    /// </summary>
    public TimeSpan TreatmentTime { get; init; }

    /// <summary>
    /// Final completion status.
    /// </summary>
    public DeliveryStatus? CompletionStatus { get; init; }

    public override string ToString() => this.ToStringProperty();
}
