namespace DalApi;

public interface IConfig
{
    DateTime Clock { get; set; }
    int SampleExpirationMinutes { get; set; }
    int MaxDeliveryDurationMinutes { get; set; }
    double FootSpeed { get; set; }
    double BikeSpeed { get; set; }
    double CarSpeed { get; set; }
    void Reset();
}
