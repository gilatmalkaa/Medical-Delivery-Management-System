using Helpers;

namespace BO;

/// <summary>
/// Represents a summarized view of an open order,
/// intended for display in order selection lists.
/// Includes scheduling, distance, and timing information.
/// </summary>
public class OpenOrderInList
{
    /// <summary>
    /// Identifier of the courier assigned to the order,
    /// or null if the order has not yet been assigned.
    /// </summary>
    public int? CourierId { get; init; }

    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Service level of the order.
    /// </summary>
    public OrderType Type { get; init; }

    /// <summary>
    /// Category or description of the ordered item.
    /// </summary>
    public string? ItemCategory { get; init; }

    /// <summary>
    /// Delivery destination address.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Estimated air distance between source and destination.
    /// </summary>
    public double AirDistance { get; init; }

    /// <summary>
    /// Actual distance traveled during delivery,
    /// if the delivery has already started.
    /// </summary>
    public double? ActualDistance { get; init; }

    /// <summary>
    /// Actual delivery time,
    /// if the delivery has already started or completed.
    /// </summary>
    public TimeSpan? ActualTime { get; init; }

    /// <summary>
    /// Current schedule status of the order
    /// (e.g., on time, delayed, scheduled).
    /// </summary>
    public ScheduleStatus ScheduleStatus { get; init; }

    /// <summary>
    /// Remaining time until the expected delivery deadline.
    /// </summary>
    public TimeSpan RemainingTime { get; init; }

    /// <summary>
    /// Expected or calculated delivery end time.
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Returns a string representation of the open order
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
