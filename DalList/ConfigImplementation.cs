using DalApi;
namespace Dal;
using DalListData;
using DO;

/// <summary>
/// Implementation class for the configuration interface (IConfig).
/// Provides controlled access to system configuration values 
/// defined in the internal DalList.Config class.
/// </summary>
internal class ConfigImplementation : IConfig
{
    /// <summary>
    /// Gets or sets the current system clock used for simulations and scheduling.
    /// Changes here will update the central configuration clock in DalList.Config.
    /// </summary>
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }

    /// <summary>
    /// Gets the maximum range value or limit of the system.
    /// This property is read-only and will throw an exception if modified.
    /// </summary>
    /// <exception cref="NotSupportedException">
    /// Thrown when attempting to set a value to a read-only property.
    /// </exception>
    public int MaxRange
    {
        get => Config.NextOrderId;
        set => throw new DalUnsupportedOperationException("MaxRange is read-only in Config.");

    }

    /// <summary>
    /// Resets all configuration settings and counters 
    /// to their initial values as defined in DalList.Config.
    /// </summary>
    public void Reset()
    {
        Config.Reset();
    }
}
