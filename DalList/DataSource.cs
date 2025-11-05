namespace Dal;
using DO;

internal static class DataSource
{
    internal static List<Courier> Couriers { get; } = new();
    internal static List<Order> Orders { get; } = new();
    internal static List<Delivery> Deliveries { get; } = new();

    internal static class Config
    {
        internal static int NextOrderId = 1000;
        internal static int NextCourierId = 2000;
        internal static int NextDeliveryId = 3000;

        internal static void Reset()
        {
            NextOrderId = 1000;
            NextCourierId = 2000;
            NextDeliveryId = 3000;
        }
    }
}