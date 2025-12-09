using BL;

namespace BO;
public class CourierInList
{
    public int Id { get; set; }              
    public string FullName { get; set; } = "";   
    public bool IsActive { get; set; }         
    public DeliveryType Type { get; set; }      
    public DateTime StartWorkDate { get; set; } 
    public int TotalDeliveriesOnTime { get; set; }      
    public int TotalDeliveriesLate { get; set; }   
    public int? CurrentDeliveryId { get; set; }
    public override string ToString() => this.ToStringProperty();
}
