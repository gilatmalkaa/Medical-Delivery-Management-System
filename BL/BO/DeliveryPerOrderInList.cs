using Helpers;

namespace BO;

/// <summary>
/// Represents a delivery belonging to a specific order,
/// displayed inside order details.
/// </summary>
public class DeliveryPerOrderInList
{
    public int DeliveryId { get; init; }

    public int? CourierId { get; init; }
    public string? CourierName { get; init; }

    public DeliveryType Type { get; init; }

    public DateTime StartDeliveryDate { get; init; }

    /// <summary>Final completion status (if finished).</summary>
    public DeliveryStatus? CompletionStatus { get; init; }

    public DateTime? EndDeliveryDate { get; init; }

    public override string ToString() => this.ToStringProperty();
}
