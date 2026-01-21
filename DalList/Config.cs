namespace DalListData;

/// <summary>
/// Static configuration class used to simulate system constants and running IDs.
/// Defined as internal so it is accessible only within the DalList project.
/// Stores system-wide parameters such as time, speed settings, and auto-increment identifiers.
/// </summary>
internal static class Config
{
    /// <summary>
    /// Administrator identifier used for authentication
    /// and privileged system operations.
    /// </summary>
    public static string AdminId { get; set; } = string.Empty;

    /// <summary>
    /// Administrator password used for authentication
    /// and access to management-level features.
    /// </summary>
    public static string AdminPassword { get; set; } = string.Empty;

    // ===== Configuration values =====

    /// <summary>
    /// Maximum allowed delivery range (in kilometers)
    /// for assigning orders to couriers.
    /// </summary>
    internal static int MaxRange { get; set; } = 50;

    /// <summary>
    /// Maximum duration (in minutes) allowed for completing
    /// a delivery before it is considered late.
    /// </summary>
    internal static int MaxDeliveryDurationMinutes { get; set; } = 180;

    /// <summary>
    /// Time window (in minutes) after which medical samples
    /// are considered expired.
    /// </summary>
    internal static int SampleExpirationMinutes { get; set; } = 5;

    /// <summary>
    /// Average delivery speed (km/h) for couriers traveling on foot.
    /// </summary>
    internal static double FootSpeed { get; set; } = 5;

    /// <summary>
    /// Average delivery speed (km/h) for couriers using bicycles.
    /// </summary>
    internal static double BikeSpeed { get; set; } = 15;

    /// <summary>
    /// Average delivery speed (km/h) for couriers using cars.
    /// </summary>
    internal static double MotorcycleSpeed { get; set; } = 45;

    /// <summary>
    /// Price charged per kilometer for delivery cost calculation.
    /// </summary>
    internal static double CarSpeed { get; set; } = 60;
    /// <summary>
    /// Average delivery speed (km/h) for couriers using motorcycles.
    /// </summary>
    internal static double PricePerKm { get; set; } = 10;

    /// <summary>
    /// Base price added to every delivery,
    /// regardless of distance.
    /// </summary>
    internal static double BaseDeliveryPrice { get; set; } = 30;

    // ===== Running IDs =====

    /// <summary>
    /// Initial identifier value for orders.
    /// Used as the starting point for auto-increment order IDs.
    /// </summary>
    internal const int startOrderId = 1000;

    /// <summary>
    /// Stores the next available order identifier.
    /// Automatically incremented on each access.
    /// </summary>
    private static int nextOrderId = startOrderId;

    /// <summary>
    /// Stores the next available order identifier.
    /// Automatically incremented on each access.
    /// </summary>
    internal static int NextOrderId => nextOrderId++;

    /// <summary>
    /// Provides a unique auto-incremented identifier
    /// for a newly created order.
    /// </summary>
    internal const int startDeliveryId = 2000;

    /// <summary>
    /// Stores the next available delivery identifier.
    /// Automatically incremented on each access.
    /// </summary>
    private static int nextDeliveryId = startDeliveryId;

    /// <summary>
    /// Provides a unique auto-incremented identifier
    /// for a newly created delivery.
    /// </summary>
    internal static int NextDeliveryId => nextDeliveryId++;

    /// <summary>
    /// System clock – represents the current simulation time of the system.
    /// </summary>
    internal static DateTime Clock { get; set; } = DateTime.Now;

    /// <summary>
    /// Resets all configuration data to its initial state:
    /// resets running IDs, system clock, and configuration defaults.
    /// </summary>
    internal static void Reset()
    {
        nextOrderId = startOrderId;
        nextDeliveryId = startDeliveryId;

        Clock = DateTime.Now;

        MaxRange = 50;
        MaxDeliveryDurationMinutes = 180;
        SampleExpirationMinutes = 5;
        FootSpeed = 5;
        BikeSpeed = 15;
        MotorcycleSpeed = 45;
        CarSpeed = 60;
        PricePerKm = 10;
        BaseDeliveryPrice = 30;
    }
}
