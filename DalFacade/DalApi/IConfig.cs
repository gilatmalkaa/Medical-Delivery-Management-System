namespace DalApi;

/// <summary>
/// Defines configuration settings stored in the DAL,
/// including administrative credentials, system clock,
/// delivery constraints, speeds, and pricing parameters.
/// </summary>
public interface IConfig
{
    /// <summary>
    /// Gets or sets the administrator identifier.
    /// </summary>
    string AdminId { get; set; }

    /// <summary>
    /// Gets or sets the administrator password.
    /// </summary>
    string AdminPassword { get; set; }

    /// <summary>
    /// Gets or sets the system clock value.
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed delivery range.
    /// </summary>
    int MaxRange { get; set; }

    /// <summary>
    /// Gets or sets the expiration time for samples in minutes.
    /// </summary>
    int SampleExpirationMinutes { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed delivery duration in minutes.
    /// </summary>
    int MaxDeliveryDurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets the delivery speed for foot couriers.
    /// </summary>
    double FootSpeed { get; set; }

    /// <summary>
    /// Gets or sets the delivery speed for bike couriers.
    /// </summary>
    double BikeSpeed { get; set; }

    /// <summary>
    /// Gets or sets the delivery speed for motorcycle couriers.
    /// </summary>
    double MotorcycleSpeed { get; set; }

    /// <summary>
    /// Gets or sets the delivery speed for car couriers.
    /// </summary>
    double CarSpeed { get; set; }

    /// <summary>
    /// Gets or sets the base price for a delivery.
    /// </summary>
    double BaseDeliveryPrice { get; set; }

    /// <summary>
    /// Gets or sets the additional price per kilometer.
    /// </summary>
    double PricePerKm { get; set; }

    /// <summary>
    /// Restores all configuration values to their default settings.
    /// </summary>
    void Reset();
}
