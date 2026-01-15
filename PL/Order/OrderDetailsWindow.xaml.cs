using BlApi;
using System;
using System.Windows;

namespace PL.Order
{
    /// <summary>
    /// Provides a window for creating a new order or
    /// viewing and updating an existing order.
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Gets the order currently being displayed or edited.
        /// </summary>
        public BO.Order CurrentOrder { get; private set; }

        /// <summary>
        /// Indicates whether the window is in update mode.
        /// </summary>
        public bool IsUpdateMode { get; }

        /// <summary>
        /// Gets the text displayed on the main action button.
        /// </summary>
        public string ButtonText => IsUpdateMode ? "Update" : "Add";

        /// <summary>
        /// Gets the list of available order types.
        /// </summary>
        public Array OrderTypes => Enum.GetValues(typeof(BO.OrderType));

        /// <summary>
        /// Indicates whether the current order can be canceled.
        /// </summary>
        public bool CanCancel =>
            IsUpdateMode &&
            CurrentOrder.OrderStatus is
                BO.OrderStatus.Created or
                BO.OrderStatus.Assigned or
                BO.OrderStatus.InDelivery;

        /// <summary>
        /// Initializes the window in create or update mode
        /// according to the provided order identifier.
        /// </summary>
        public OrderDetailsWindow(int id)
        {
            InitializeComponent();

            if (id == 0)
            {
                IsUpdateMode = false;

                CurrentOrder = new BO.Order
                {
                    Id = 0,
                    CreatedAt = DateTime.Now,
                    OrderStatus = BO.OrderStatus.Created,
                    ScheduleStatus = BO.ScheduleStatus.Scheduled,
                    TimeRemaining = null,
                    Deliveries = new List<BO.DeliveryPerOrderInList>()
                };
            }
            else
            {
                IsUpdateMode = true;
                CurrentOrder = s_bl.Orders.Get(id);
            }

            DataContext = this;
        }

        /// <summary>
        /// Saves the order by creating a new one
        /// or updating an existing order.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsUpdateMode)
                {
                    if (CurrentOrder.OrderStatus != BO.OrderStatus.Created)
                        throw new InvalidOperationException(
                            "Cannot update a closed order");

                    s_bl.Orders.Update(CurrentOrder);
                }
                else
                {
                    s_bl.Orders.Create(CurrentOrder);
                }

                MessageBox.Show(
                    "Operation completed successfully",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cancels the current order if allowed.
        /// </summary>
        private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Orders.Cancel(CurrentOrder.Id);

                MessageBox.Show(
                    "Order canceled successfully",
                    "Canceled",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot cancel order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
