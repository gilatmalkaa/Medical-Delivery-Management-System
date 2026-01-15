using Helpers;

namespace BO;

/// <summary>
/// Lightweight courier summary object used for list and table views.
/// Contains only essential information required for display purposes.
/// </summary>
public class CourierInList
{
    /// <summary>
    /// Unique identifier of the courier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Full name of the courier.
    /// </summary>
    public string? FullName { get; init; }

    /// <summary>
    /// Indicates whether the courier is currently active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Courier transportation type.
    /// </summary>
    public CourierType Type { get; init; }

    /// <summary>
    /// Date when the courier started working.
    /// </summary>
    public DateTime? StartWorkDate { get; init; }

    /// <summary>
    /// Total number of deliveries completed on time.
    /// </summary>
    public int TotalDeliveriesOnTime { get; init; }

    /// <summary>
    /// Total number of deliveries completed late.
    /// </summary>
    public int TotalDeliveriesLate { get; init; }

    /// <summary>
    /// Identifier of the current active delivery,
    /// or null if no delivery is in progress.
    /// </summary>
    public int? CurrentDeliveryId { get; init; }

    /// <summary>
    /// Indicates whether the courier can be safely deleted
    /// (typically true if the courier has no delivery history).
    /// </summary>
    public bool CanDelete { get; init; }

    /// <summary>
    /// Total number of deliveries handled by the courier.
    /// </summary>
    public int TotalDeliveries { get; init; }

    /// <summary>
    /// Returns a string representation of the courier summary
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
