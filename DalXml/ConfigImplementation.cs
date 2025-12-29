using DalApi;
namespace Dal;

internal class ConfigImplementation : IConfig
{
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    // DalXml does not manage these values – return defaults

    public int MaxRange
    {
        get => 0;
        set { /* not supported in DalXml */ }
    }

    public double FootSpeed
    {
        get => 0;
        set { }
    }

    public double BikeSpeed
    {
        get => 0;
        set { }
    }

    public double MotorcycleSpeed
    {
        get => 0;
        set { }
    }

    public double CarSpeed
    {
        get => 0;
        set { }
    }

    public int SampleExpirationMinutes
    {
        get => 0;
        set { }
    }

    public int MaxDeliveryDurationMinutes
    {
        get => 0;
        set { }
    }

    public double PricePerKm
    {
        get => 0;
        set { }
    }

    public double BaseDeliveryPrice
    {
        get => 0;
        set { }
    }

    public void Reset()
    {
        Config.Reset();
    }
}
