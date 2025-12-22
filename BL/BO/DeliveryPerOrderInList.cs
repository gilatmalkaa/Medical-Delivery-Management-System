using Helpers;

namespace BO;

public class DeliveryPerOrderInList
{
    public int DeliveryId { get; init; }

    public int? CourierId { get; init; }
    public string? CourierName { get; init; }

    public DeliveryType Type { get; init; }

    public DateTime StartDeliveryDate { get; init; }
    public DeliveryStatus? CompletionStatus { get; init; }
    public DateTime? EndDeliveryDate { get; init; }

    public override string ToString() => this.ToStringProperty();
}
