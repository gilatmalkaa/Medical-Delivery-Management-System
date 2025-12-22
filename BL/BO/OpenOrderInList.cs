using Helpers;

namespace BO;

public class OpenOrderInList
{
    public int? CourierId { get; init; }
    public int OrderId { get; init; }

    public OrderType Type { get; init; }
    public string? ItemCategory { get; init; }
    public string? Address { get; init; }

    public double AirDistance { get; init; }
    public double? ActualDistance { get; init; }

    public TimeSpan? ActualTime { get; init; }
    public ScheduleStatus ScheduleStatus { get; init; }

    public TimeSpan RemainingTime { get; init; }
    public DateTime EndTime { get; init; }

    public override string ToString() => this.ToStringProperty();
}
