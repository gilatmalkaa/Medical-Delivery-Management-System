namespace DalApi;

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

    void Reset();
}