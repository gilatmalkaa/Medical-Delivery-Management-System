using Helpers;

namespace BO;

/// <summary>
/// Represents a delivery entry associated with a specific order.
/// Intended for display within order detail views.
/// </summary>
public class DeliveryPerOrderInList
{
    /// <summary>
    /// Unique identifier of the delivery.
    /// </summary>
    public int DeliveryId { get; init; }

    /// <summary>
    /// Identifier of the courier assigned to the delivery,
    /// or null if no courier is assigned.
    /// </summary>
    public int? CourierId { get; init; }

    /// <summary>
    /// Full name of the courier assigned to the delivery.
    /// </summary>
    public string? CourierName { get; init; }

    /// <summary>
    /// Transportation type of the assigned courier.
    /// </summary>
    public CourierType CourierType { get; init; }

    /// <summary>
    /// Date and time when the delivery was started.
    /// </summary>
    public DateTime StartDeliveryDate { get; init; }

    /// <summary>
    /// Final completion status of the delivery,
    /// or null if the delivery is still in progress.
    /// </summary>
    public DeliveryStatus? CompletionStatus { get; init; }

    /// <summary>
    /// Date and time when the delivery was completed,
    /// or null if the delivery has not yet finished.
    /// </summary>
    public DateTime? EndDeliveryDate { get; init; }

    /// <summary>
    /// Returns a string representation of the delivery
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
