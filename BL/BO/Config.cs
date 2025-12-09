namespace BO;

public class Config
{
    public int MaxRange { get; init; }
    public double FootSpeed { get; init; }
    public double BikeSpeed { get; init; }
    public double MotorcycleSpeed { get; init; }
    public double CarSpeed { get; init; }
    public int SampleExpirationMinutes { get; init; }
    public int MaxDeliveryDurationMinutes { get; init; }
    public double BaseDeliveryPrice { get; init; }
    public double PricePerKm { get; init; }
    public override string ToString() => this.ToStringProperty();
}
