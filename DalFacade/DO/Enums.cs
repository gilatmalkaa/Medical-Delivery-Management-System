namespace DO
{
    /// <summary>
    /// Defines the different types of orders according to delivery urgency.
    /// </summary>
    public enum OrderType
    {
        Regular,
        Express,
        SameDay
    }

    /// <summary>
    /// Defines the available courier transportation methods.
    /// </summary>
    public enum CourierType
    {
        Foot,
        Bicycle,
        Motorcycle,
        Car
    }

    /// <summary>
    /// Defines the possible states of a delivery lifecycle.
    /// </summary>
    public enum DeliveryStatus
    {
        Pending,
        InProgress,
        Delivered,
        Canceled
    }

    /// <summary>
    /// Defines the possible states of an order throughout its lifecycle.
    /// </summary>
    public enum OrderStatus
    {
        All,
        Created,
        Assigned,
        InDelivery,
        Delivered,
        Failed
    }
}
