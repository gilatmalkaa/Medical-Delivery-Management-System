using Helpers;

namespace BO;

/// <summary>
/// Represents global configurable business parameters.
/// This object is shared between BL and PL layers
/// to allow the administrator to control system behavior.
/// </summary>
public class Config
{
    /// <summary>
    /// Logical system clock.
    /// </summary>
    public DateTime Clock { get; set; }

    public int MaxRange { get; set; }
    public int SampleExpirationMinutes { get; set; }
    public int MaxDeliveryDurationMinutes { get; set; }

    public double FootSpeed { get; set; }
    public double BikeSpeed { get; set; }
    public double MotorcycleSpeed { get; set; }
    public double CarSpeed { get; set; }

    public double BaseDeliveryPrice { get; set; }
    public double PricePerKm { get; set; }

    public override string ToString() => this.ToStringProperty();
}
