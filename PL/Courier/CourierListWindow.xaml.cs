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

namespace PL.Courier
{
    /// <summary>
    /// Displays list of couriers.
    /// Uses dependency property binding and reads data from BL.
    /// </summary>
    public partial class CourierListWindow : Window
    {

        // Access to BL layer
        static readonly IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<CourierInList> CourierList
        {
            get => (IEnumerable<CourierInList>)GetValue(CourierListProperty);
            set => SetValue(CourierListProperty, value);
        }

        public static readonly DependencyProperty CourierListProperty =
            DependencyProperty.Register(
                nameof(CourierList),
                typeof(IEnumerable<CourierInList>),
                typeof(CourierListWindow),
                new PropertyMetadata(null));
        public CourierListWindow()
        {
            InitializeComponent();

            CourierList = s_bl.Couriers.GetAll();

        }
    }
}
