using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;

/// <summary>
/// XML-based implementation of system configuration.
/// Reads and writes configuration values from data-config.xml.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the current system clock used by the simulator.
    /// This clock represents the logical time of the system
    /// and is synchronized across all components.
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
    /// Gets or sets the maximum delivery duration (in minutes).
    /// Deliveries exceeding this duration may be considered late.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MaxDeliveryDurationMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MaxDeliveryDurationMinutes = value;
    }

    /// <summary>
    /// Gets or sets the expiration time (in minutes) for time-sensitive samples.
    /// After this duration, samples are considered expired.
    /// </summary>
    public int SampleExpirationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.SampleExpirationMinutes;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.SampleExpirationMinutes = value;
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering on foot.
    /// Used for delivery time estimation.
    /// </summary>
    public double FootSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.FootSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.FootSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by bicycle.
    /// Used for delivery time estimation.
    /// </summary>
    public double BikeSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.BikeSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.BikeSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by motorcycle.
    /// Used for delivery time estimation.
    /// </summary>
    public double MotorcycleSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.MotorcycleSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.MotorcycleSpeed = value;
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by car.
    /// Used for delivery time estimation.
    /// </summary>
    public double CarSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.CarSpeed;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.CarSpeed = value;
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer for a delivery.
    /// Used to calculate the total delivery cost.
    /// </summary>
    public double PricePerKm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.PricePerKm;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.PricePerKm = value;
    }

    /// <summary>
    /// Gets or sets the base delivery price added to every order,
    /// regardless of distance.
    /// </summary>
    public double BaseDeliveryPrice
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.BaseDeliveryPrice;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.BaseDeliveryPrice = value;
    }

    /// <summary>
    /// Gets or sets the administrator identifier used for system authentication
    /// and privileged operations.
    /// </summary>
    public string AdminId
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.AdminId;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AdminId = value;
    }

    /// <summary>
    /// Gets or sets the administrator password used for system authentication
    /// and access to management-level features.
    /// </summary>
    public string AdminPassword
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.AdminPassword;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AdminPassword = value;
    }

    /// <summary>
    /// Resets all configuration values to their default state,
    /// including system clock, speed settings, pricing,
    /// and other simulation parameters.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Reset()
    {
        Config.Reset();
    }

}
