using DalApi;
namespace Dal;
using DalListData;
using DO;


/// <summary>
/// DAL implementation of system configuration.
/// Reads and writes configuration from XML storage.
/// Some values are not managed in XML DAL and therefore return defaults.
/// </summary>
internal class ConfigImplementation : IConfig
{
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    // ==== MAPPED PROPERTIES (names expected by BL) ====

    // MaxRange ← mapped to AirDistance (km)
    public int MaxRange
    {
        get => (int)Config.AirDistance;
        set => Config.AirDistance = value;
    }

    // FootSpeed ← WalkingSpeed
    public double FootSpeed
    {
        get => Config.WalkingSpeed;
        set => Config.WalkingSpeed = value;
    }

    // BikeSpeed ← VehicleSpeed
    public double BikeSpeed
    {
        get => Config.VehicleSpeed;
        set => Config.VehicleSpeed = value;
    }

    // MotorcycleSpeed ← MotorcycleSpeed (exists directly)
    public double MotorcycleSpeed
    {
        get => Config.MotorcycleSpeed;
        set => Config.MotorcycleSpeed = value;
    }

    // CarSpeed ← VehicleSpeed (same source)
    public double CarSpeed
    {
        get => Config.VehicleSpeed;
        set => Config.VehicleSpeed = value;
    }

    // SampleExpirationMinutes ← IdleTimeRange
    public int SampleExpirationMinutes
    {
        get => (int)Config.IdleTimeRange.TotalMinutes;
        set => Config.IdleTimeRange = TimeSpan.FromMinutes(value);
    }

    // MaxDeliveryDurationMinutes ← DeliveryWindow
    public int MaxDeliveryDurationMinutes
    {
        get => (int)Config.DeliveryWindow.TotalMinutes;
        set => Config.DeliveryWindow = TimeSpan.FromMinutes(value);
    }

    // Prices (add defaults if not originally defined)
    public double BaseDeliveryPrice
    {
        get => _baseDeliveryPrice;
        set => _baseDeliveryPrice = value;
    }
    private static double _baseDeliveryPrice = 25;

    public double PricePerKm
    {
        get => _pricePerKm;
        set => _pricePerKm = value;
    }
    private static double _pricePerKm = 2.5;

    public void Reset()
    {
        Config.Reset();
    }
}
