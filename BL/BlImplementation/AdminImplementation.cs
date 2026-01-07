namespace BlApi;

using BO;
using Helpers;

/// <summary>
/// Implementation of IAdmin
/// </summary>
internal class AdminImplementation : IAdmin
{
    public DateTime GetClock() => AdminManager.Now;

    public void UpdateClock(DateTime newClock) =>
        AdminManager.UpdateClock(newClock);

    public Config GetConfig() => AdminManager.GetConfig();

    public void SetConfig(Config config) =>
        AdminManager.SetConfig(config);

    public void InitializeDB() =>
        AdminManager.InitializeDB();

    public void ResetDB() =>
        AdminManager.ResetDB();

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
