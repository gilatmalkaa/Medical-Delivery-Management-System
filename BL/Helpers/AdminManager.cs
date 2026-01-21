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

    /// <summary>
    /// Data access layer instance used by the order manager
    /// to perform CRUD operations on orders and deliveries.
    /// </summary>
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;
    /// <summary>  
    /// Mutex to use from BL methods to get mutual exclusion while the simulator is running 
    /// </summary> 
    internal static readonly object BlMutex = new();
    /// <summary> 
    /// The thread of the simulator 
    /// </summary> 
    private static volatile Thread? s_thread;
    /// <summary> 
    /// The Interval for clock updating 
    /// in minutes by second (default value is 1, will be set on Start())  
    /// </summary> 
    private static int s_interval = 1;
    /// <summary> 
    /// The flag that signs whether simulator is running 
    /// </summary> 
    private static volatile bool s_stop = false;

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
    internal static void UpdateClock(DateTime newClock, bool fromSimulator = false)
    {
        DateTime oldClock;

        lock (BlMutex)
        {
            if (!fromSimulator)
                ThrowOnSimulatorIsRunning();

            oldClock = s_dal.Config.Clock;
            s_dal.Config.Clock = newClock;
        }

        ClockUpdatedObservers?.Invoke();

        _ = Task.Run(() =>
        {
            OrderManager.PeriodicOrdersUpdates(oldClock, newClock);
            CourierManager.PeriodicCouriersUpdates(oldClock, newClock);
            DeliveryManager.PeriodicDeliveriesUpdates(oldClock, newClock);
        });
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
        ThrowOnSimulatorIsRunning();

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
        ThrowOnSimulatorIsRunning();
        lock (BlMutex)
        {
            s_dal.ResetDB();
            UpdateClock(DateTime.Now);
        }
    }


    /// <summary>
    /// Initializes the database and refreshes
    /// system configuration and clock.
    /// </summary>
    internal static void InitializeDB()
    {
        ThrowOnSimulatorIsRunning();
        lock (BlMutex)
        {
            UpdateClock(DateTime.Now);
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
                var delivery = deliveries
                    .Where(d => d.OrderId == order.Id)
                    .OrderBy(d => d.Id)
                    .LastOrDefault();

                if (delivery == null)
                    return OrderStatus.Created;

                return delivery.CompletionStatus switch
                {
                    DO.DeliveryStatus.Pending => OrderStatus.Assigned,
                    DO.DeliveryStatus.InProgress => OrderStatus.InDelivery,
                    DO.DeliveryStatus.Delivered => OrderStatus.Delivered,
                    DO.DeliveryStatus.Canceled => OrderStatus.Canceled,
                    _ => OrderStatus.Failed
                };
            })
            .GroupBy(status => status)
            .ToDictionary(g => g.Key, g => g.Count());
    }



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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void ThrowOnSimulatorIsRunning()
    {
        if (s_thread is not null)
            throw new BO.BLTemporaryNotAvailableException(
                "Cannot perform the operation since Simulator is running");
    }
    private static void clockRunner()
    {
        try
        {
            while (!s_stop)
            {
                DateTime oldClock = Now;
                DateTime newClock = Now.AddMinutes(s_interval);

                UpdateClock(newClock, fromSimulator: true);

                Task.Run(() =>
                {
                    DeliveryManager.AssignOrdersAutomatically(oldClock, newClock);
                });

                Thread.Sleep(1000);
            }
        }
        catch (ThreadInterruptedException)
        {
            // exit gracefully
        }
    }

    /// <summary>
    /// Starts the simulator clock runner with the given interval.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] // stage 7
    internal static void Start(int interval)
    {
        if (s_thread is null)
        {
            s_interval = interval;
            s_stop = false;

            s_thread = new Thread(clockRunner)
            {
                Name = "ClockRunner"
            };

            s_thread.Start();
        }
    }

    /// <summary>
    /// Stops the simulator clock runner.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] // stage 7
    internal static void Stop()
    {
        if (s_thread is not null)
        {
            s_stop = true;
            s_thread.Interrupt(); // awaken a sleeping thread
            s_thread.Name = "ClockRunner stopped";
            s_thread = null;
        }
    }

    /// <summary>
    /// Returns a summary of orders grouped by ScheduleStatus
    /// (OnTime / AtRisk / Late).
    /// </summary>
    internal static IDictionary<ScheduleStatus, int>
        GetOrdersCountByScheduleStatus()
    {
        var deliveries = s_dal.Delivery.ReadAll();

        return deliveries
            .Select(d => CalcScheduleStatus(d))
            .GroupBy(status => status)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Calculates the delivery schedule status based on
    /// actual distance vs expected distance.
    /// </summary>
    private static ScheduleStatus CalcScheduleStatus(DO.Delivery delivery)
    {
        if (delivery.ExpectedDistance is null || delivery.ExpectedDistance <= 0)
            return ScheduleStatus.OnTime;

        double ratio =
            delivery.ActualDistance / delivery.ExpectedDistance.Value;

        if (ratio <= 1.0)
            return ScheduleStatus.OnTime;

        if (ratio <= 1.2)
            return ScheduleStatus.SlightDelay;

        return ScheduleStatus.Late;
    }
}
