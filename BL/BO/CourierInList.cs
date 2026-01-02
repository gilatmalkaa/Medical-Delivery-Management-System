using Helpers;

namespace BO;

/// <summary>
/// Lightweight courier summary used in list views.
/// </summary>
public class CourierInList
{
    public int Id { get; init; }
    public string? FullName { get; init; }

    public bool IsActive { get; init; }
    public DeliveryType Type { get; init; }

    public DateTime StartWorkDate { get; init; }

    /// <summary>Total amount of deliveries completed on time.</summary>
    public int TotalDeliveriesOnTime { get; init; }

    /// <summary>Total amount of deliveries completed late.</summary>
    public int TotalDeliveriesLate { get; init; }

    /// <summary>Current delivery Id if exists.</summary>
    public int? CurrentDeliveryId { get; init; }

    public override string ToString() => this.ToStringProperty();
}
