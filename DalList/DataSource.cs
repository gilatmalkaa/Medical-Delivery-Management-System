namespace Dal;
using DO;

internal static class DataSource
{
    internal static List<Courier> Couriers { get; } = new();
    internal static List<Order> Orders { get; } = new();
    internal static List<Delivery> Deliveries { get; } = new();
}