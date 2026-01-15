using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Courier
{
    /// <summary>
    /// Displays and manages a list of couriers,
    /// supporting filtering, selection, and CRUD operations.
    /// </summary>
    public partial class CourierListWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Gets or sets the list of couriers displayed in the window.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the currently selected courier type filter.
        /// </summary>
        public BO.CourierType? SelectedType { get; set; } = null;

        /// <summary>
        /// Gets or sets the currently selected courier.
        /// </summary>
        public CourierInList? SelectedCourier { get; set; }

        /// <summary>
        /// Initializes the courier list window and loads courier data.
        /// </summary>
        public CourierListWindow()
        {
            InitializeComponent();
            CourierList = s_bl.Couriers.GetAll();
            DataContext = this;
        }

        /// <summary>
        /// Refreshes the courier list according to the selected filter.
        /// </summary>
        private void queryList()
        {
            var list = s_bl.Couriers.GetAll();
            CourierList = SelectedType == null
                ? list
                : list.Where(c => c.Type == SelectedType);
        }

        /// <summary>
        /// Handles changes in the filter selection and updates the list accordingly.
        /// </summary>
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selected = comboBox?.SelectedItem;

            var list = s_bl.Couriers.GetAll();

            if (selected == null || selected is string)
            {
                CourierList = list;
            }
            else
            {
                CourierList = list.Where(c => c.Type == (CourierType)selected);
            }
        }

        /// <summary>
        /// Refreshes the UI when courier data changes.
        /// </summary>
        private void observerRefresh()
        {
            Dispatcher.Invoke(queryList);
        }

        /// <summary>
        /// Registers an observer when the window is loaded.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Couriers.AddObserver(observerRefresh);

        /// <summary>
        /// Unregisters the observer when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Couriers.RemoveObserver(observerRefresh);

        /// <summary>
        /// Opens the add/update window when a courier is double-clicked.
        /// </summary>
        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid &&
                grid.SelectedItem is CourierInList courier)
            {
                new CourierAddUpdateWindow(courier.Id).ShowDialog();
            }
        }


        /// <summary>
        /// Opens the add courier window.
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
            => new CourierAddUpdateWindow(0).ShowDialog();

        /// <summary>
        /// Deletes the selected courier after user confirmation.
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCourier == null)
            {
                MessageBox.Show("Please select a courier first.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete courier #{SelectedCourier.Id}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                s_bl.Couriers.Delete(SelectedCourier.Id);
            }
            catch (BO.BlPermissionException ex)
            {
                MessageBox.Show(ex.Message, "Delete not allowed");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete courier.");
            }
        }
    }
}
