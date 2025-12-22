using System.Runtime.CompilerServices;

namespace Helpers;

/// <summary>
/// Internal BL manager for all Application's Configuration Variables and Clock logic policies
/// </summary>
internal static class AdminManager //stage 4
{
    #region Stage 4-7
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get; //stage 4

    /// <summary>
    /// Property for providing current application's clock value for any BL class that may need it
    /// </summary>
    internal static DateTime Now { get => s_dal.Config.Clock; } //stage 4

    internal static event Action? ConfigUpdatedObservers; //stage 5
    internal static event Action? ClockUpdatedObservers;  //stage 5

    private static Task? _periodicTask = null; //stage 7

    /// <summary>
    /// Method to update application's clock from any BL class as may be required
    /// </summary>
    internal static void UpdateClock(DateTime newClock) //stage 4-7
    {
        var oldClock = s_dal.Config.Clock; //stage 4
        s_dal.Config.Clock = newClock;     //stage 4

        // TO_DO: stage 4
        // No periodic logic yet for this project

        //Calling all the observers of clock update
        ClockUpdatedObservers?.Invoke(); //prepared for stage 5
    }

    /// <summary>
    /// Method for providing current configuration variables values
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    internal static BO.Config GetConfig() //stage 4
        => new BO.Config()
        {
            Clock = s_dal.Config.Clock,
            MaxRange = s_dal.Config.MaxRange,
            SampleExpirationMinutes = s_dal.Config.SampleExpirationMinutes,
            MaxDeliveryDurationMinutes = s_dal.Config.MaxDeliveryDurationMinutes,
            FootSpeed = s_dal.Config.FootSpeed,
            BikeSpeed = s_dal.Config.BikeSpeed,
            MotorcycleSpeed = s_dal.Config.MotorcycleSpeed,
            CarSpeed = s_dal.Config.CarSpeed,
            BaseDeliveryPrice = s_dal.Config.BaseDeliveryPrice,
            PricePerKm = s_dal.Config.PricePerKm
        };

    /// <summary>
    /// Method for setting current configuration variables values
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    internal static void SetConfig(BO.Config configuration) //stage 4
    {
        bool configChanged = false; // stage 5

        if (s_dal.Config.Clock != configuration.Clock)
        {
            s_dal.Config.Clock = configuration.Clock;
            configChanged = true;
        }

        if (s_dal.Config.MaxRange != configuration.MaxRange)
        {
            s_dal.Config.MaxRange = configuration.MaxRange;
            configChanged = true;
        }

        if (s_dal.Config.SampleExpirationMinutes != configuration.SampleExpirationMinutes)
        {
            s_dal.Config.SampleExpirationMinutes = configuration.SampleExpirationMinutes;
            configChanged = true;
        }

        if (s_dal.Config.MaxDeliveryDurationMinutes != configuration.MaxDeliveryDurationMinutes)
        {
            s_dal.Config.MaxDeliveryDurationMinutes = configuration.MaxDeliveryDurationMinutes;
            configChanged = true;
        }

        if (s_dal.Config.FootSpeed != configuration.FootSpeed)
        {
            s_dal.Config.FootSpeed = configuration.FootSpeed;
            configChanged = true;
        }

        if (s_dal.Config.BikeSpeed != configuration.BikeSpeed)
        {
            s_dal.Config.BikeSpeed = configuration.BikeSpeed;
            configChanged = true;
        }

        if (s_dal.Config.MotorcycleSpeed != configuration.MotorcycleSpeed)
        {
            s_dal.Config.MotorcycleSpeed = configuration.MotorcycleSpeed;
            configChanged = true;
        }

        if (s_dal.Config.CarSpeed != configuration.CarSpeed)
        {
            s_dal.Config.CarSpeed = configuration.CarSpeed;
            configChanged = true;
        }

        if (s_dal.Config.BaseDeliveryPrice != configuration.BaseDeliveryPrice)
        {
            s_dal.Config.BaseDeliveryPrice = configuration.BaseDeliveryPrice;
            configChanged = true;
        }

        if (s_dal.Config.PricePerKm != configuration.PricePerKm)
        {
            s_dal.Config.PricePerKm = configuration.PricePerKm;
            configChanged = true;
        }

        if (configChanged)
            ConfigUpdatedObservers?.Invoke(); // stage 5
    }

    internal static void ResetDB() //stage 4-7
    {
        lock (BlMutex) //stage 7
        {
            s_dal.ResetDB(); //stage 4
            UpdateClock(Now); //stage 5
            SetConfig(GetConfig()); //stage 5
        }
    }

    internal static void InitializeDB() //stage 4-7
    {
        lock (BlMutex) //stage 7
        {
            //DalTest.Initialization.Do(); //stage 4
            UpdateClock(Now);  //stage 5
            SetConfig(GetConfig()); //stage 5
        }
    }
    #endregion

    #region Stage 7 base
    internal static readonly object BlMutex = new();
    #endregion
}
