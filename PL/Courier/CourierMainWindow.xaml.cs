using BlApi;
using BO;
using System;
using System.Windows;

namespace PL.Courier
{
    /// <summary>
    /// Provides the main operational window for a courier,
    /// enabling delivery completion, order selection,
    /// profile updates, and delivery history viewing.
    /// </summary>
    public partial class CourierMainWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// Gets the courier currently logged into the system.
        /// </summary>
        public BO.Courier Courier { get; private set; }

        /// <summary>
        /// Indicates whether the courier is eligible to choose a new order.
        /// </summary>
        public bool CanChooseOrder =>
            Courier.IsActive && Courier.CurrentOrder == null;

        /// <summary>
        /// Indicates whether the courier can complete the current delivery.
        /// </summary>
        public bool CanFinishDelivery =>
            Courier != null &&
            Courier.CurrentOrder != null;

        /// <summary>
        /// Initializes the main courier window for the specified courier.
        /// </summary>
        public CourierMainWindow(int courierId)
        {
            Courier = s_bl.Couriers.Get(courierId);
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Completes the current delivery assigned to the courier.
        /// </summary>
        private void BtnFinishDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Couriers.CompleteDelivery(Courier.Id);
                RefreshCourier();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot finish delivery");
            }
        }

        /// <summary>
        /// Refreshes the courier data from the business layer.
        /// </summary>
        private void RefreshCourier()
        {
            Courier = s_bl.Couriers.Get(Courier.Id);
            DataContext = null;
            DataContext = this;
        }

        /// <summary>
        /// Opens the courier update window and refreshes the data after closing.
        /// </summary>
        private void BtnUpdateCourierDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updateWindow = new CourierAddUpdateWindow(Courier.Id);
                updateWindow.ShowDialog();

                RefreshCourier();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Courier Failed");
            }
        }

        /// <summary>
        /// Opens the delivery history window for the courier.
        /// </summary>
        private void BtnDeliveryHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var historyWindow =
                    new CourierDeliveryHistoryWindow(Courier.Id);

                historyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot open delivery history");
            }
        }

        /// <summary>
        /// Allows the courier to choose a new order if available.
        /// </summary>
        private void BtnChooseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var courier = s_bl.Couriers.Get(Courier.Id);

                if (!courier.IsAvailable)
                {
                    MessageBox.Show(
                        "Courier already has an active delivery",
                        "Cannot choose order",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                new ChooseOrderWindow(Courier.Id).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
