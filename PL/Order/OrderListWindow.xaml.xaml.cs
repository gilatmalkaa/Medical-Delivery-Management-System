using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        static readonly IBl s_bl = BlApi.Factory.Get();

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
        /// Initializes the order list window and loads order data.
        /// </summary>
        public OrderListWindow()
        {
            InitializeComponent();
            OrderList = s_bl.Orders.GetAll();
            DataContext = this;
        }

        /// <summary>
        /// Handles changes in the status filter selection.
        /// </summary>
        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => queryOrderList();

        /// <summary>
        /// Refreshes the order list according to the selected status filter.
        /// </summary>
        private void queryOrderList()
        {
            OrderList =
                (SelectedStatus == null || SelectedStatus == BO.OrderStatus.All)
                ? s_bl.Orders.GetAll()!
                : s_bl.Orders
                      .GetAll()!
                      .Where(o => o.OrderStatus == SelectedStatus);
        }

        /// <summary>
        /// Refreshes the order list when changes occur in the business layer.
        /// </summary>
        private void orderListObserver() => queryOrderList();

        /// <summary>
        /// Registers the observer when the window is loaded.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Orders.AddObserver(orderListObserver);

        /// <summary>
        /// Unregisters the observer when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Orders.RemoveObserver(orderListObserver);

        /// <summary>
        /// Opens the order details window for the selected order.
        /// </summary>
        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedOrder == null)
                return;

            var win = new OrderDetailsWindow(SelectedOrder.OrderId);
            win.ShowDialog();
        }

        /// <summary>
        /// Opens the order details window in create mode.
        /// </summary>
        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var win = new OrderDetailsWindow(0);
            win.ShowDialog();
        }

        /// <summary>
        /// Displays a message indicating that deleting orders is not supported.
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
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
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn ||
                btn.DataContext is not OrderInList order)
                return;

            try
            {
                s_bl.Orders.Cancel(order.OrderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot cancel order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            e.Handled = true;
        }

        /// <summary>
        /// Prevents mouse interaction from propagating further.
        /// </summary>
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
