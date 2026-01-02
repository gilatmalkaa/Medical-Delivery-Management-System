namespace DalApi;

/// <summary>
/// Represents configuration values stored in DAL.
/// Provides clock handling and predefined system parameters.
/// </summary>
public interface IConfig
{
    DateTime Clock { get; set; }

    int MaxRange { get; set; }
    int SampleExpirationMinutes { get; set; }
    int MaxDeliveryDurationMinutes { get; set; }

    double FootSpeed { get; set; }
    double BikeSpeed { get; set; }
    double MotorcycleSpeed { get; set; }
    double CarSpeed { get; set; }

    double BaseDeliveryPrice { get; set; }
    double PricePerKm { get; set; }

    /// <summary>
    /// Restores configuration to default values.
    /// </summary>
    void Reset();
}
