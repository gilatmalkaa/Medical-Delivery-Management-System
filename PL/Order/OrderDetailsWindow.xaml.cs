using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Order
{
    /// <summary>
    /// Provides a window for creating a new order or
    /// viewing and updating an existing order.
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        /// <summary>
        /// Business logic facade used for order-related operations.
        /// </summary>
        private static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Holds the identifier of the order being displayed or edited.
        /// A value of 0 indicates creation mode.
        /// </summary>
        private readonly int _orderId;

        /// <summary>
        /// Gets the order currently being displayed or edited.
        /// </summary>
        public BO.Order CurrentOrder { get; private set; }

        /// <summary>
        /// Indicates whether the window is in update mode.
        /// </summary>
        public bool IsUpdateMode => _orderId != 0;

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
        /// Initializes the window.
        /// Data loading is performed asynchronously on load.
        /// </summary>
        public OrderDetailsWindow(int id)
        {
            InitializeComponent();

            _orderId = id;
            DataContext = this;

            Loaded += OrderDetailsWindow_Loaded;
        }

        /// <summary>
        /// Loads order data asynchronously.
        /// </summary>
        private async void OrderDetailsWindow_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (_orderId == 0)
                {
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
                    CurrentOrder = await Task.Run(() =>
                        s_bl.Orders.Get(_orderId));
                }

                DataContext = null;
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to load order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Close();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Saves the order by creating or updating it.
        /// </summary>
        private async void BtnSave_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() =>
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
                });

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
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Cancels the current order if allowed.
        /// </summary>
        private async void BtnCancelOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() =>
                    s_bl.Orders.Cancel(CurrentOrder.Id));

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
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
