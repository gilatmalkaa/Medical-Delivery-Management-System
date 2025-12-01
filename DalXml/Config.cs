namespace Dal;

/// <summary>
/// Static configuration class for managing XML data files and auto-increment IDs for DAL entities.
/// Provides access to configuration values and methods to reset them.
/// </summary>
internal static class Config
{
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
