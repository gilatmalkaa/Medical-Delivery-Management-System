using BL;

namespace BO;
public class OrderInList
{
    public int? DeliveryId { get; init; }
    public int OrderId { get; init; }
    public OrderType Type { get; set; }
    public double AirDistance { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ScheduleStatus ScheduleStatus { get; set; }
    public TimeSpan? TimeRemaining { get; set; }
    public TimeSpan? HandlingTime { get; set; }
    public int DeliveriesCount { get; set; }
    public override string ToString() => this.ToStringProperty();
}
