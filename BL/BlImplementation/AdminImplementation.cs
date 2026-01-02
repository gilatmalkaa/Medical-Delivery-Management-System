using BlApi;
using BO;
using Helpers;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    public DateTime GetClock() => AdminManager.Now;

    public void UpdateClock(DateTime newClock)
        => AdminManager.UpdateClock(newClock);

    public Config GetConfig() => AdminManager.GetConfig();

    public void SetConfig(Config config)using BlApi;
using BO;
using Helpers;

namespace BlImplementation;

/// <summary>
/// Implementation of <see cref="IAdmin"/>.
/// Delegates all logic to the AdminManager in the BL Helpers layer.
/// </summary>
internal class AdminImplementation : IAdmin
{
    /// <summary>
    /// Returns the current logical system clock.
    /// </summary>
    public DateTime GetClock() => AdminManager.Now;

    /// <summary>
    /// Updates the logical system clock.
    /// </summary>
    public void UpdateClock(DateTime newClock)
        => AdminManager.UpdateClock(newClock);

    /// <summary>
    /// Returns current system configuration.
    /// </summary>
    public Config GetConfig() => AdminManager.GetConfig();

    /// <summary>
    /// Updates system configuration values.
    /// </summary>
    public void SetConfig(Config config)
        => AdminManager.SetConfig(config);

    /// <summary>
    /// Initializes database with demo data.
    /// </summary>
    public void InitializeDB() => AdminManager.InitializeDB();

    /// <summary>
    /// Clears database and resets it.
    /// </summary>
    public void ResetDB() => AdminManager.ResetDB();

    #region Stage 5

    /// <summary>
    /// Registers observer for clock changes.
    /// </summary>
    public void AddClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers += clockObserver;

    /// <summary>
    /// Removes observer for clock changes.
    /// </summary>
    public void RemoveClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers -= clockObserver;

    /// <summary>
    /// Registers observer for configuration changes.
    /// </summary>
    public void AddConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers += configObserver;

    /// <summary>
    /// Removes observer for configuration changes.
    /// </summary>
    public void RemoveConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers -= configObserver;

    #endregion
}

        => AdminManager.SetConfig(config);

    public void InitializeDB() => AdminManager.InitializeDB();

    public void ResetDB() => AdminManager.ResetDB();

    #region Stage 5

    public void AddClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers += clockObserver;

    public void RemoveClockObserver(Action clockObserver) =>
        AdminManager.ClockUpdatedObservers -= clockObserver;

    public void AddConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers += configObserver;

    public void RemoveConfigObserver(Action configObserver) =>
        AdminManager.ConfigUpdatedObservers -= configObserver;

    #endregion
}
