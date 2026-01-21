using BlApi;
using BO;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PL.Courier
{
    /// <summary>
    /// Main operational window for a courier.
    /// Allows managing the current delivery,
    /// choosing new orders, updating personal details,
    /// and viewing delivery history.
    /// </summary>
    public partial class CourierMainWindow : Window
    {
        /// <summary>
        /// Business layer access.
        /// </summary>
        private static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// The courier currently logged into the system.
        /// </summary>
        public BO.Courier Courier { get; private set; }

        /// <summary>
        /// Indicates whether the courier is allowed to choose a new order.
        /// A courier can choose an order only if active and not currently delivering.
        /// </summary>
        public bool CanChooseOrder =>
            Courier != null && Courier.IsActive && Courier.CurrentOrder == null;

        /// <summary>
        /// Indicates whether the courier can complete the current delivery.
        /// </summary>
        public bool CanFinishDelivery =>
            Courier?.CurrentOrder != null &&
            Courier.CurrentOrder.OrderStatus == BO.OrderStatus.InDelivery;


        /// <summary>
        /// The identifier of the courier displayed in this window.
        /// </summary>
        private readonly int _courierId;

        /// <summary>
        /// Constructs the courier main window for a specific courier.
        /// </summary>
        /// <param name="courierId">Courier identifier.</param>
        public CourierMainWindow(int courierId)
        {
            InitializeComponent();
            _courierId = courierId;

            // Load courier data asynchronously once the window is ready
            Loaded += CourierMainWindow_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the window.
        /// Loads the courier data asynchronously and binds it to the UI.
        /// </summary>
        private async void CourierMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshCourierAsync();
        }

        /// <summary>
        /// Reloads the courier data from the business layer asynchronously
        /// and refreshes the UI bindings.
        /// </summary>
        private async Task RefreshCourierAsync()
        {
            // Fetch courier data from BL on a background thread
            Courier = await Task.Run(() =>
                s_bl.Couriers.Get(_courierId));

            // Update UI bindings on the UI thread
            Dispatcher.Invoke(() =>
            {
                DataContext = null;
                DataContext = this;
            });
        }

        /// <summary>
        /// Completes the current delivery assigned to the courier.
        /// </summary>
        private async void BtnFinishDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Complete delivery through BL (background thread)
                await Task.Run(() =>
                    s_bl.Couriers.CompleteDelivery(Courier.Id));

                // Refresh courier data after completion
                await RefreshCourierAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot finish delivery",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the courier update window and refreshes
        /// the courier data after closing it.
        /// </summary>
        private async void BtnUpdateCourierDetails_Click(object sender, RoutedEventArgs e)
        {
            var updateWindow =
                new CourierAddUpdateWindow(Courier.Id);

            updateWindow.ShowDialog();

            // Reload courier data after update
            await RefreshCourierAsync();
        }

        /// <summary>
        /// Opens the delivery history window for the courier.
        /// </summary>
        private void BtnDeliveryHistory_Click(object sender, RoutedEventArgs e)
        {
            new CourierDeliveryHistoryWindow(Courier.Id)
                .ShowDialog();
        }

        /// <summary>
        /// Allows the courier to choose a new order,
        /// if the courier is currently available.
        /// </summary>
        private async void BtnChooseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate availability using fresh data from BL
                var courier = await Task.Run(() =>
                    s_bl.Couriers.Get(Courier.Id));

                if (!courier.IsAvailable)
                {
                    MessageBox.Show(
                        "Courier already has an active delivery",
                        "Cannot choose order",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Open order selection window
                new ChooseOrderWindow(Courier.Id)
                    .ShowDialog();

                // Refresh courier data after choosing an order
                await RefreshCourierAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Choose order failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
