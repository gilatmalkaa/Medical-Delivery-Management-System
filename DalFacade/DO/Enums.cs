namespace DO
{

    public enum OrderType
    {
        Regular,
        Express,
        SameDay
    }


    public enum DeliveryType
    {
        Foot,
        Bicycle,
        Car
    }


    public enum DeliveryStatus
    {
        Pending,       
        InProgress,     
        Delivered,      
        Canceled        
    }
}
