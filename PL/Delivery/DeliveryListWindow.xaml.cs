using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Delivery
{
    /// <summary>
    /// Displays and manages a list of deliveries,
    /// supporting filtering, selection, and status updates.
    /// </summary>
    public partial class DeliveryListWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Gets or sets the list of deliveries displayed in the window.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the currently selected delivery.
        /// </summary>
        public DeliveryPerOrderInList? SelectedDelivery { get; set; }

        /// <summary>
        /// Gets or sets the selected delivery status filter.
        /// </summary>
        public BO.DeliveryStatus? SelectedStatus { get; set; }

        /// <summary>
        /// Initializes the delivery list window and loads delivery data.
        /// </summary>
        public DeliveryListWindow()
        {
            InitializeComponent();
            DeliveryList = s_bl.Deliveries.ReadAll();
            DataContext = this;
        }

        /// <summary>
        /// Refreshes the delivery list according to the selected status filter.
        /// </summary>
        private void RefreshList()
        {
            DeliveryList = s_bl.Deliveries.ReadAll(
                d => SelectedStatus == null || d.CompletionStatus == SelectedStatus);
        }

        /// <summary>
        /// Handles changes in the delivery status filter.
        /// </summary>
        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        /// <summary>
        /// Opens the delivery update window on double-click
        /// and refreshes the list after closing.
        /// </summary>
        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedDelivery is null)
                return;

            new DeliveryAddUpdateWindow(SelectedDelivery.DeliveryId).ShowDialog();
            RefreshList();
        }

        /// <summary>
        /// Opens the delivery update window for the selected delivery.
        /// Creation of new deliveries is not allowed from this screen.
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDelivery is null)
            {
                MessageBox.Show(
                    "A delivery cannot be created from this screen.\nSelect a delivery to manage it.",
                    "Stage 6 restriction",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            new DeliveryAddUpdateWindow(SelectedDelivery.DeliveryId).ShowDialog();
            RefreshList();
        }
    }
}
