namespace BlApi;
using BlImplementation;

/// <summary>
/// Factory class responsible for providing access
/// to the Business Logic layer.
/// </summary>
public static class Factory
{
    /// <summary
    /// Creates and returns an instance of the BL implementation.
    /// </summary>
    public static IBl Get() => new BlImplementation.Bl();
}
