using DalApi;
namespace Dal;
using DalListData;
using DO;

/// <summary>
/// Implementation class for the configuration interface (IConfig).
/// Provides controlled access to system configuration values 
/// defined in the internal DalList.Config class.
/// </summary>
internal class ConfigImplementation : IConfig
{
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    public int MaxRange
    {
        get => Config.MaxRange;
        set => Config.MaxRange = value;
    }

    public double FootSpeed
    {
        get => Config.FootSpeed;
        set => Config.FootSpeed = value;
    }

    public double BikeSpeed
    {
        get => Config.BikeSpeed;
        set => Config.BikeSpeed = value;
    }

    public double MotorcycleSpeed
    {
        get => Config.MotorcycleSpeed;
        set => Config.MotorcycleSpeed = value;
    }

    public double CarSpeed
    {
        get => Config.CarSpeed;
        set => Config.CarSpeed = value;
    }

    public int SampleExpirationMinutes
    {
        get => Config.SampleExpirationMinutes;
        set => Config.SampleExpirationMinutes = value;
    }

    public int MaxDeliveryDurationMinutes
    {
        get => Config.MaxDeliveryDurationMinutes;
        set => Config.MaxDeliveryDurationMinutes = value;
    }

    public double BaseDeliveryPrice
    {
        get => Config.BaseDeliveryPrice;
        set => Config.BaseDeliveryPrice = value;
    }

    public double PricePerKm
    {
        get => Config.PricePerKm;
        set => Config.PricePerKm = value;
    }

    /// <summary>
    /// Resets all configuration settings and counters 
    /// to their initial values as defined in DalList.Config.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }
}
