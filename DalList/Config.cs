namespace DalList;

/// <summary>
/// Static configuration class used to simulate system constants and running IDs.
/// Defined as internal so it is accessible only within the DalList project.
/// </summary>
internal static class Config
{
    internal const int startOrderId = 1000;
    private static int nextOrderId = startOrderId;
    internal static int NextOrderId { get => nextOrderId++; }

    internal const int startDeliveryId = 2000;
    private static int nextDeliveryId = startDeliveryId;
    internal static int NextDeliveryId { get => nextDeliveryId++; }

    internal static DateTime Clock { get; set; } = DateTime.Now;
    internal static int ManagerId { get; set; } = 1;
    internal static string ManagerSignature { get; set; } = "SignedByManager";

    internal static string? CompanyAddress { get; set; } = "Herzl St 10, Jerusalem";
    internal static double? Latitude { get; set; } = 31.7683;  
    internal static double? Longitude { get; set; } = 35.2137; 
    internal static double AirDistance { get; set; } = 10.5;  
    internal static double VehicleSpeed { get; set; } = 60; 
    internal static double MotorcycleSpeed { get; set; } = 45; 
    internal static double WalkingSpeed { get; set; } = 5;   

    internal static TimeSpan DeliveryWindow { get; set; } = TimeSpan.FromHours(2); 
    internal static TimeSpan RiskRange { get; set; } = TimeSpan.FromHours(1.5);   
    internal static TimeSpan IdleTimeRange { get; set; } = TimeSpan.FromHours(3);

    internal static void Reset()
    {
        nextOrderId = startOrderId;
        nextDeliveryId = startDeliveryId;
        Clock = DateTime.Now;
    }
}
