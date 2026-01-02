namespace BO;

/// <summary>Courier vehicle type.</summary>
public enum CourierType { Foot, Bicycle, Motorcycle, Car }

/// <summary>Delivery transportation type.</summary>
public enum DeliveryType { Foot, Bicycle, Motorcycle, Car }

/// <summary>Status of a delivery process.</summary>
public enum DeliveryStatus { InProgress, Delivered, Failed, Canceled }

/// <summary>Order service level.</summary>
public enum OrderType { Regular, Express, Prime }

/// <summary>Logical business order state.</summary>
public enum OrderStatus { All, Created, Assigned, InDelivery, Delivered, Failed }

/// <summary>Delivery schedule condition.</summary>
public enum ScheduleStatus { OnTime, SlightDelay, Late, Scheduled }
