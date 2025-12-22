using Helpers;

namespace BO;

public class ClosedDeliveryInList
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }

    public DeliveryType Type { get; init; }
    public string? Address { get; init; }

    public OrderType OrderType { get; init; }

    public double? ActualDistance { get; init; }
    public TimeSpan TreatmentTime { get; init; }

    public DeliveryStatus? CompletionStatus { get; init; }

    public override string ToString() => this.ToStringProperty();
}
