using DalApi;

namespace Dal;

public class ConfigImplementation : IConfig
{
    public DateTime Clock
    {
        get => DalList.Config.Clock;
        set => DalList.Config.Clock = value;
    }

    public int MaxRange
    {
        get => DalList.Config.NextOrderId;
        set => throw new NotSupportedException("MaxRange is read-only in Config.");
    }

    public void Reset()
    {
        DalList.Config.Reset();
    }
}
