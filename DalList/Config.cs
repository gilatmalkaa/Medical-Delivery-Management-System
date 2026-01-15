namespace DalListData;

/// <summary>
/// Static configuration class used to simulate system constants and running IDs.
/// Defined as internal so it is accessible only within the DalList project.
/// Stores system-wide parameters such as time, speed settings, and auto-increment identifiers.
/// </summary>
internal static class Config
{
    public static string AdminId { get; set; } = string.Empty;
    public static string AdminPassword { get; set; } = string.Empty;
    /// <summary>
    /// Starting ID number for orders.
    /// </summary>
    internal const int startOrderId = 1000;

    /// <summary>
    /// Private counter to hold the next available order ID.
    /// </summary>
    private static int nextOrderId = startOrderId;

    /// <summary>
    /// Property that returns the next running order ID and increments it automatically.
    /// </summary>
    internal static int NextOrderId { get => nextOrderId++; }

    /// <summary>
    /// Starting ID number for deliveries.
    /// </summary>
    internal const int startDeliveryId = 2000;

    /// <summary>
    /// Private counter to hold the next available delivery ID.
    /// </summary>
    private static int nextDeliveryId = startDeliveryId;

    /// <summary>
    /// Property that returns the next running delivery ID and increments it automatically.
    /// </summary>
    internal static int NextDeliveryId { get => nextDeliveryId++; }

    /// <summary>
    /// System clock – represents the current simulation time of the system.
    /// </summary>
    internal static DateTime Clock { get; set; } = DateTime.Now;

    /// <summary>
    /// Identifier of the system manager (used for management-level actions).
    /// </summary>
    internal static int ManagerId { get; set; } = 1;

    /// <summary>
    /// Digital signature representing the manager's approval.
    /// </summary>
    internal static string ManagerSignature { get; set; } = "SignedByManager";

    /// <summary>
    /// Company’s base address for deliveries.
    /// </summary>
    internal static string? CompanyAddress { get; set; } = "Herzl St 10, Jerusalem";

    /// <summary>
    /// Geographic latitude of the company’s location.
    /// </summary>
    internal static double? Latitude { get; set; } = 31.7683;

    /// <summary>
    /// Geographic longitude of the company’s location.
    /// </summary>
    internal static double? Longitude { get; set; } = 35.2137;

    /// <summary>
    /// Default air distance (in kilometers) used for estimation.
    /// </summary>
    internal static double AirDistance { get; set; } = 10.5;

    /// <summary>
    /// Average vehicle delivery speed (in km/h).
    /// </summary>
    internal static double VehicleSpeed { get; set; } = 60;

    /// <summary>
    /// Average motorcycle delivery speed (in km/h).
    /// </summary>
    internal static double MotorcycleSpeed { get; set; } = 45;

    /// <summary>
    /// Average walking delivery speed (in km/h).
    /// </summary>
    internal static double WalkingSpeed { get; set; } = 5;

    /// <summary>
    /// Maximum allowed time window for delivery completion.
    /// </summary>
    internal static TimeSpan DeliveryWindow { get; set; } = TimeSpan.FromHours(2);

    /// <summary>
    /// Risk time range (for cases such as weather delays, traffic, etc.).
    /// </summary>
    internal static TimeSpan RiskRange { get; set; } = TimeSpan.FromHours(1.5);

    /// <summary>
    /// Idle time range for couriers between deliveries.
    /// </summary>
    internal static TimeSpan IdleTimeRange { get; set; } = TimeSpan.FromHours(3);

    /// <summary>
    /// Resets all configuration data to its initial state:
    /// resets running IDs and system clock to the current time.
    /// </summary>
    internal static void Reset()
    {
        nextOrderId = startOrderId;
        nextDeliveryId = startDeliveryId;
        Clock = DateTime.Now;
    }
}
