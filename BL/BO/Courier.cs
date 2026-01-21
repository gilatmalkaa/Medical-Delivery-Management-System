using Helpers;
using System.Xml.Serialization;

namespace BO;

/// <summary>
/// Represents a courier entity in the business layer.
/// Stores personal information, work status,
/// availability, and delivery performance statistics.
/// </summary>
public class Courier
{
    /// <summary>
    /// Unique identifier of the courier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Full name of the courier.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Password used for courier authentication.
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// Courier phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Courier email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Optional digital signature of the courier.
    /// </summary>
    public string? Signature { get; set; }

    /// <summary>
    /// Indicates whether the courier is currently active in the system.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Maximum delivery distance the courier is willing
    /// to handle for personal deliveries.
    /// </summary>
    public double? MaxPersonalDeliveryDistance { get; set; }

    /// <summary>
    /// Courier transportation type used for deliveries.
    /// </summary>
    public DeliveryType Type { get; set; }

    /// <summary>
    /// Date when the courier started working.
    /// </summary>
    public DateTime? StartWorkDate { get; init; }

    /// <summary>
    /// Total number of deliveries handled by the courier.
    /// </summary>
    public int TotalDeliveries =>
        DeliveredOnTimeCount + DeliveredLateCount;

    /// <summary>
    /// Number of deliveries completed on time.
    /// </summary>
    public int DeliveredOnTimeCount { get; set; }

    /// <summary>
    /// Number of deliveries completed late.
    /// </summary>
    public int DeliveredLateCount { get; set; }

    /// <summary>
    /// Indicates whether the courier is currently
    /// available to receive a new delivery.
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Current active order assigned to the courier,
    /// or null if no delivery is in progress.
    /// </summary>
    public OrderInProgress? CurrentOrder { get; set; }

    /// <summary>
    /// Returns a string representation of the courier
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
