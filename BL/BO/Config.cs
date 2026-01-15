using Helpers;

namespace BO;

/// <summary>
/// Represents global configurable business parameters.
/// This object is shared between the BL and PL layers
/// to allow administrators to control system behavior.
/// </summary>
public class Config
{
    /// <summary>
    /// Administrator user identifier used for authentication.
    /// </summary>
    public string AdminId { get; set; } = "";

    /// <summary>
    /// Administrator password used for authentication.
    /// </summary>
    public string AdminPassword { get; set; } = "";

    /// <summary>
    /// Logical system clock used for time-based calculations.
    /// </summary>
    public DateTime Clock { get; set; }

    /// <summary>
    /// Maximum allowed delivery range (in kilometers).
    /// </summary>
    public int MaxRange { get; set; }

    /// <summary>
    /// Maximum time (in minutes) before a sample expires.
    /// </summary>
    public int SampleExpirationMinutes { get; set; }

    /// <summary>
    /// Maximum allowed delivery duration (in minutes).
    /// </summary>
    public int MaxDeliveryDurationMinutes { get; set; }

    /// <summary>
    /// Average delivery speed for couriers on foot (km/h).
    /// </summary>
    public double FootSpeed { get; set; }

    /// <summary>
    /// Average delivery speed for couriers using bicycles (km/h).
    /// </summary>
    public double BikeSpeed { get; set; }

    /// <summary>
    /// Average delivery speed for couriers using motorcycles (km/h).
    /// </summary>
    public double MotorcycleSpeed { get; set; }

    /// <summary>
    /// Average delivery speed for couriers using cars (km/h).
    /// </summary>
    public double CarSpeed { get; set; }

    /// <summary>
    /// Base price charged for any delivery.
    /// </summary>
    public double BaseDeliveryPrice { get; set; }

    /// <summary>
    /// Additional price charged per kilometer traveled.
    /// </summary>
    public double PricePerKm { get; set; }

    /// <summary>
    /// Returns a string representation of the configuration
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
