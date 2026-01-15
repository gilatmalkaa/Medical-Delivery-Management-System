using BlApi;
using BO;
using PL.Courier;
using PL.Delivery;
using PL.Order;
using System.Windows;
using System.Windows.Input;

namespace PL.Admin
{
    /// <summary>
    /// Provides an administrative control panel for managing
    /// system configuration, system clock, database lifecycle,
    /// and navigation to management screens.
    /// </summary>
    public partial class AdminWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        private Action clockObserver;
        private Action configObserver;

        /// <summary>
        /// Gets or sets the system time displayed in the UI.
        /// </summary>
        public DateTime SystemTime
        {
            get { return (DateTime)GetValue(SystemTimeProperty); }
            set { SetValue(SystemTimeProperty, value); }
        }

        public static readonly DependencyProperty SystemTimeProperty =
            DependencyProperty.Register(
                nameof(SystemTime),
                typeof(DateTime),
                typeof(AdminWindow),
                new PropertyMetadata(DateTime.Now)
            );

        /// <summary>
        /// Gets or sets the current system clock value.
        /// </summary>
        public DateTime CurrentTime
        {
            get => (DateTime)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                nameof(CurrentTime),
                typeof(DateTime),
                typeof(AdminWindow),
                new PropertyMetadata(DateTime.Now)
            );

        /// <summary>
        /// Advances the system clock by one minute.
        /// </summary>
        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddMinutes(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        /// <summary>
        /// Advances the system clock by one hour.
        /// </summary>
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddHours(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        /// <summary>
        /// Advances the system clock by one day.
        /// </summary>
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        /// <summary>
        /// Advances the system clock by one month.
        /// </summary>
        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        /// <summary>
        /// Advances the system clock by one year.
        /// </summary>
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        /// <summary>
        /// Updates system configuration values.
        /// </summary>
        private void btnUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetConfig(Configuration);
            MessageBox.Show("Configuration updated successfully");
        }

        /// <summary>
        /// Gets or sets the system configuration displayed in the UI.
        /// </summary>
        public Config Configuration
        {
            get { return (Config)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        public static readonly DependencyProperty ConfigurationProperty =
            DependencyProperty.Register(
                nameof(Configuration),
                typeof(Config),
                typeof(AdminWindow),
                new PropertyMetadata(null)
            );

        /// <summary>
        /// Updates the UI when the system clock changes.
        /// </summary>
        private void ClockObserver()
        {
            Dispatcher.Invoke(() =>
            {
                CurrentTime = s_bl.Admin.GetClock();
            });
        }

        /// <summary>
        /// Updates the UI when the configuration changes.
        /// </summary>
        private void ConfigObserver()
        {
            Dispatcher.Invoke(() =>
            {
                Configuration = s_bl.Admin.GetConfig();
            });
        }

        /// <summary>
        /// Initializes data and registers observers when the window is loaded.
        /// </summary>
        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.Admin.GetClock();
            Configuration = s_bl.Admin.GetConfig();

            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);
        }

        /// <summary>
        /// Unregisters observers when the window is closed.
        /// </summary>
        private void AdminWindow_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
        }

        /// <summary>
        /// Opens the courier management window.
        /// </summary>
        private void btnCouriers_Click(object sender, RoutedEventArgs e)
        {
            OpenSingleWindow<CourierListWindow>();
        }

        /// <summary>
        /// Opens the order management window.
        /// </summary>
        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            OpenSingleWindow<OrderListWindow>();
        }

        /// <summary>
        /// Closes all other open windows except this one.
        /// </summary>
        private void CloseOtherWindows()
        {
            foreach (Window w in Application.Current.Windows)
                if (w != this)
                    w.Close();
        }

        /// <summary>
        /// Executes an action while displaying a wait cursor.
        /// </summary>
        private void RunWithWaitCursor(Action action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try { action(); }
            finally { Mouse.OverrideCursor = null; }
        }

        /// <summary>
        /// Initializes the database with demo data.
        /// </summary>
        private void btnInitDB_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to initialize the database?\n" +
                "Existing data will be deleted and demo data will be created.",
                "Initialize Database",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            CloseOtherWindows();

            RunWithWaitCursor(() =>
            {
                s_bl.Admin.InitializeDB();
            });

            LoadOrdersSummary();

            MessageBox.Show(
                "Database was successfully initialized.",
                "Operation Completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Resets the database and removes all stored data.
        /// </summary>
        private void btnResetDB_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to reset the database?",
                "Reset Database",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            CloseOtherWindows();

            RunWithWaitCursor(() =>
            {
                s_bl.Admin.ResetDB();
            });

            LoadOrdersSummary();

            MessageBox.Show(
                "Database was successfully reset.",
                "Operation Completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Initializes the admin window, registers observers,
        /// and loads initial system data.
        /// </summary>
        public AdminWindow()
        {
            InitializeComponent();

            CurrentTime = s_bl.Admin.GetClock();
            Configuration = s_bl.Admin.GetConfig();

            clockObserver = ClockObserver;
            configObserver = ConfigObserver;

            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);

            LoadOrdersSummary();
        }

        /// <summary>
        /// Retrieves the count of orders for a specific status.
        /// </summary>
        private static int GetCount(
            IDictionary<OrderStatus, int> summary,
            OrderStatus status)
        {
            return summary.TryGetValue(status, out int count)
                ? count
                : 0;
        }

        /// <summary>
        /// Loads and displays the summary of orders by status.
        /// </summary>
        private void LoadOrdersSummary()
        {
            var summary = s_bl.Admin.GetOrdersCountByStatus();

            CreatedOrdersText.Text =
                $"Created: {GetCount(summary, OrderStatus.Created)}";

            InDeliveryOrdersText.Text =
                $"In Delivery: {GetCount(summary, OrderStatus.InDelivery)}";

            DeliveredOrdersText.Text =
                $"Delivered: {GetCount(summary, OrderStatus.Delivered)}";

            FailedOrdersText.Text =
                $"Failed: {GetCount(summary, OrderStatus.Failed)}";
        }

        /// <summary>
        /// Opens a single instance of a window of the specified type.
        /// </summary>
        private void OpenSingleWindow<T>() where T : Window, new()
        {
            var existingWindow = Application.Current.Windows
                .OfType<T>()
                .FirstOrDefault();

            if (existingWindow != null)
            {
                if (existingWindow.WindowState == WindowState.Minimized)
                    existingWindow.WindowState = WindowState.Normal;

                existingWindow.Activate();
                existingWindow.Focus();
            }
            else
            {
                new T().Show();
            }
        }
    }
}
