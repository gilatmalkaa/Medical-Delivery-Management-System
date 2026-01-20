using BlApi;
using BO;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Delivery
{
    /// <summary>
    /// Window for viewing and updating an existing delivery.
    /// Allows changing delivery status in a responsive (async) manner.
    /// </summary>
    public partial class DeliveryAddUpdateWindow : Window
    {
        /// <summary>
        /// Business layer access.
        /// </summary>
        private static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// The delivery currently displayed and edited in the window.
        /// </summary>
        public BO.DeliveryPerOrderInList CurrentDelivery { get; set; }

        /// <summary>
        /// Determines the text shown on the main action button
        /// according to whether this is a new or existing delivery.
        /// </summary>
        public string ButtonText =>
            CurrentDelivery.DeliveryId == 0
                ? "Add Delivery"
                : "Update Delivery";

        /// <summary>
        /// Identifier of the delivery handled by this window.
        /// </summary>
        private readonly int _deliveryId;

        /// <summary>
        /// Creates the delivery add/update window.
        /// If the delivery ID is 0 – the window is in add mode (read-only notice).
        /// Otherwise – loads the delivery for update.
        /// </summary>
        /// <param name="id">Delivery identifier.</param>
        public DeliveryAddUpdateWindow(int id)
        {
            InitializeComponent();

            _deliveryId = id;

            // Load delivery data after the window is loaded
            Loaded += DeliveryAddUpdateWindow_Loaded;

            DataContext = this;
        }

        /// <summary>
        /// Handles the Loaded event.
        /// Loads delivery data asynchronously to keep the UI responsive.
        /// </summary>
        private async void DeliveryAddUpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RunWithWaitCursorAsync(async () =>
            {
                if (_deliveryId == 0)
                {
                    // Delivery cannot be created directly – only via Order
                    CurrentDelivery = new BO.DeliveryPerOrderInList();
                }
                else
                {
                    // Load delivery from BL on background thread
                    CurrentDelivery = await Task.Run(() =>
                        s_bl.Deliveries.Get(_deliveryId));
                }
            });

            // Refresh bindings after loading
            DataContext = null;
            DataContext = this;
        }

        /// <summary>
        /// Saves delivery changes by updating the delivery status.
        /// The operation is performed asynchronously.
        /// </summary>
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentDelivery.DeliveryId == 0)
                {
                    MessageBox.Show(
                        "Delivery can only be created from an Order.",
                        "Operation not allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                await RunWithWaitCursorAsync(async () =>
                {
                    await Task.Run(() =>
                        s_bl.Deliveries.UpdateStatus(
                            CurrentDelivery.DeliveryId,
                            CurrentDelivery.CompletionStatus!.Value));
                });

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Operation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Executes an asynchronous operation while displaying
        /// a wait cursor to the user.
        /// </summary>
        /// <param name="action">Async action to execute.</param>
        private async Task RunWithWaitCursorAsync(Func<Task> action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                await action();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
