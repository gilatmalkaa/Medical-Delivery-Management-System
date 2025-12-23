using BO;

namespace BlApi;

public interface IAdmin
{
    DateTime GetClock();
    void UpdateClock(DateTime newClock);

    Config GetConfig();
    void SetConfig(Config config);

    void InitializeDB();
    void ResetDB();

    #region Stage 5 
    void AddConfigObserver(Action configObserver);
    void RemoveConfigObserver(Action configObserver);
    void AddClockObserver(Action clockObserver);
    void RemoveClockObserver(Action clockObserver);

    #endregion Stage 5 
}
