namespace BlApi;
using BlImplementation;
using DalApi;
using System.Data.SqlTypes;

/// <summary>
/// Factory class responsible for providing access
/// to the Business Logic layer.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Creates and returns an instance of the BL implementation.
    /// </summary>
    public static IBl Get() => new BlImplementation.Bl();

    /// <summary>
    /// Gets the singleton instance of the DAL.
    /// </summary>
    /// </summary>
    public static IDal GetDal() => DalApi.Factory.Get;
}
