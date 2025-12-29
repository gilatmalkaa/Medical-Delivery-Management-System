using BlApi;
using BO;
using DO;
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

namespace PL.Delivery
{
    /// <summary>
    /// Interaction logic for DeliveryListWindow.xaml
    /// </summary>
    public partial class DeliveryListWindow : Window
    {

        // Access to BL layer
        static readonly IBl s_bl = BlApi.Factory.Get();
        private int _orderId;
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
        public DeliveryListWindow()
        {
            InitializeComponent();
            DeliveryList = s_bl.Deliveries.ReadAll();  

        }
    }
}
