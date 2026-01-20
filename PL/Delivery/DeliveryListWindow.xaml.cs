using BlApi;
using BO;
using PL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Delivery
{
    /// <summary>
    /// Displays and manages a list of deliveries,
    /// supporting filtering, selection, and status updates.
    /// </summary>
    public partial class DeliveryListWindow : Window
    {
        /// <summary>
        /// Business layer access.
        /// </summary>
        private static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// Synchronization mutex for clock observer updates (Stage 7).
        /// Prevents concurrent or overlapping UI refreshes.
        /// </summary>
        private readonly ObserverMutex _deliveryMutex = new(); // stage 7


        /// <summary>
        /// Gets or sets the list of deliveries displayed in the window.
        /// </summary>
        public IEnumerable<DeliveryPerOrderInList> DeliveryList
        {
            get => (IEnumerable<DeliveryPerOrderInList>)GetValue(DeliveryListProperty);
            set => SetValue(DeliveryListProperty, value);
        }

        public static readonly DependencyProperty DeliveryListProperty =
            DependencyProperty.Register(
                nameof(DeliveryList),
                typeof(IEnumerable<DeliveryPerOrderInList>),
                typeof(DeliveryListWindow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected delivery.
        /// </summary>
        public DeliveryPerOrderInList? SelectedDelivery { get; set; }

        /// <summary>
        /// Gets or sets the selected delivery status filter.
        /// </summary>
        public BO.DeliveryStatus? SelectedStatus { get; set; }

        /// <summary>
        /// Initializes the delivery list window.
        /// Actual data loading is done asynchronously on load.
        /// </summary>
        public DeliveryListWindow()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += DeliveryListWindow_Loaded;
        }

        /// <summary>
        /// Loads deliveries asynchronously when the window is loaded.
        /// </summary>
        private async void DeliveryListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshListAsync();
        }

        /// <summary>
        /// Refreshes the delivery list according to the selected status filter.
        /// Executes the BL call on a background thread.
        /// </summary>
        private async Task RefreshListAsync()
        {
            if (_deliveryMutex.CheckAndSetLoadInProgressOrRestartRequired())
                return;

            await Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    var list = await Task.Run(() =>
                        s_bl.Deliveries.ReadAll(
                            d => SelectedStatus == null ||
                                 d.CompletionStatus == SelectedStatus));

                    DeliveryList = list;
                }
                finally
                {
                    if (await _deliveryMutex.UnsetLoadInProgressAndCheckRestartRequested())
                        await RefreshListAsync();
                }
            });
        }


        /// <summary>
        /// Handles changes in the delivery status filter.
        /// </summary>
        private async void StatusFilter_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            await RefreshListAsync();
        }

        /// <summary>
        /// Opens the delivery update window on double-click
        /// and refreshes the list after closing.
        /// </summary>
        private async void List_MouseDoubleClick(
            object sender,
            MouseButtonEventArgs e)
        {
            if (SelectedDelivery is null)
                return;

            new DeliveryAddUpdateWindow(
                SelectedDelivery.DeliveryId).ShowDialog();

            await RefreshListAsync();
        }

        /// <summary>
        /// Opens the delivery update window for the selected delivery.
        /// Creation of new deliveries is not allowed from this screen.
        /// </summary>
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDelivery is null)
            {
                MessageBox.Show(
                    "A delivery cannot be created from this screen.\n" +
                    "Select a delivery to manage it.",
                    "Stage 4 restriction",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            new DeliveryAddUpdateWindow(
                SelectedDelivery.DeliveryId).ShowDialog();

            await RefreshListAsync();
        }
    }
}
