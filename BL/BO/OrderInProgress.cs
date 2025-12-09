using BL;
using DO;

namespace BO;

/// <summary>
/// הזמנה בטיפול שליח – תצוגה לוגית עבור מסך "הזמנה בטיפול שליח".
/// </summary>
public class OrderInProgress
{
    public int DeliveryId { get; set; }   
    public int OrderId { get; set; } 
    public OrderType Type { get; set; }  
    public string Description { get; set; } = ""; 
    public string Address { get; set; } = ""; 
    public double AirDistance { get; set; }   
    public double? ActualDistance { get; set; }    
    public string CustomerName { get; set; } = ""; 
    public string CustomerPhone { get; set; } = ""; 
    public DateTime OpenDate { get; set; }
    public DateTime StartDeliveryDate { get; set; } 
    public DateTime ExpectedArrivalTime { get; set; }
    public DateTime LatestSupplyTime { get; set; }
    public OrderStatus OrderStatus { get; set; } // סטטוס הזמנה
    public ScheduleStatus ScheduleStatus { get; set; } // סטטוס עמידה בזמנים
    public TimeSpan RemainingTimeToFinishOrder { get; set; }
    public override string ToString() => this.ToStringProperty();
}
