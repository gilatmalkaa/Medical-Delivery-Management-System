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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        // Static access to the Business Logic layer
        static readonly IBl s_bl = BlApi.Factory.Get();

        private Action clockObserver;
        private Action configObserver;


        // System clock
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

        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddMinutes(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddHours(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            DateTime current = s_bl.Admin.GetClock();
            DateTime updated = current.AddDays(1);

            s_bl.Admin.UpdateClock(updated);
            CurrentTime = updated;
        }

        private void btnUpdateConfig_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.SetConfig(Configuration);
        }

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
        private void ClockObserver()
        {
            Dispatcher.Invoke(() =>
            {
                CurrentTime = s_bl.Admin.GetClock();
            });
        }

        private void ConfigObserver()
        {
            Dispatcher.Invoke(() =>
            {
                Configuration = s_bl.Admin.GetConfig();
            });
        }

        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // a. Load initial system clock
            CurrentTime = s_bl.Admin.GetClock();

            // b. Load initial configuration
            Configuration = s_bl.Admin.GetConfig();

            // c. Register clock observer
            s_bl.Admin.AddClockObserver(clockObserver);

            // d. Register config observer
            s_bl.Admin.AddConfigObserver(configObserver);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);

            s_bl.Admin.RemoveConfigObserver(configObserver);
        }

        private void btnCouriers_Click(object sender, RoutedEventArgs e)
        {
            new CourierListWindow().Show();
        }

        private void btnDeliveries_Click(object sender, RoutedEventArgs e)
        {
            new DeliveryListWindow().Show();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            new OrderListWindow().Show();
        }

        private void CloseOtherWindows()
        {
            foreach (Window w in Application.Current.Windows)
                if (w != this)
                    w.Close();
        }

        // Helper: wait cursor
        private void RunWithWaitCursor(Action action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try { action(); }
            finally { Mouse.OverrideCursor = null; }
        }

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

            MessageBox.Show(
                "Database was successfully initialized.",
                "Operation Completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

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

            MessageBox.Show(
                "Database was successfully reset.",
                "Operation Completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        public AdminWindow()
        {
            InitializeComponent();
            // Initial values
            CurrentTime = s_bl.Admin.GetClock();
            Configuration = s_bl.Admin.GetConfig();

            // Create observers
            clockObserver = ClockObserver;
            configObserver = ConfigObserver;

            // Register observers
            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);

        }
    }
}
