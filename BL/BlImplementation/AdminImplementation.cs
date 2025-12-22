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

    public void SetConfig(Config config)
        => AdminManager.SetConfig(config);

    public void InitializeDB() => AdminManager.InitializeDB();

    public void ResetDB() => AdminManager.ResetDB();
}
