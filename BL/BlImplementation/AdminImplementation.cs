namespace BlApi;

using DalApi;
using BO;
using Helpers;

/// <summary>
/// Concrete implementation of the IAdmin interface.
/// Acts as a facade that delegates all administrative
/// operations to the AdminManager in the Helpers layer.
/// </summary>
internal class AdminImplementation : IAdmin
{
    /// <summary>
    /// Returns the current logical system clock.
    /// </summary>
    public DateTime GetClock() => AdminManager.Now;

    /// <summary>
    /// Updates the logical system clock to a new value.
    /// </summary>
    /// <param name="newClock">New date and time to set.</param>
    public void UpdateClock(DateTime newClock) =>
        AdminManager.UpdateClock(newClock);

    /// <summary>
    /// Retrieves the current system configuration.
    /// </summary>
    /// <returns>The current configuration object.</returns>
    public Config GetConfig() => AdminManager.GetConfig();

    /// <summary>
    /// Updates the system configuration with new values.
    /// </summary>
    /// <param name="config">Updated configuration data.</param>
    public void SetConfig(Config config) =>
        AdminManager.SetConfig(config);

    /// <summary>
    /// Initializes the database with predefined demo data.
    /// </summary>
    public void InitializeDB() =>
        AdminManager.InitializeDB();

    /// <summary>
    /// Clears all data from the database and resets it
    /// to an empty initial state.
    /// </summary>
    public void ResetDB() =>
        AdminManager.ResetDB();

    #region Stage 5

    /// <summary>
    /// Registers an observer that is notified when
    /// the system clock is updated.
    /// </summary>
    /// <param name="clockObserver">Callback to invoke on clock changes.</param>
    public void AddClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers += clockObserver;

    /// <summary>
    /// Removes a previously registered clock observer.
    /// </summary>
    /// <param name="clockObserver">The observer callback to remove.</param>
    public void RemoveClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers -= clockObserver;

    /// <summary>
    /// Registers an observer that is notified when
    /// system configuration values are updated.
    /// </summary>
    /// <param name="configObserver">Callback to invoke on configuration changes.</param>
    public void AddConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers += configObserver;

    /// <summary>
    /// Removes a previously registered configuration observer.
    /// </summary>
    /// <param name="configObserver">The observer callback to remove.</param>
    public void RemoveConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers -= configObserver;

    /// <summary>
    /// Returns the number of orders grouped by their current status.
    /// </summary>
    /// <returns>
    /// A dictionary mapping each order status to the
    /// number of orders in that status.
    /// </returns>
    public IDictionary<OrderStatus, int> OrdersCountByStatus => AdminManager.GetOrdersCountByStatus();

    /// <summary>
    /// Authenticates a user and returns the corresponding system role.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="password">User password.</param>
    /// <returns>The role assigned to the authenticated user.</returns>
    public UserRole Login(string id, string password)
    {
        return AdminManager.Login(id, password);
    }

    #region Authentication
    // Authentication-related logic is delegated entirely
    // to the AdminManager helper class.
    #endregion

    #endregion
    /// <summary>
    /// Starts the simulator if it is not already running.
    /// </summary>
    /// <param name="interval">Clock advance interval in minutes.</param>
    public void StartSimulator(int interval) 
    {
        AdminManager.ThrowOnSimulatorIsRunning(); 
        AdminManager.Start(interval);             
    }

    /// <summary>
    /// Stops the simulator.
    /// </summary>
    public void StopSimulator()
    {
        AdminManager.Stop();
    }

    /// <summary>
    /// Gets order counts grouped by ScheduleStatus
    /// (OnTime / AtRisk / Late).
    /// </summary>
    public IDictionary<ScheduleStatus, int>
        GetOrdersCountByScheduleStatus()
    {
        return Helpers.AdminManager.GetOrdersCountByScheduleStatus();
    }

}
