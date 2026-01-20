using BlApi;
using BO;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Courier
{
    /// <summary>
    /// Provides a window that allows a courier to choose and assign
    /// an available order based on eligibility and distance constraints.
    /// </summary>
    public partial class ChooseOrderWindow : Window
    {
        /// <summary>
        /// Business layer access.
        /// </summary>
        private static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// Gets the collection of open orders available for assignment.
        /// </summary>
        public ObservableCollection<OpenOrderInList> OpenOrders { get; }
            = new();

        /// <summary>
        /// Stores the identifier of the current courier.
        /// </summary>
        private readonly int _courierId;

        /// <summary>
        /// Initializes the window.
        /// Actual data loading is performed asynchronously on load.
        /// </summary>
        public ChooseOrderWindow(int courierId)
        {
            InitializeComponent();

            _courierId = courierId;
            DataContext = this;

            Loaded += ChooseOrderWindow_Loaded;
        }

        /// <summary>
        /// Loads courier availability and open orders asynchronously.
        /// </summary>
        private async void ChooseOrderWindow_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                var courier = await Task.Run(() =>
                    s_bl.Couriers.Get(_courierId));

                if (!courier.IsAvailable)
                {
                    MessageBox.Show(
                        "Courier already has an active delivery",
                        "Cannot choose order",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    Close();
                    return;
                }

                var orders = await Task.Run(() =>
                    s_bl.Couriers.GetOpenOrdersForCourier(_courierId));

                OpenOrders.Clear();
                foreach (var order in orders)
                    OpenOrders.Add(order);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to load orders",
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
        /// Assigns the selected order to the courier
        /// and closes the window upon success.
        /// </summary>
        private async void BtnAssign_Click(
            object sender,
            RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext
                is not OpenOrderInList order)
                return;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() =>
                    s_bl.Couriers.AssignOrder(
                        _courierId,
                        order.OrderId));

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot assign order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
