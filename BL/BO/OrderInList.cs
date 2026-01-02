using Helpers;

namespace BO;

/// <summary>
/// Lightweight representation of order used in list screens.
/// Provides summary timing and status information.
/// </summary>
public class OrderInList
{
    public int? DeliveryId { get; init; }
    public int OrderId { get; init; }

    public OrderType Type { get; init; }
    public double AirDistance { get; init; }

    public OrderStatus OrderStatus { get; init; }
    public ScheduleStatus ScheduleStatus { get; init; }

    public TimeSpan? TimeRemaining { get; init; }
    public TimeSpan? HandlingTime { get; init; }

    public int DeliveriesCount { get; init; }

    public override string ToString() => this.ToStringProperty();
}
