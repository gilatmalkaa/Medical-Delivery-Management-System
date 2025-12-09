namespace BO;

public enum DeliveryType
{
    Foot,
    Bicycle,
    Motorcycle,
    Car
}


public enum DeliveryStatus
{
    InProgress,  
    Delivered,    
    Failed,     
    Canceled      
}

public enum OrderType
{
    Regular,
    Express,
    Prime
}

public enum OrderStatus
{
    Created,    
    Assigned,    
    InDelivery, 
    Delivered,    
    Failed      
}

public enum ScheduleStatus
{
    OnTime,
    SlightDelay,
    Late
}



