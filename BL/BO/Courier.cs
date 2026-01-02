using Helpers;

namespace BO;

/// <summary>
/// Represents a courier entity in the business layer.
/// Contains personal details, work status and delivery statistics.
/// </summary>
public class Courier
{
    /// <summary>Unique courier identifier.</summary>
    public int Id { get; init; }

    /// <summary>Courier full name.</summary>
    public string? Name { get; set; }

    /// <summary>Courier phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Courier email address.</summary>
    public string? Email { get; set; }

    /// <summary>Courier digital signature (optional).</summary>
    public string? Signature { get; set; }

    /// <summary>Determines whether courier is currently active.</summary>
    public bool IsActive { get; set; }

    /// <summary>Maximum distance courier is willing to handle for personal deliveries.</summary>
    public double? MaxPersonalDeliveryDistance { get; set; }

    /// <summary>Courier transportation type.</summary>
    public DeliveryType Type { get; init; }

    /// <summary>Work start date.</summary>
    public DateTime StartWorkDate { get; init; }

    /// <summary>Total deliveries ever handled.</summary>
    public int TotalDeliveries { get; set; }

    /// <summary>Total work time (minutes / configurable meaning).</summary>
    public int TimeInWork { get; set; }

    /// <summary>Indicates whether courier is currently free to receive delivery.</summary>
    public bool IsAvailable { get; set; }

    /// <summary>Current active delivery if exists.</summary>
    public OrderInProgress? CurrentOrder { get; set; }

    public override string ToString() => this.ToStringProperty();
}
