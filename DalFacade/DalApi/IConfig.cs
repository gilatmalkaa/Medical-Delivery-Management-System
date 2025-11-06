namespace DalApi;

public interface IConfig
{
    /// <summary>
    /// Represents the current system clock used for time-based operations.
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// Represents the maximum allowed operational range (for example, delivery distance).
    /// </summary>
    int MaxRange { get; set; }

    /// <summary>
    /// Resets configuration values to their default settings.
    /// </summary>
    void Reset();
}
