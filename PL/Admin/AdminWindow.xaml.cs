using BlApi;
using BO;
using PL.Courier;
using PL.Order;
using PL.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Admin
{
    /// <summary>
    /// Provides an administrative control panel for managing
    /// system configuration, system clock, database lifecycle,
    /// simulator control, and navigation to management screens.
    /// </summary>
    public partial class AdminWindow : Window
    {
        /// <summary>
        /// Business logic facade used by the admin window.
        /// </summary>
        private static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Synchronization mutex for clock observer updates (Stage 7).
        /// Prevents concurrent or overlapping UI refreshes.
        /// </summary>
        private readonly ObserverMutex _clockMutex = new(); // stage 7

        /// <summary>
        /// Synchronization mutex for configuration observer updates (Stage 7).
        /// </summary>
        private readonly ObserverMutex _configMutex = new(); // stage 7

        /// <summary>
        /// Indicates whether the simulator is currently running.
        /// </summary>
        private bool _isSimulatorRunning = false;

        /// <summary>
        /// Simulator tick interval in minutes.
        /// </summary>
        private const int SimulatorIntervalMinutes = 1;

        /// <summary>
        /// Observer callback for system clock changes.
        /// </summary>
        private Action _clockObserver;

        /// <summary>
        /// Observer callback for configuration changes.
        /// </summary>
        private Action _configObserver;

        #region Dependency Properties

        /// <summary>
        /// Gets or sets the current system time displayed in the UI.
        /// </summary>
        public DateTime CurrentTime
        {
            get => (DateTime)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

        /// <summary>
        /// Dependency property backing store for CurrentTime.
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                nameof(CurrentTime),
                typeof(DateTime),
                typeof(AdminWindow),
                new PropertyMetadata(DateTime.Now));

        /// <summary>
        /// Gets or sets the system configuration displayed in the admin panel.
        /// </summary>
        public Config Configuration
        {
            get => (Config)GetValue(ConfigurationProperty);
            set => SetValue(ConfigurationProperty, value);
        }

        /// <summary>
        /// Dependency property backing store for Configuration.
        /// </summary>
        public static readonly DependencyProperty ConfigurationProperty =
            DependencyProperty.Register(
                nameof(Configuration),
                typeof(Config),
                typeof(AdminWindow),
                new PropertyMetadata(new Config()));

        #endregion

        #region Constructor / Lifecycle

        /// <summary>
        /// Initializes the admin window and registers observers.
        /// </summary>
        public AdminWindow()
        {
            InitializeComponent();

            _clockObserver = ClockObserver;
            _configObserver = ConfigObserver;

            Loaded += AdminWindow_Loaded;
            Closed += AdminWindow_Closed;
        }

        /// <summary>
        /// Handles window load event.
        /// Initializes data bindings and registers BL observers.
        /// </summary>
        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            Configuration = s_bl.Admin.GetConfig();

            s_bl.Admin.AddClockObserver(_clockObserver);
            s_bl.Admin.AddConfigObserver(_configObserver);

            LoadOrdersSummary();
            UpdateSimulatorUi();
        }

        /// <summary>
        /// Handles window close event.
        /// Stops simulator if running and unregisters observers.
        /// </summary>
        private void AdminWindow_Closed(object? sender, EventArgs e)
        {
            if (_isSimulatorRunning)
            {
                s_bl.Admin.StopSimulator();
                _isSimulatorRunning = false;
            }

            s_bl.Admin.RemoveClockObserver(_clockObserver);
            s_bl.Admin.RemoveConfigObserver(_configObserver);
        }

        #endregion

        #region Observers

        /// <summary>
        /// Observer callback invoked when the system clock changes.
        /// Refreshes time and order summary safely.
        /// </summary>
        private void ClockObserver()
        {
            if (_clockMutex.CheckAndSetLoadInProgressOrRestartRequired())
                return;

            Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    CurrentTime = s_bl.Admin.GetClock();
                    LoadOrdersSummary();
                }
                finally
                {
                    if (await _clockMutex.UnsetLoadInProgressAndCheckRestartRequested())
                        ClockObserver();
                }
            });
        }

        /// <summary>
        /// Observer callback invoked when system configuration changes.
        /// Updates configuration binding safely.
        /// </summary>
        private void ConfigObserver()
        {
            if (_configMutex.CheckAndSetLoadInProgressOrRestartRequired())
                return;

            Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    Configuration = s_bl.Admin.GetConfig();
                }
                finally
                {
                    if (await _configMutex.UnsetLoadInProgressAndCheckRestartRequested())
                        ConfigObserver();
                }
            });
        }

        #endregion

        #region Clock Buttons

        /// <summary>
        /// Advances the system clock by one minute.
        /// </summary>
        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e) =>
            UpdateClock(t => t.AddMinutes(1));

        /// <summary>
        /// Advances the system clock by one hour.
        /// </summary>
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e) =>
            UpdateClock(t => t.AddHours(1));

        /// <summary>
        /// Advances the system clock by one day.
        /// </summary>
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e) =>
            UpdateClock(t => t.AddDays(1));

        /// <summary>
        /// Advances the system clock by one month.
        /// </summary>
        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e) =>
            UpdateClock(t => t.AddMonths(1));

        /// <summary>
        /// Advances the system clock by one year.
        /// </summary>
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e) =>
            UpdateClock(t => t.AddYears(1));

        /// <summary>
        /// Updates the system clock using the provided transformation.
        /// </summary>
        private void UpdateClock(Func<DateTime, DateTime> updater)
        {
            DateTime updated = updater(s_bl.Admin.GetClock());
            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        #endregion

        #region Config

        /// <summary>
        /// Saves the updated configuration to the system.
        /// </summary>
        private void btnUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetConfig(Configuration);
            MessageBox.Show("Configuration updated successfully");
        }

        #endregion

        #region DB Operations

        /// <summary>
        /// Initializes the database with demo data.
        /// </summary>
        private async void btnInitDB_Click(object sender, RoutedEventArgs e)
        {
            if (!Confirm("Initialize Database",
                "Existing data will be deleted and demo data will be created."))
                return;

            CloseOtherWindows();

            await RunWithWaitCursorAsync(() =>
                Task.Run(() => s_bl.Admin.InitializeDB()));

            LoadOrdersSummary();
            MessageBox.Show("Database was successfully initialized.");
        }

        /// <summary>
        /// Resets the database and deletes all data.
        /// </summary>
        private async void btnResetDB_Click(object sender, RoutedEventArgs e)
        {
            if (!Confirm("Reset Database",
                "All existing data will be deleted."))
                return;

            CloseOtherWindows();

            await RunWithWaitCursorAsync(() =>
                Task.Run(() => s_bl.Admin.ResetDB()));

            LoadOrdersSummary();
            MessageBox.Show("Database was successfully reset.");
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Opens the courier management window.
        /// </summary>
        private void btnCouriers_Click(object sender, RoutedEventArgs e) =>
            OpenSingleWindow<CourierListWindow>();

        /// <summary>
        /// Opens the order management window.
        /// </summary>
        private void btnOrders_Click(object sender, RoutedEventArgs e) =>
            OpenSingleWindow<OrderListWindow>();

        /// <summary>
        /// Ensures only a single instance of a window is open.
        /// </summary>
        private void OpenSingleWindow<T>() where T : Window, new()
        {
            var existing = Application.Current.Windows.OfType<T>().FirstOrDefault();

            if (existing != null)
            {
                existing.WindowState = WindowState.Normal;
                existing.Activate();
            }
            else
            {
                new T().Show();
            }
        }

        /// <summary>
        /// Closes all windows except the admin window.
        /// </summary>
        private void CloseOtherWindows()
        {
            foreach (Window w in Application.Current.Windows)
                if (w != this)
                    w.Close();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Displays a confirmation dialog.
        /// </summary>
        private static bool Confirm(string title, string message) =>
            MessageBox.Show(message, title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;

        /// <summary>
        /// Executes an asynchronous action with a wait cursor.
        /// </summary>
        private async Task RunWithWaitCursorAsync(Func<Task> action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try { await action(); }
            finally { Mouse.OverrideCursor = null; }
        }

        /// <summary>
        /// Loads and displays a summary of orders by status.
        /// </summary>
        private void LoadOrdersSummary()
        {
            var summary = s_bl.Admin.OrdersCountByStatus;

            CreatedOrdersText.Text =
                $"Created: {Get(summary, OrderStatus.Created)}";

            InDeliveryOrdersText.Text =
                $"In Delivery: {Get(summary, OrderStatus.InDelivery)}";

            DeliveredOrdersText.Text =
                $"Delivered: {Get(summary, OrderStatus.Delivered)}";

            FailedOrdersText.Text =
                $"Failed: {Get(summary, OrderStatus.Failed)}";

            var scheduleSummary = s_bl.Admin.GetOrdersCountByScheduleStatus();

            OnTimeOrdersText.Text =
                $"On Time: {Get(scheduleSummary, ScheduleStatus.OnTime)}";

            AtRiskOrdersText.Text =
                $"At Risk: {Get(scheduleSummary, ScheduleStatus.SlightDelay)}";

            LateOrdersText.Text =
                $"Late: {Get(scheduleSummary, ScheduleStatus.Late)}";

        }

        /// <summary>
        /// Safely retrieves an order count for a given status.
        /// </summary>
        private static int Get(
            IDictionary<OrderStatus, int> summary,
            OrderStatus status) =>
            summary.TryGetValue(status, out int count) ? count : 0;

        /// <summary>
        /// Safely retrieves an order count for a given schedule status.
        /// </summary>
        private static int Get(
            IDictionary<ScheduleStatus, int> summary,
            ScheduleStatus status) =>
            summary.TryGetValue(status, out int count) ? count : 0;


        #endregion

        /// <summary>
        /// Starts or stops the simulator.
        /// </summary>
        private void btnSimulatorToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSimulatorRunning)
            {
                s_bl.Admin.StartSimulator(SimulatorIntervalMinutes);
                _isSimulatorRunning = true;
            }
            else
            {
                s_bl.Admin.StopSimulator();
                _isSimulatorRunning = false;
            }

            UpdateSimulatorUi();
        }

        /// <summary>
        /// Updates the simulator UI state and button availability.
        /// </summary>
        private void UpdateSimulatorUi()
        {
            SimulatorToggleButton.Content =
                _isSimulatorRunning ? "Stop Simulator" : "Start Simulator";

            BtnMinute.IsEnabled = !_isSimulatorRunning;
            BtnHour.IsEnabled = !_isSimulatorRunning;
            BtnDay.IsEnabled = !_isSimulatorRunning;
            BtnMonth.IsEnabled = !_isSimulatorRunning;
            BtnYear.IsEnabled = !_isSimulatorRunning;
        }
    }
}
