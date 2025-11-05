namespace DO
{
    /// <summary>
    /// Enumerates available order types based on delivery urgency.
    /// </summary>
    public enum OrderType
    {
        /// <summary>Standard non-urgent delivery.</summary>
        Regular,

        /// <summary>Express (fast) delivery.</summary>
        Express,

        /// <summary>Same-day delivery service.</summary>
        SameDay
    }

    /// <summary>
    /// Enumerates available courier vehicle or delivery types.
    /// </summary>
    public enum DeliveryType
    {
        /// <summary>Delivery made on foot.</summary>
        Foot,

        /// <summary>Delivery made using a bicycle.</summary>
        Bicycle,

        /// <summary>Delivery made using a car or motor vehicle.</summary>
        Car
    }

    /// <summary>
    /// Enumerates the possible statuses of a delivery process.
    /// </summary>
    public enum DeliveryStatus
    {
        /// <summary>Delivery is scheduled but not yet started.</summary>
        Pending,

        /// <summary>Delivery is currently in progress.</summary>
        InProgress,

        /// <summary>Delivery has been completed successfully.</summary>
        Delivered,

        /// <summary>Delivery was canceled or failed.</summary>
        Canceled
    }
}
