namespace Dal;

/// <summary>
/// Static configuration class for managing XML data files and auto-increment IDs for DAL entities.
/// Provides access to configuration values and methods to reset them.
/// </summary>
internal static class Config
{
    /// <summary>
    /// Administrator identifier used for system-level authentication.
    /// Stored in the main configuration XML file.
    /// </summary>
    internal static string AdminId
    {
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "AdminId");
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "AdminId", value);
    }

    /// <summary>
    /// Administrator password used for system-level authentication.
    /// Stored securely in the main configuration XML file.
    /// </summary>
    internal static string AdminPassword
    {
        get => XMLTools.GetConfigStringVal(s_data_config_xml, "AdminPassword");
        set => XMLTools.SetConfigStringVal(s_data_config_xml, "AdminPassword", value);
    }

    /// <summary>
    /// File name of the main configuration XML file.
    /// </summary>
    internal const string s_data_config_xml = "data-config.xml";

    /// <summary>
    /// File name of the couriers XML data file.
    /// </summary>
    internal const string s_couriers_xml = "couriers.xml";

    /// <summary>
    /// File name of the deliveries XML data file.
    /// </summary>
    internal const string s_deliveries_xml = "deliveries.xml";

    /// <summary>
    /// File name of the orders XML data file.
    /// </summary>
    internal const string s_orders_xml = "orders.xml";


    /// <summary>
    /// Gets the next available Order ID and increments it in the configuration file.
    /// </summary>
    internal static int NextOrderId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextOrderId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextOrderId", value);
    }

    /// <summary>
    /// Gets the next available Courier ID and increments it in the configuration file.
    /// </summary>
    internal static int NextCourierId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextCourierId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextCourierId", value);
    }

    /// <summary>
    /// Gets the next available Delivery ID and increments it in the configuration file.
    /// </summary>
    internal static int NextDeliveryId
    {
        get => XMLTools.GetAndIncreaseConfigIntVal(s_data_config_xml, "NextDeliveryId");
        private set => XMLTools.SetConfigIntVal(s_data_config_xml, "NextDeliveryId", value);
    }

    /// <summary>
    /// Gets or sets the maximum allowed delivery range (in kilometers)
    /// that a courier can be assigned for an order.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static int MaxRange
    {
        get => XMLTools.GetConfigIntVal(s_data_config_xml, "MaxRange");
        set => XMLTools.SetConfigIntVal(s_data_config_xml, "MaxRange", value);
    }

    /// <summary>
    /// Gets or sets the maximum delivery duration (in minutes)
    /// before a delivery is considered late.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static int MaxDeliveryDurationMinutes
    {
        get => XMLTools.GetConfigIntVal(s_data_config_xml, "MaxDeliveryDurationMinutes");
        set => XMLTools.SetConfigIntVal(s_data_config_xml, "MaxDeliveryDurationMinutes", value);
    }

    /// <summary>
    /// Gets or sets the expiration time (in minutes) for time-sensitive samples.
    /// After this duration, a sample is considered expired.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static int SampleExpirationMinutes
    {
        get => XMLTools.GetConfigIntVal(s_data_config_xml, "SampleExpirationMinutes");
        set => XMLTools.SetConfigIntVal(s_data_config_xml, "SampleExpirationMinutes", value);
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering on foot.
    /// Used for delivery time estimation.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double FootSpeed
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "FootSpeed");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "FootSpeed", value);
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by bicycle.
    /// Used for delivery time estimation.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double BikeSpeed
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "BikeSpeed");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "BikeSpeed", value);
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by motorcycle.
    /// Used for delivery time estimation.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double MotorcycleSpeed
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "MotorcycleSpeed");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "MotorcycleSpeed", value);
    }

    /// <summary>
    /// Gets or sets the average courier speed (km/h) when delivering by car.
    /// Used for delivery time estimation.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double CarSpeed
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "CarSpeed");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "CarSpeed", value);
    }

    /// <summary>
    /// Gets or sets the price charged per kilometer for a delivery.
    /// Used to calculate the total delivery cost.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double PricePerKm
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "PricePerKm");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "PricePerKm", value);
    }

    /// <summary>
    /// Gets or sets the base delivery price added to every order,
    /// regardless of distance.
    /// The value is stored and retrieved from the XML configuration file.
    /// </summary>
    internal static double BaseDeliveryPrice
    {
        get => XMLTools.GetConfigDoubleVal(s_data_config_xml, "BaseDeliveryPrice");
        set => XMLTools.SetConfigDoubleVal(s_data_config_xml, "BaseDeliveryPrice", value);
    }



    /// <summary>
    /// Gets or sets the simulated clock for the system.
    /// Stored in the configuration XML file.
    /// </summary>
    internal static DateTime Clock
    {
        get => XMLTools.GetConfigDateVal(s_data_config_xml, "Clock");
        set => XMLTools.SetConfigDateVal(s_data_config_xml, "Clock", value);
    }

    /// <summary>
    /// Resets all configuration values to default initial values.
    /// Next IDs are reset to 1000 and the Clock is set to the current system time.
    /// </summary>
    internal static void Reset()
    {
        NextOrderId = 1000;
        NextCourierId = 1000;
        NextDeliveryId = 1000;
        Clock = DateTime.Now;
    }
}
