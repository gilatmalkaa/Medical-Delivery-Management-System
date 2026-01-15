namespace BO;

/// <summary>
/// Defines the type of vehicle used by a courier
/// to perform deliveries.
/// </summary>
public enum CourierType
{
    /// <summary>Courier delivers on foot.</summary>
    Foot,

    /// <summary>Courier delivers using a bicycle.</summary>
    Bicycle,

    /// <summary>Courier delivers using a motorcycle.</summary>
    Motorcycle,

    /// <summary>Courier delivers using a car.</summary>
    Car,

    /// <summary>Courier vehicle type is unknown or not specified.</summary>
    Unknown
}

/// <summary>
/// Defines the transportation method actually used
/// for executing a delivery.
/// </summary>
public enum DeliveryType
{
    /// <summary>Delivery performed on foot.</summary>
    Foot,

    /// <summary>Delivery performed using a bicycle.</summary>
    Bicycle,

    /// <summary>Delivery performed using a motorcycle.</summary>
    Motorcycle,

    /// <summary>Delivery performed using a car.</summary>
    Car
}

/// <summary>
/// Represents the lifecycle status of a delivery.
/// </summary>
public enum DeliveryStatus
{
    /// <summary>Delivery is currently in progress.</summary>
    InProgress,

    /// <summary>Delivery was completed successfully.</summary>
    Delivered,

    /// <summary>Delivery failed due to an error or issue.</summary>
    Failed,

    /// <summary>Delivery was canceled before completion.</summary>
    Canceled
}

/// <summary>
/// Defines the service level of an order,
/// affecting priority and handling rules.
/// </summary>
public enum OrderType
{
    /// <summary>Standard delivery service.</summary>
    Regular,

    /// <summary>High-priority delivery service.</summary>
    Express,

    /// <summary>Premium delivery service with top priority.</summary>
    Prime
}

/// <summary>
/// Represents the logical business state of an order
/// throughout its lifecycle.
/// </summary>
public enum OrderStatus
{
    /// <summary>Represents all order states (used mainly for filtering).</summary>
    All,

    /// <summary>Order has been created but not yet assigned.</summary>
    Created,

    /// <summary>Order has been assigned to a courier.</summary>
    Assigned,

    /// <summary>Order is currently being delivered.</summary>
    InDelivery,

    /// <summary>Order delivery was completed successfully.</summary>
    Delivered,

    /// <summary>Order delivery failed.</summary>
    Failed
}

/// <summary>
/// Indicates the delivery timing status
/// relative to the planned schedule.
/// </summary>
public enum ScheduleStatus
{
    /// <summary>Delivery is on time.</summary>
    OnTime,

    /// <summary>Delivery has a minor delay.</summary>
    SlightDelay,

    /// <summary>Delivery is significantly delayed.</summary>
    Late,

    /// <summary>Delivery is scheduled but not yet started.</summary>
    Scheduled
}

/// <summary>
/// Defines system user roles used
/// for authorization and access control.
/// </summary>
public enum UserRole
{
    /// <summary>System administrator role.</summary>
    Admin,

    /// <summary>Courier role with operational permissions.</summary>
    Courier
}
