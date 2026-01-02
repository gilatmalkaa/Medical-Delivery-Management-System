using Helpers;

namespace BO;

/// <summary>
/// Logical representation of an order currently handled by courier.
/// Used in courier active-delivery UI screens.
/// </summary>
public class OrderInProgress
{
    public int DeliveryId { get; init; }
    public int OrderId { get; init; }

    public OrderType Type { get; init; }
    public string? Description { get; set; }
    public string? Address { get; set; }

    public double AirDistance { get; init; }
    public double? ActualDistance { get; init; }

    public string? CustomerName { get; init; }
    public string? CustomerPhone { get; init; }

    public DateTime OpenDate { get; init; }
    public DateTime StartDeliveryDate { get; init; }
    public DateTime ExpectedArrivalTime { get; init; }
    public DateTime LatestSupplyTime { get; init; }

    public OrderStatus OrderStatus { get; init; }
    public ScheduleStatus ScheduleStatus { get; init; }

    public TimeSpan RemainingTimeToFinishOrder { get; init; }

    public override string ToString() => this.ToStringProperty();
}
