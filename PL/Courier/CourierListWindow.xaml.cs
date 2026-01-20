using BlApi;
using BO;
using PL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <summary>
        /// Business logic facade used for courier operations.
        /// </summary>
        static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Synchronization mutex for clock observer updates (Stage 7).
        /// Prevents concurrent or overlapping UI refreshes.
        /// </summary>
        private readonly ObserverMutex _couriersMutex = new(); // stage 7


        /// <summary>
        /// Gets or sets the list of couriers displayed in the window.
        /// </summary>
        public IEnumerable<CourierInList> CourierList
        {
            get => (IEnumerable<CourierInList>)GetValue(CourierListProperty);
            set => SetValue(CourierListProperty, value);
        }

        /// <summary>
        /// Dependency property backing the CourierList property,
        /// enabling data binding for the couriers list in the UI.
        /// </summary>
        public static readonly DependencyProperty CourierListProperty =
    DependencyProperty.Register(
        nameof(CourierList),
        typeof(IEnumerable<CourierInList>),
        typeof(CourierListWindow),
        new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected courier
        /// in the couriers list.
        /// </summary>
        public CourierInList? SelectedCourier
        {
            get => (CourierInList?)GetValue(SelectedCourierProperty);
            set => SetValue(SelectedCourierProperty, value);
        }

        /// <summary>
        /// Dependency property backing the SelectedCourier property,
        /// used to track the selected courier in the UI.
        /// </summary>
        public static readonly DependencyProperty SelectedCourierProperty =
            DependencyProperty.Register(
                nameof(SelectedCourier),
                typeof(CourierInList),
                typeof(CourierListWindow),
                new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the currently selected courier type filter.
        /// </summary>
        public object? SelectedType { get; set; } = null;


        /// <summary>
        /// Initializes the courier list window and loads courier data.
        /// </summary>
        public CourierListWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Refreshes the courier list according to the selected filter.
        /// </summary>
        private async Task LoadCouriersAsync()
        {
            try
            {
                var list = await Task.Run(() => s_bl.Couriers.GetAll());

                CourierList = SelectedType switch
                {
                    null => list,
                    string => list,
                    BO.CourierType type => list.Where(c => c.Type == type),
                    _ => list
                };
            }
            catch (BO.BLTemporaryNotAvailableException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Simulator is running",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }



        /// <summary>
        /// Handles changes in the filter selection and updates the list accordingly.
        /// </summary>
        private async void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await LoadCouriersAsync();
        }


        /// <summary>
        /// Refreshes the UI when courier data changes.
        /// </summary>
        private void observerRefresh()
        {
            if (_couriersMutex.CheckAndSetLoadInProgressOrRestartRequired())
                return;

            Dispatcher.BeginInvoke(async () =>
            {
                try
                {
                    await LoadCouriersAsync();
                }
                finally
                {
                    if (await _couriersMutex.UnsetLoadInProgressAndCheckRestartRequested())
                        observerRefresh();
                }
            });
        }





        /// <summary>
        /// Registers an observer when the window is loaded.
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_bl.Couriers.AddObserver(observerRefresh);
            s_bl.Admin.AddClockObserver(observerRefresh);
            await LoadCouriersAsync();
        }


        /// <summary>
        /// Unregisters the observer when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        { 
            s_bl.Couriers.RemoveObserver(observerRefresh);
            s_bl.Admin.RemoveClockObserver(observerRefresh);
        }

        /// <summary>
        /// Opens the add/update window when a courier is double-clicked.
        /// </summary>
        private async void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid &&
                grid.SelectedItem is CourierInList courier)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        s_bl.Couriers.Get(courier.Id);
                    });

                    new CourierAddUpdateWindow(courier.Id).ShowDialog();
                }
                catch (BO.BLTemporaryNotAvailableException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Simulator is running",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }



        /// <summary>
        /// Opens the add courier window.
        /// </summary>
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => { });

                new CourierAddUpdateWindow(0).ShowDialog();
            }
            catch (BO.BLTemporaryNotAvailableException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Simulator is running",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Deletes the selected courier after user confirmation.
        /// </summary>
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
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
                if (!SelectedCourier.CanDelete)
                {
                    MessageBox.Show(
                        "Courier has active deliveries and cannot be deleted.",
                        "Operation not allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                int _id = SelectedCourier.Id;   
                await Task.Run(() => s_bl.Couriers.Delete(_id));

            }
            catch (BO.BlPermissionException ex)
            {
                MessageBox.Show(ex.Message, "Delete not allowed");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete courier.");
            }

            await LoadCouriersAsync();


        }
    }
}
