using Helpers;

namespace BO;

public class Order
{
    public int Id { get; init; }
    public OrderType Type { get; init; }

    public string? Description { get; set; }
    public string? Address { get; set; }

    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double AirDistance { get; init; }

    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? PackageDetails { get; set; }

    public DateTime CreatedAt { get; init; }
    public DateTime? ExpectedDeliveryTime { get; set; }
    public DateTime? MaxDeliveryTime { get; set; }

    public OrderStatus OrderStatus { get; set; }
    public ScheduleStatus ScheduleStatus { get; set; }
    public TimeSpan? TimeRemaining { get; set; }

    public List<DeliveryPerOrderInList>? Deliveries { get; init; }

    public override string ToString() => this.ToStringProperty();
}
