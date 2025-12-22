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
}
