using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

/// <summary>
/// Provides general utility functions for object formatting,
/// geographic calculations, time computations, scheduling logic,
/// and mapping between DO and BO entities.
/// </summary>
internal static class Tools
{
    /// <summary>
    /// Builds a readable string representation of an object's public properties,
    /// including nested collections, using reflection.
    /// </summary>
    public static string ToStringProperty<T>(this T obj)
    {
        if (obj == null)
            return string.Empty;

        StringBuilder sb = new();
        Type type = obj.GetType();

        sb.AppendLine(type.Name + ":");

        foreach (PropertyInfo prop in type.GetProperties())
        {
            object? value = prop.GetValue(obj);
            sb.Append($"  {prop.Name}: ");

            if (value == null)
                sb.AppendLine("null");
            else if (value is string)
                sb.AppendLine(value.ToString());
            else if (value is IEnumerable enumerable)
            {
                sb.AppendLine();
                foreach (var item in enumerable)
                    sb.AppendLine($"    - {item?.ToStringProperty()}");
            }
            else
                sb.AppendLine(value.ToString());
        }

        return sb.ToString();
    }

    /// <summary>
    /// Generates deterministic pseudo-coordinates for a given address string.
    /// </summary>
    internal static (double Latitude, double Longitude) GetCoordinates(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return (0, 0);

        int hash = address.GetHashCode();

        return (
            31.0 + (hash % 1000) * 0.0001,
            35.0 + (hash % 1000) * 0.0001
        );
    }

    /// <summary>
    /// Asynchronously generates deterministic pseudo-coordinates
    /// for a given address string.
    /// </summary>
    internal static Task<(double Latitude, double Longitude)> GetCoordinatesAsync(
        string address)
    {
        return Task.Run(() => GetCoordinates(address));
    }

    /// <summary>
    /// Calculates the aerial distance between two geographic coordinates
    /// using the Haversine formula.
    /// </summary>
    internal static double CalcAirDistance(
        double lat1,
        double lon1,
        double lat2,
        double lon2)
    {
        double R = 6371;

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegreesToRadians(lat1)) *
            Math.Cos(DegreesToRadians(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    /// <summary>
    /// Converts an angle value from degrees to radians.
    /// </summary>
    private static double DegreesToRadians(double deg) =>
        deg * Math.PI / 180;

    /// <summary>
    /// Calculates the expected delivery completion time based on
    /// distance and courier speed.
    /// </summary>
    internal static DateTime CalcExpectedDeliveryTime(
        DateTime startTime,
        double airDistance,
        double speedKmPerHour)
    {
        double hours = airDistance / speedKmPerHour;
        return startTime.AddHours(hours);
    }

    /// <summary>
    /// Calculates the maximum allowed delivery time according
    /// to the order type.
    /// </summary>
    internal static DateTime CalcMaxDeliveryTime(
        DateTime expected,
        BO.OrderType type)
    {
        TimeSpan slack = type switch
        {
            BO.OrderType.Regular => TimeSpan.FromMinutes(30),
            BO.OrderType.Express => TimeSpan.FromMinutes(15),
            BO.OrderType.Prime => TimeSpan.FromHours(1),
            _ => TimeSpan.Zero
        };

        return expected + slack;
    }

    /// <summary>
    /// Calculates the remaining time until the maximum delivery deadline.
    /// </summary>
    internal static TimeSpan CalcTimeRemaining(DateTime maxDeliveryTime)
    {
        TimeSpan remaining = maxDeliveryTime - DateTime.Now;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    /// <summary>
    /// Determines the scheduling status of an order based on
    /// its delivery status and timing constraints.
    /// </summary>
    internal static BO.ScheduleStatus CalcScheduleStatus(
        BO.OrderStatus orderStatus,
        DateTime? maxDeliveryTime,
        DateTime? deliveredAt)
    {
        if (maxDeliveryTime == null)
            return BO.ScheduleStatus.Scheduled;

        if (orderStatus == BO.OrderStatus.Delivered)
        {
            if (deliveredAt == null)
                return BO.ScheduleStatus.Late;

            TimeSpan diff = deliveredAt.Value - maxDeliveryTime.Value;

            if (diff <= TimeSpan.Zero)
                return BO.ScheduleStatus.OnTime;

            if (diff <= TimeSpan.FromMinutes(10))
                return BO.ScheduleStatus.SlightDelay;

            return BO.ScheduleStatus.Late;
        }

        return DateTime.Now <= maxDeliveryTime
            ? BO.ScheduleStatus.Scheduled
            : BO.ScheduleStatus.Late;
    }

    /// <summary>
    /// Cache for storing resolved geographic coordinates per address.
    /// Prevents repeated calls to the external geocoding service
    /// for the same address and improves performance.
    /// </summary>
    private static readonly ConcurrentDictionary<string, (double Lat, double Lon)>
        _coordinatesCache = new();

    /// <summary>
    /// Retrieves geographic coordinates (latitude, longitude) for a given address,
    /// using an in-memory cache to avoid redundant geocoding requests.
    /// </summary>
    /// <param name="address">
    /// The textual address to resolve into coordinates.
    /// </param>
    /// <returns>
    /// A tuple containing (Latitude, Longitude).
    /// Returns (0, 0) if the address is invalid or resolution fails.
    /// </returns>
    public static async Task<(double Latitude, double Longitude)>
        GetCoordinatesCachedAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return (0, 0);

        if (_coordinatesCache.TryGetValue(address, out var cached))
            return cached;

        try
        {
            var coords = await GetCoordinatesAsync(address);

            _coordinatesCache[address] = coords;
            return coords;
        }
        catch
        {
            return (0, 0);
        }
    }

    /// <summary>
    /// Courier average speed in km/h (single source of truth)
    /// </summary>
    internal const double CourierSpeedKmPerHour = 40.0;

    /// <summary>
    /// Calculates delivery duration based on distance and courier speed.
    /// </summary>
    internal static TimeSpan CalcDeliveryDuration(double distanceKm)
    {
        if (distanceKm <= 0)
            return TimeSpan.Zero;

        return TimeSpan.FromHours(distanceKm / CourierSpeedKmPerHour);
    }

    /// <summary>
    /// Maps a DO.Courier entity to its corresponding BO.Courier representation.
    /// </summary>
    public static BO.Courier ToBO(this DO.Courier c) =>
        new BO.Courier
        {
            Id = c.Id,
            Name = c.FullName,
            Password = c.Password,
            Phone = c.Phone,
            Email = c.Email,
            Signature = c.Signature,
            MaxPersonalDeliveryDistance = c.MaxPersonalDeliveryDistance,
            Type = (BO.DeliveryType)c.Type,
            IsActive = c.IsActive,
            StartWorkDate = c.StartWorkDate ?? DateTime.Now
        };

    /// <summary>
    /// Maps a BO.Courier entity to its corresponding DO.Courier representation.
    /// </summary>
    public static DO.Courier ToDO(this BO.Courier c) =>
        new DO.Courier(
            c.Id,
            c.Name ?? "",
            c.Phone ?? "",
            c.Email ?? "",
            c.Signature ?? "",
            c.Password ?? "",
            c.MaxPersonalDeliveryDistance ?? 0,
            (DO.CourierType)c.Type,
            c.IsActive,
            c.StartWorkDate
        );
}
