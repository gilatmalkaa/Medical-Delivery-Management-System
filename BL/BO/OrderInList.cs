using Helpers;

namespace BO;

/// <summary>
/// Lightweight representation of an order used in list and table views.
/// Provides summarized timing, status, and control information.
/// </summary>
public class OrderInList
{
    /// <summary>
    /// Identifier of the related delivery,
    /// or null if no delivery has been created yet.
    /// </summary>
    public int? DeliveryId { get; init; }

    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Service level of the order.
    /// </summary>
    public OrderType Type { get; init; }

    /// <summary>
    /// Calculated air distance between source and destination.
    /// </summary>
    public double AirDistance { get; init; }

    /// <summary>
    /// Current logical status of the order.
    /// </summary>
    public OrderStatus OrderStatus { get; init; }

    /// <summary>
    /// Current scheduling status of the order
    /// relative to expected delivery times.
    /// </summary>
    public ScheduleStatus ScheduleStatus { get; init; }

    /// <summary>
    /// Remaining time until the delivery deadline,
    /// if applicable.
    /// </summary>
    public TimeSpan? TimeRemaining { get; init; }

    /// <summary>
    /// Total handling time of the order,
    /// from creation to completion.
    /// </summary>
    public TimeSpan? HandlingTime { get; init; }

    /// <summary>
    /// Total number of deliveries associated with the order.
    /// </summary>
    public int DeliveriesCount { get; init; }

    /// <summary>
    /// Indicates whether the order can still be canceled
    /// according to business rules.
    /// </summary>
    public bool CanCancel { get; init; }

    /// <summary>
    /// Indicates whether the order is currently active
    /// (created, assigned, or in delivery).
    /// </summary>
    public bool IsActive =>
        OrderStatus is OrderStatus.Created
                  or OrderStatus.Assigned
                  or OrderStatus.InDelivery;

    /// <summary>
    /// Returns a string representation of the order summary
    /// using reflection-based property formatting.
    /// </summary>
    public override string ToString() => this.ToStringProperty();
}
