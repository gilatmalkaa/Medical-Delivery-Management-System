using BO;
using System.Runtime.CompilerServices;

namespace Helpers;

/// <summary>
/// Internal business logic manager responsible for
/// system configuration, logical clock handling,
/// authentication, and administrative operations.
/// </summary>
internal static class AdminManager
{
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    /// <summary>
    /// Gets the current logical system clock.
    /// </summary>
    internal static DateTime Now => s_dal.Config.Clock;

    /// <summary>
    /// Event raised when configuration values are updated.
    /// </summary>
    internal static event Action? ConfigUpdatedObservers;

    /// <summary>
    /// Event raised when the system clock is updated.
    /// </summary>
    internal static event Action? ClockUpdatedObservers;

    private static Task? _periodicTask = null;

    /// <summary>
    /// Updates the logical system clock
    /// and notifies registered observers.
    /// </summary>
    /// <param name="newClock">New clock value.</param>
    internal static void UpdateClock(DateTime newClock)
    {
        s_dal.Config.Clock = newClock;
        ClockUpdatedObservers?.Invoke();
    }

    /// <summary>
    /// Retrieves the current system configuration.
    /// </summary>
    /// <returns>Configuration snapshot.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static Config GetConfig()
        => new Config
        {
            AdminId = s_dal.Config.AdminId,
            AdminPassword = s_dal.Config.AdminPassword,
            Clock = s_dal.Config.Clock,
            MaxRange = s_dal.Config.MaxRange,
            SampleExpirationMinutes = s_dal.Config.SampleExpirationMinutes,
            MaxDeliveryDurationMinutes = s_dal.Config.MaxDeliveryDurationMinutes,
            FootSpeed = s_dal.Config.FootSpeed,
            BikeSpeed = s_dal.Config.BikeSpeed,
            MotorcycleSpeed = s_dal.Config.MotorcycleSpeed,
            CarSpeed = s_dal.Config.CarSpeed,
            BaseDeliveryPrice = s_dal.Config.BaseDeliveryPrice,
            PricePerKm = s_dal.Config.PricePerKm
        };

    /// <summary>
    /// Updates system configuration values
    /// and notifies observers if changes occurred.
    /// </summary>
    /// <param name="configuration">Updated configuration.</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static void SetConfig(Config configuration)
    {
        bool changed = false;

        if (s_dal.Config.Clock != configuration.Clock)
        {
            s_dal.Config.Clock = configuration.Clock;
            changed = true;
        }

        if (s_dal.Config.MaxRange != configuration.MaxRange)
        {
            s_dal.Config.MaxRange = configuration.MaxRange;
            changed = true;
        }

        if (s_dal.Config.SampleExpirationMinutes != configuration.SampleExpirationMinutes)
        {
            s_dal.Config.SampleExpirationMinutes = configuration.SampleExpirationMinutes;
            changed = true;
        }

        if (s_dal.Config.MaxDeliveryDurationMinutes != configuration.MaxDeliveryDurationMinutes)
        {
            s_dal.Config.MaxDeliveryDurationMinutes = configuration.MaxDeliveryDurationMinutes;
            changed = true;
        }

        if (s_dal.Config.FootSpeed != configuration.FootSpeed)
        {
            s_dal.Config.FootSpeed = configuration.FootSpeed;
            changed = true;
        }

        if (s_dal.Config.BikeSpeed != configuration.BikeSpeed)
        {
            s_dal.Config.BikeSpeed = configuration.BikeSpeed;
            changed = true;
        }

        if (s_dal.Config.MotorcycleSpeed != configuration.MotorcycleSpeed)
        {
            s_dal.Config.MotorcycleSpeed = configuration.MotorcycleSpeed;
            changed = true;
        }

        if (s_dal.Config.CarSpeed != configuration.CarSpeed)
        {
            s_dal.Config.CarSpeed = configuration.CarSpeed;
            changed = true;
        }

        if (s_dal.Config.BaseDeliveryPrice != configuration.BaseDeliveryPrice)
        {
            s_dal.Config.BaseDeliveryPrice = configuration.BaseDeliveryPrice;
            changed = true;
        }

        if (s_dal.Config.PricePerKm != configuration.PricePerKm)
        {
            s_dal.Config.PricePerKm = configuration.PricePerKm;
            changed = true;
        }

        if (changed)
            ConfigUpdatedObservers?.Invoke();
    }

    /// <summary>
    /// Resets the database and reinitializes
    /// system configuration and clock.
    /// </summary>
    internal static void ResetDB()
    {
        lock (BlMutex)
        {
            s_dal.ResetDB();
            UpdateClock(Now);
            SetConfig(GetConfig());
        }
    }

    /// <summary>
    /// Initializes the database and refreshes
    /// system configuration and clock.
    /// </summary>
    internal static void InitializeDB()
    {
        lock (BlMutex)
        {
            UpdateClock(Now);
            SetConfig(GetConfig());
        }
    }

    /// <summary>
    /// Calculates the number of orders
    /// grouped by their current status.
    /// </summary>
    /// <returns>Dictionary mapping order status to count.</returns>
    internal static IDictionary<OrderStatus, int> GetOrdersCountByStatus()
    {
        var orders = s_dal.Order.ReadAll();
        var deliveries = s_dal.Delivery.ReadAll();

        return orders
            .Select(order =>
            {
                var delivery = deliveries.FirstOrDefault(d => d.OrderId == order.Id);

                if (delivery == null || delivery.CompletionStatus == null)
                    return OrderStatus.Created;

                return delivery.CompletionStatus switch
                {
                    DO.DeliveryStatus.InProgress => OrderStatus.InDelivery,
                    DO.DeliveryStatus.Delivered => OrderStatus.Delivered,
                    _ => OrderStatus.Failed
                };
            })
            .GroupBy(status => status)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Synchronization object for BL critical sections.
    /// </summary>
    internal static readonly object BlMutex = new();

    /// <summary>
    /// Authenticates a user and returns their system role.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="password">User password.</param>
    /// <returns>User role.</returns>
    /// <exception cref="BlInvalidCredentialsException">
    /// Thrown when credentials are invalid.
    /// </exception>
    public static UserRole Login(string id, string password)
    {
        var config = GetConfig();

        if (id == config.AdminId && password == config.AdminPassword)
            return UserRole.Admin;

        var courier = s_dal.Courier
            .Read(c => c.Id.ToString() == id && c.Password == password);

        if (courier != null)
            return UserRole.Courier;

        throw new BlInvalidCredentialsException("Invalid ID or password");
    }
}
