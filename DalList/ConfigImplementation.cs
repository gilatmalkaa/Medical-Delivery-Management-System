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
    /// Gets or sets the maximum allowed delivery range (in kilometers)
    /// for assigning orders to couriers.
    /// </summary>
    public int MaxRange
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MaxRange;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MaxRange = value;
    }

    /// <summary>
    /// Gets or sets the maximum delivery duration (in minutes)
    /// before a delivery is considered late.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MaxDeliveryDurationMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MaxDeliveryDurationMinutes = value;
    }

    /// <summary>
    /// Gets or sets the expiration time (in minutes)
    /// for medical or time-sensitive samples.
    /// </summary>
    public int SampleExpirationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.SampleExpirationMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.SampleExpirationMinutes = value;
    }

    /// <summary>
    /// Gets or sets the average delivery speed (km/h)
    /// for couriers traveling on foot.
    /// </summary>
    public double FootSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.FootSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.FootSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average delivery speed (km/h)
    /// for couriers using bicycles.
    /// </summary>
    public double BikeSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.BikeSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.BikeSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average delivery speed (km/h)
    /// for couriers using bicycles.
    /// </summary>
    public double MotorcycleSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MotorcycleSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MotorcycleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average delivery speed (km/h)
    /// for couriers using cars.
    /// </summary>
    public double CarSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.CarSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.CarSpeed = value;
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer
    /// for delivery cost calculation.
    /// </summary>
    public double PricePerKm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.PricePerKm;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.PricePerKm = value;
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer
    /// for delivery cost calculation.
    /// </summary>
    public double BaseDeliveryPrice
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.BaseDeliveryPrice;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.BaseDeliveryPrice = value;
    }

    /// <summary>
    /// Gets or sets the administrator identifier
    /// used for authentication and privileged operations.
    /// </summary>
    public string AdminId
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.AdminId;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AdminId = value;
    }

    /// <summary>
    /// Gets or sets the administrator password
    /// used for authentication and access to system management features.
    /// </summary>
    public string AdminPassword
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.AdminPassword;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AdminPassword = value;
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
