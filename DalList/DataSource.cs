namespace Dal;
using DO;

/// <summary>
/// Represents the in-memory database used by the DAL layer.
/// Holds all lists of data entities (Couriers, Orders, Deliveries) 
/// and configuration values used to generate running IDs.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// List containing all courier entities in the data source.
    /// </summary>
    internal static List<Courier> Couriers { get; } = new();

    /// <summary>
    /// List containing all order entities in the data source.
    /// </summary>
    internal static List<Order> Orders { get; } = new();

    /// <summary>
    /// List containing all delivery entities in the data source.
    /// </summary>
    internal static List<Delivery> Deliveries { get; } = new();

    /// <summary>
    /// Nested configuration class responsible for maintaining 
    /// running (auto-increment) IDs for entities stored in the data source.
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// The next available running ID for orders.
        /// </summary>
        internal static int NextOrderId = 1000;

        /// <summary>
        /// The next available running ID for couriers.
        /// </summary>
        internal static int NextCourierId = 2000;

        /// <summary>
        /// The next available running ID for deliveries.
        /// </summary>
        internal static int NextDeliveryId = 3000;

        /// <summary>
        /// Resets all running IDs back to their initial starting values.
        /// </summary>
        internal static void Reset()
        {
            NextOrderId = 1000;
            NextCourierId = 2000;
            NextDeliveryId = 3000;
        }
    }
}
