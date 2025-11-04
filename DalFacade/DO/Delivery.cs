namespace DO;

/// <summary>
/// Delivery entity represents the link between an order and the courier assigned to handle it.
/// Each delivery has a unique ID and connects an order ID to a courier ID.
/// </summary>
/// <param name="Id">Unique ID of the delivery (running number, generated automatically).</param>
/// <param name="OrderId">ID of the order associated with this delivery.</param>
/// <param name="CourierId">ID of the courier assigned to this delivery.</param>
/// <param name="Type">Delivery type (standard, express, etc.).</param>
/// <param name="StartDeliveryDate">Date and time when the delivery started.</param>
/// <param name="ActualDistance">Actual distance traveled for this delivery (in km).</param>
/// <param name="ExpectedDistance">Calculated expected distance (in km) based on system’s logic.</param>
/// <param name="CompletionStatus">Delivery completion status (e.g., Delivered, Failed).</param>
/// <param name="EndDeliveryDate">Date and time when the delivery ended (if completed).</param>
public record Delivery(
    int Id,
    int OrderId, 
    int CourierId,
    DeliveryType Type,
    DateTime StartDeliveryDate,
    double ActualDistance,
    double? ExpectedDistance,
    DeliveryStatus? CompletionStatus,
    DateTime? EndDeliveryDate
)
{
    public Delivery() : this(0, 0, 0, DeliveryType.Foot, default, null, null, DeliveryStatus.InProgress, null) { }
}