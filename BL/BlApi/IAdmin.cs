using BO;

namespace BlApi;

/// <summary>
/// Administration service.
/// Allows managing system clock,
/// configuration values and database.
/// </summary>
public interface IAdmin
{
    /// <summary>Returns the current system logical clock.</summary>
    DateTime GetClock();

    /// <summary>
    /// Updates the logical system clock.
    /// </summary>
    /// <param name="newClock">New date and time.</param>
    void UpdateClock(DateTime newClock);

    /// <summary>
    /// Returns current system configuration.
    /// </summary>
    Config GetConfig();

    /// <summary>
    /// Updates configuration values.
    /// </summary>
    /// <param name="config">Updated configuration.</param>
    void SetConfig(Config config);

    /// <summary>
    /// Initializes database with demo data.
    /// </summary>
    void InitializeDB();

    /// <summary>
    /// Clears database and resets it to empty state.
    /// </summary>
    void ResetDB();

    #region Stage 5  

    /// <summary>
    /// Registers observer for configuration updates.
    /// </summary>
    void AddConfigObserver(Action configObserver);

    /// <summary>
    /// Removes observer for configuration updates.
    /// </summary>
    void RemoveConfigObserver(Action configObserver);

    /// <summary>
    /// Registers observer for clock changes.
    /// </summary>
    void AddClockObserver(Action clockObserver);

    /// <summary>
    /// Removes observer for clock changes.
    /// </summary>
    void RemoveClockObserver(Action clockObserver);

    #endregion 
}
