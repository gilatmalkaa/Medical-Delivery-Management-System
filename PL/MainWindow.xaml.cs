using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Represents the main entry window of the application,
    /// handling user authentication and navigation to system modules.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Provides access to the business logic layer.
        /// </summary>
        static readonly IBl s_bl = Factory.Get();

        /// <summary>
        /// Initializes the main window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var t = new DeliveryStatusCollection();
        }

        /// <summary>
        /// Handles user login and opens the appropriate main window
        /// based on the authenticated user role.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string id = IdBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter ID and password");
                return;
            }

            try
            {
                var role = s_bl.Admin.Login(id, password);

                Helpers.SessionManager.UserId = id;

                switch (role)
                {
                    case BO.UserRole.Admin:
                        Helpers.SessionManager.Role = BO.UserRole.Admin;
                        new Admin.AdminWindow().Show();
                        break;

                    case BO.UserRole.Courier:
                        Helpers.SessionManager.Role = BO.UserRole.Courier;
                        int courierId = int.Parse(id);
                        new Courier.CourierMainWindow(courierId).Show();
                        break;
                }

                Close();
            }
            catch (BO.BlInvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Login failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
        }

        /// <summary>
        /// Opens the courier management window.
        /// </summary>
        private void OpenCouriers(object sender, RoutedEventArgs e)
        {
            Courier.CourierListWindow w = new Courier.CourierListWindow();
            w.ShowDialog();
        }

        /// <summary>
        /// Opens the order management window.
        /// </summary>
        private void OpenOrders(object sender, RoutedEventArgs e)
        {
            Order.OrderListWindow w = new Order.OrderListWindow();
            w.ShowDialog();
        }

        /// <summary>
        /// Opens the delivery management window.
        /// </summary>
        private void OpenDeliveries(object sender, RoutedEventArgs e)
        {
            Delivery.DeliveryListWindow w = new Delivery.DeliveryListWindow();
            w.ShowDialog();
        }
    }
}
