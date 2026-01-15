using BlApi;
using System;
using System.Windows;

namespace PL.Delivery
{
    /// <summary>
    /// Provides a window for viewing and updating delivery details,
    /// including delivery status management.
    /// </summary>
    public partial class DeliveryAddUpdateWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Gets or sets the delivery currently being edited.
        /// </summary>
        public BO.DeliveryPerOrderInList CurrentDelivery { get; set; } =
            new BO.DeliveryPerOrderInList();

        /// <summary>
        /// Gets the text displayed on the main action button.
        /// </summary>
        public string ButtonText =>
            CurrentDelivery.DeliveryId == 0 ? "Add Delivery" : "Update Delivery";

        /// <summary>
        /// Initializes the window in add or update mode
        /// according to the provided delivery identifier.
        /// </summary>
        public DeliveryAddUpdateWindow(int id)
        {
            InitializeComponent();

            CurrentDelivery =
                id == 0
                ? new BO.DeliveryPerOrderInList()
                : s_bl.Deliveries.Get(id)!;

            DataContext = this;
        }

        /// <summary>
        /// Saves the delivery changes by updating the delivery status.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentDelivery.DeliveryId == 0)
                {
                    MessageBox.Show(
                        "Delivery can only be created from an Order.",
                        "Operation not allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Deliveries.UpdateStatus(
                        CurrentDelivery.DeliveryId,
                        CurrentDelivery.CompletionStatus!.Value);
                }

                Close();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Could not save delivery details. Please check the data and try again.",
                    "Operation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
