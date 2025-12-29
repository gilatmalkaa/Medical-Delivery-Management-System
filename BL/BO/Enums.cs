namespace BO;

public enum CourierType
{
    Foot,
    Bicycle,
    Motorcycle,
    Car
}
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
    All,
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
    Late,
    Scheduled
}
