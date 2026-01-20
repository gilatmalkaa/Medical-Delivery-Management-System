using DalApi;
namespace Dal;
using DalListData;
using DO;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.Clock;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets or sets the maximum delivery range.
    /// </summary>
    public int MaxRange
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => (int)Config.AirDistance;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AirDistance = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for foot couriers.
    /// </summary>
    public double FootSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.WalkingSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.WalkingSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for bicycle couriers.
    /// </summary>
    public double BikeSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.VehicleSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.VehicleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for motorcycle couriers.
    /// </summary>
    public double MotorcycleSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MotorcycleSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MotorcycleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the delivery speed for car couriers.
    /// </summary>
    public double CarSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.VehicleSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.VehicleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the expiration time for samples in minutes.
    /// </summary>
    public int SampleExpirationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => (int)Config.IdleTimeRange.TotalMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.IdleTimeRange = TimeSpan.FromMinutes(value);
    }

    /// <summary>
    /// Gets or sets the maximum allowed delivery duration in minutes.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => (int)Config.DeliveryWindow.TotalMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.DeliveryWindow = TimeSpan.FromMinutes(value);
    }

    /// <summary>
    /// Gets or sets the base price for a delivery.
    /// </summary>
    public double BaseDeliveryPrice
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _baseDeliveryPrice;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _baseDeliveryPrice = value;
    }

    /// <summary>
    /// Gets or sets the additional price charged per kilometer.
    /// </summary>
    public double PricePerKm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => _pricePerKm;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => _pricePerKm = value;
    }

    /// <summary>
    /// Gets or sets the administrator identifier.
    /// </summary>
    public string AdminId
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => throw new NotImplementedException();
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the administrator password.
    /// </summary>
    public string AdminPassword
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => throw new NotImplementedException();
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Restores all configuration values to their default state.
    /// </summary>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Reset()
    {
        Config.Reset();
    }

    /// <summary>
    /// Base price charged for every delivery,
    /// regardless of distance.
    /// </summary>
    private static double _baseDeliveryPrice = 25;

    /// <summary>
    /// Additional price charged per kilometer
    /// beyond the base delivery price.
    /// </summary>
    private static double _pricePerKm = 2.5;
}
