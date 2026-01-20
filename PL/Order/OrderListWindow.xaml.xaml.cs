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

namespace PL.Order
{
    /// <summary>
    /// Displays and manages a list of orders,
    /// supporting filtering, automatic refresh, and navigation
    /// to the order details window.
    /// </summary>
    public partial class OrderListWindow : Window
    {
        /// <summary>
        /// Business logic facade used by the admin window.
        /// </summary>
        static readonly IBl s_bl = BlApi.Factory.Get();


        /// <summary>
        /// Synchronization mutex for clock observer updates (Stage 7).
        /// Prevents concurrent or overlapping UI refreshes.
        /// </summary>
        private readonly ObserverMutex _orderMutex = new(); // stage 7

        /// <summary>
        /// Gets or sets the list of orders displayed in the window.
        /// </summary>
        public IEnumerable<OrderInList> OrderList
        {
            get => (IEnumerable<OrderInList>)GetValue(OrderListProperty);
            set => SetValue(OrderListProperty, value);
        }

        public static readonly DependencyProperty OrderListProperty =
            DependencyProperty.Register(
                nameof(OrderList),
                typeof(IEnumerable<OrderInList>),
                typeof(OrderListWindow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the selected order status filter.
        /// </summary>
        public BO.OrderStatus? SelectedStatus { get; set; } = null;

        /// <summary>
        /// Gets or sets the currently selected order.
        /// </summary>
        public OrderInList? SelectedOrder { get; set; }

        /// <summary>
        /// Initializes the order list window.
        /// Data is loaded asynchronously on window load.
        /// </summary>
        public OrderListWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Loads the orders asynchronously and applies the filter.
        /// </summary>
        private async Task LoadOrdersAsync()
        {
            var allOrders = await Task.Run(() => s_bl.Orders.GetAll());

            OrderList =
                SelectedStatus == null || SelectedStatus == BO.OrderStatus.All
                    ? allOrders
                    : allOrders.Where(o => o.OrderStatus == SelectedStatus);
        }

        /// <summary>
        /// Handles changes in the status filter selection.
        /// </summary>
        private async void StatusFilter_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            await LoadOrdersAsync();
        }

        /// <summary>
        /// Observer callback to refresh the list when BL changes.
        /// </summary>
        private void OrderListObserver()
        {
            if (_orderMutex.CheckAndSetLoadInProgressOrRestartRequired())
                return;

            Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    await LoadOrdersAsync();
                }
                finally
                {
                    if (await _orderMutex.UnsetLoadInProgressAndCheckRestartRequested())
                        OrderListObserver();
                }
            });
        }


        /// <summary>
        /// Registers the observer and loads data on window load.
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Orders.AddObserver(OrderListObserver);
            s_bl.Admin.AddClockObserver(OrderListObserver); 

            await LoadOrdersAsync();
        }

        /// <summary>
        /// Unregisters the observer when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Orders.RemoveObserver(OrderListObserver);
            s_bl.Admin.RemoveClockObserver(OrderListObserver);

        }

        /// <summary>
        /// Opens the order details window for the selected order.
        /// </summary>
        private void OrdersList_MouseDoubleClick(
            object sender,
            MouseButtonEventArgs e)
        {
            if (SelectedOrder == null)
                return;

            new OrderDetailsWindow(SelectedOrder.OrderId).ShowDialog();
        }

        /// <summary>
        /// Opens the order details window in create mode.
        /// </summary>
        private void btnAddOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            new OrderDetailsWindow(0).ShowDialog();
        }

        /// <summary>
        /// Displays a message indicating that deleting orders is not supported.
        /// </summary>
        private void BtnDelete_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Deleting an order is not allowed in the system.",
                "Operation not supported",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            e.Handled = true;
        }

        /// <summary>
        /// Cancels the selected order if allowed.
        /// </summary>
        private async void BtnCancel_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button btn ||
                btn.DataContext is not OrderInList order)
                return;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() =>
                    s_bl.Orders.Cancel(order.OrderId));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot cancel order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Prevents mouse interaction from propagating further.
        /// </summary>
        private void Button_PreviewMouseDown(
            object sender,
            MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
