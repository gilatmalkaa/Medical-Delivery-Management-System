using BL;

namespace BO;

/// <summary>
/// Logic-layer representation of a courier, enriched with computed
/// properties and DTO references required for presentation.
/// </summary>
public class Courier
{
    public int Id { get; set; }                     
    public string Name { get; set; } = "";              
    public string Phone { get; set; } = "";             
    public string Email { get; set; } = "";            
    public string Signature { get; set; } = "";         
    public bool IsActive { get; set; }                
    public double? MaxPersonalDeliveryDistance { get; set; }  
    public DeliveryType Type { get; set; }              
    public DateTime StartWorkDate { get; set; }        
    public int TotalDeliveries { get; set; }           
    public int TimeInWork { get; set; }               
    public OrderInProgress? CurrentOrder { get; set; }
    public bool IsAvailable { get; set; }
    public override string ToString() => this.ToStringProperty();
}
