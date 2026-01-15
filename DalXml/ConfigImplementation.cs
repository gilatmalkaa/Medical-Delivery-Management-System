using DalApi;
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
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets or sets the maximum delivery range.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int MaxRange
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for foot couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double FootSpeed
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for bicycle couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double BikeSpeed
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for motorcycle couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double MotorcycleSpeed
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the delivery speed for car couriers.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double CarSpeed
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the sample expiration time in minutes.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int SampleExpirationMinutes
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the maximum delivery duration in minutes.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public int MaxDeliveryDurationMinutes
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double PricePerKm
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the base delivery price.
    /// Returns a default value as it is not supported in this DAL implementation.
    /// </summary>
    public double BaseDeliveryPrice
    {
        get => 0;
        set { }
    }

    /// <summary>
    /// Gets or sets the administrator identifier.
    /// </summary>
    public string AdminId
    {
        get => Config.AdminId;
        set => Config.AdminId = value;
    }

    /// <summary>
    /// Gets or sets the administrator password.
    /// </summary>
    public string AdminPassword
    {
        get => Config.AdminPassword;
        set => Config.AdminPassword = value;
    }

    /// <summary>
    /// Restores all configuration values to their default state.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }
}
