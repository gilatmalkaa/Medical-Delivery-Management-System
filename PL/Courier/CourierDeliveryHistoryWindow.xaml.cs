using BlApi;
using BO;
using System.Collections.Generic;
using System.Windows;

namespace PL.Courier
{
    /// <summary>
    /// Displays the delivery history of a specific courier,
    /// including all completed deliveries.
    /// </summary>
    public partial class CourierDeliveryHistoryWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// Gets the list of closed deliveries associated with the courier.
        /// </summary>
        public IEnumerable<ClosedDeliveryInList> ClosedDeliveries { get; }

        /// <summary>
        /// Initializes the window and loads the delivery history
        /// for the specified courier.
        /// </summary>
        public CourierDeliveryHistoryWindow(int courierId)
        {
            InitializeComponent();
            ClosedDeliveries = s_bl.Deliveries.GetClosedDeliveriesByCourier(courierId);
            DataContext = this;
        }
    }
}
