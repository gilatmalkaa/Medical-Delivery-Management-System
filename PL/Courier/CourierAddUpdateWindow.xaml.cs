using BlApi;
using BO;
using PL.Helpers;
using System;
using System.Windows;

namespace PL.Courier
{
    /// <summary>
    /// Provides a window for adding a new courier or updating
    /// an existing courier's details.
    /// </summary>
    public partial class CourierAddUpdateWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Indicates whether the window is in add mode.
        /// </summary>
        public bool IsAddMode { get; }

        public bool IsAdmin { get; }

        /// <summary>
        /// Gets or sets the courier currently being edited.
        /// </summary>
        public BO.Courier CurrentCourier { get; set; }

        /// <summary>
        /// Gets the text displayed on the main action button.
        /// </summary>
        public string ButtonText =>
            IsAddMode ? "Add Courier" : "Update Courier";

        /// <summary>
        /// Initializes the window in add or update mode
        /// according to the provided courier identifier.
        /// </summary>
        public CourierAddUpdateWindow(int id)
        {
            InitializeComponent();

            IsAddMode = id == 0;

            CurrentCourier = IsAddMode
                ? new BO.Courier()
                : s_bl.Couriers.Get(id)!;

            CourierTypeCombo.ItemsSource =

                Enum.GetValues(typeof(DeliveryType));
                IsAdmin = SessionManager.Role == UserRole.Admin;

            DataContext = this;
        }

        /// <summary>
        /// Saves the courier by creating a new record
        /// or updating an existing one.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAddMode)
                    s_bl.Couriers.Create(CurrentCourier);
                else
                    s_bl.Couriers.Update(CurrentCourier);

                MessageBox.Show(
                    "Courier saved successfully",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close(); 
            }
            catch (BlInvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input");
            }
            catch (BlAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "Already Exists");
            }
            catch (BlNullPropertyException ex)
            {
                MessageBox.Show(ex.Message, "Missing Data");
            }
        }


        /// <summary>
        /// Deletes the current courier after user confirmation.
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsAddMode)
                return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this courier?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                s_bl.Couriers.Delete(CurrentCourier.Id);

                MessageBox.Show(
                    "Courier deleted successfully",
                    "Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Delete Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
