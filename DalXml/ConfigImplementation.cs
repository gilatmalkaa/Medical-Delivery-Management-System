using DalApi;
using System.Runtime.CompilerServices;
namespace Dal;

/// <summary>
/// Provides an XML-based DAL implementation for system configuration,
/// exposing only the values supported by the XML data source
/// and returning default values for unsupported settings.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the system clock value.
    /// </summary>
    /// 
    public DateTime Clock
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.Clock;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets or sets the maximum delivery range.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int MaxRange
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for foot couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double FootSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for bicycle couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double BikeSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for motorcycle couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double MotorcycleSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for car couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double CarSpeed
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the sample expiration time in minutes.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int SampleExpirationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the maximum delivery duration in minutes.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double PricePerKm
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the base delivery price.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double BaseDeliveryPrice
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set { }
    }

    /// <summary>
    /// Gets or sets the administrator identifier.
    /// </summary>
    public string AdminId
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.AdminId;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.AdminId = value;
    }

    /// <summary>
    /// Gets or sets the administrator password.
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
}
