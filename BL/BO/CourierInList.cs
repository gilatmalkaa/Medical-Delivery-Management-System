using Helpers;

namespace BO;

public class CourierInList
{
    public int Id { get; init; }
    public string? FullName { get; init; }

    public bool IsActive { get; init; }
    public DeliveryType Type { get; init; }

    public DateTime StartWorkDate { get; init; }

    public int TotalDeliveriesOnTime { get; init; }
    public int TotalDeliveriesLate { get; init; }

    public int? CurrentDeliveryId { get; init; }

    public override string ToString() => this.ToStringProperty();
}
