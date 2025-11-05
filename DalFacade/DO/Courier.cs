namespace DO;

/// <summary>
/// Courier entity represents a delivery courier in the system.
/// Each courier has identifying details, contact information,
/// and operational parameters such as delivery range and vehicle type.
/// </summary>
/// <param name="Id">Personal unique ID of the courier.</param>
/// <param name="FullName">Full name of the courier (private and family name).</param>
/// <param name="Phone">Courier’s cellphone number (10 digits, numeric only).</param>
/// <param name="Email">Courier’s email address.</param>
/// <param name="Signature">Courier’s digital signature for confirmation (if applicable).</param>
/// <param name="MaxPersonalDeliveryDistance">Maximum distance (in km) the courier can deliver personally.</param>
/// <param name="Type">Type of courier (vehicle type: foot/bike/car).</param>
/// <param name="IsActive">Whether the courier is currently active in the system (default true).</param>
/// <param name="StartWorkDate">Date and time when the courier started working in the company.</param>
public record Courier(
    int Id,
    string FullName,
    string Phone,
    string Email,
    string Signature,
    double MaxPersonalDeliveryDistance,
    DeliveryType Type = DeliveryType.Foot,
    bool IsActive = true,
    DateTime? StartWorkDate = default
)
{
    /// <summary>
    /// Default constructor for initializing an empty courier object
    /// with default values for all properties.
    /// </summary>
    public Courier() : this(0, "", "", "", "", 0, DeliveryType.Foot, true, default) { }
}
