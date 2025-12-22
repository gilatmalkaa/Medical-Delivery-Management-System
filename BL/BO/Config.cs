using Helpers;

namespace BO;

/// <summary>
/// Configuration values exposed to the presentation layer.
/// </summary>
public class Config
{
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
