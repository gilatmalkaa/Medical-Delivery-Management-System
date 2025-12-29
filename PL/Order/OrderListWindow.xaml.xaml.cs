using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Order
{

    /// <summary>
    /// Interaction logic for OrderListWindow.xaml
    /// </summary>
    public partial class OrderListWindow : Window
    {
        // Access to BL layer
        static readonly IBl s_bl = BlApi.Factory.Get();
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


        // ===== Selected filters and selected row =====
        public BO.OrderStatus? SelectedStatus { get; set; } = null;
        public OrderInList? SelectedOrder { get; set; }
        public OrderListWindow()
        {
            
            InitializeComponent();
            OrderList = s_bl.Orders.GetAll();
            DataContext = this;

        }
        // ===== Filtering =====
        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => queryOrderList();

        private void queryOrderList()
        {
            OrderList =
                (SelectedStatus == null || SelectedStatus == BO.OrderStatus.All)
                ? s_bl.Orders.GetAll()!
                : s_bl.Orders
                      .GetAll()!
                      .Where(o => o.OrderStatus == SelectedStatus);
        }

        // ===== Observer =====
        private void orderListObserver() => queryOrderList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Orders.AddObserver(orderListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Orders.RemoveObserver(orderListObserver);

        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedOrder == null)
                return;

            var win = new OrderDetailsWindow(SelectedOrder.OrderId);
            win.ShowDialog();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var win = new OrderDetailsWindow(0);
            win.ShowDialog();
        }
    }
}
