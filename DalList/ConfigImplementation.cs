using DalApi;
namespace Dal;
using DalListData;
using DO;

/// <summary>
/// Implements system configuration handling for the DAL,
/// providing access to timing, distance, speed, and pricing settings.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the system clock value.
    /// </summary>
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets or sets the maximum delivery range.
    /// </summary>
    public int MaxRange
    {
        get => (int)Config.AirDistance;
        set => Config.AirDistance = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for foot couriers.
    /// </summary>
    public double FootSpeed
    {
        get => Config.WalkingSpeed;
        set => Config.WalkingSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for bicycle couriers.
    /// </summary>
    public double BikeSpeed
    {
        get => Config.VehicleSpeed;
        set => Config.VehicleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for motorcycle couriers.
    /// </summary>
    public double MotorcycleSpeed
    {
        get => Config.MotorcycleSpeed;
        set => Config.MotorcycleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for car couriers.
    /// </summary>
    public double CarSpeed
    {
        get => Config.VehicleSpeed;
        set => Config.VehicleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the expiration time for samples in minutes.
    /// </summary>
    public int SampleExpirationMinutes
    {
        get => (int)Config.IdleTimeRange.TotalMinutes;
        set => Config.IdleTimeRange = TimeSpan.FromMinutes(value);
    }

    /// <summary>
    /// Gets or sets the maximum allowed delivery duration in minutes.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        get => (int)Config.DeliveryWindow.TotalMinutes;
        set => Config.DeliveryWindow = TimeSpan.FromMinutes(value);
    }

    /// <summary>
    /// Gets or sets the base price for a delivery.
    /// </summary>
    public double BaseDeliveryPrice
    {
        get => _baseDeliveryPrice;
        set => _baseDeliveryPrice = value;
    }

    /// <summary>
    /// Gets or sets the additional price charged per kilometer.
    /// </summary>
    public double PricePerKm
    {
        get => _pricePerKm;
        set => _pricePerKm = value;
    }

    /// <summary>
    /// Gets or sets the administrator identifier.
    /// </summary>
    public string AdminId
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the administrator password.
    /// </summary>
    public string AdminPassword
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Restores all configuration values to their default state.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }

    private static double _baseDeliveryPrice = 25;
    private static double _pricePerKm = 2.5;
}
