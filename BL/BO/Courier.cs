using Helpers;

namespace BO;

public class Courier
{
    public int Id { get; init; }

    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Signature { get; set; }

    public bool IsActive { get; set; }
    public double? MaxPersonalDeliveryDistance { get; set; }

    public DeliveryType Type { get; init; }
    public DateTime StartWorkDate { get; init; }

    public int TotalDeliveries { get; set; }
    public int TimeInWork { get; set; }

    public bool IsAvailable { get; set; }
    public OrderInProgress? CurrentOrder { get; set; }

    public override string ToString() => this.ToStringProperty();
}
