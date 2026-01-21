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
        /// Represents the current system clock.
        /// Used as the reference time for delivery scheduling,
        /// delay calculations, and simulation logic.
        /// </summary>
        internal static DateTime Clock = DateTime.Now;

        /// <summary>
        /// Defines the maximum allowed delivery range (in kilometers)
        /// that a courier can be assigned for an order.
        /// </summary>
        internal static int MaxRange = 100;

        /// <summary>
        /// Defines the maximum time (in minutes) before a medical
        /// or time-sensitive sample is considered expired.
        /// </summary>
        internal static int SampleExpirationMinutes = 60;

        /// <summary>
        /// Defines the maximum allowed delivery duration (in minutes)
        /// before a delivery is considered late.
        /// </summary>
        internal static int MaxDeliveryDurationMinutes = 120;

        /// <summary>
        /// Average delivery speed (km/h) for couriers traveling on foot.
        /// Used for delivery time estimation.
        /// </summary>
        internal static double FootSpeed = 4.0;

        /// <summary>
        /// Average delivery speed (km/h) for couriers using bicycles.
        /// Used for delivery time estimation.
        /// </summary>
        internal static double BikeSpeed = 15.0;

        /// <summary>
        /// Average delivery speed (km/h) for couriers using motorcycles.
        /// Used for delivery time estimation.
        /// </summary>
        internal static double MotorcycleSpeed = 40.0;

        /// <summary>
        /// Average delivery speed (km/h) for couriers using cars.
        /// Used for delivery time estimation.
        /// </summary>
        internal static double CarSpeed = 50.0;

        /// <summary>
        /// Base price added to every delivery,
        /// regardless of distance.
        /// </summary>
        internal static double BaseDeliveryPrice = 20.0;

        /// <summary>
        /// Price charged per kilometer for delivery distance.
        /// Used to calculate the total delivery cost.
        /// </summary>
        internal static double PricePerKm = 5.0;

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
