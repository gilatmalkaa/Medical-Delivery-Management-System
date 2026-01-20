using BlApi;
using BO;
using PL.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Courier
{
    /// <summary>
    /// Provides a window for adding a new courier or updating
    /// an existing courier's details.
    /// </summary>
    public partial class CourierAddUpdateWindow : Window
    {
        /// <summary>
        /// Business logic facade used for courier operations.
        /// </summary>
        private static readonly IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Identifier of the courier being edited.
        /// Value 0 indicates add mode.
        /// </summary>
        private readonly int _courierId;

        /// <summary>
        /// Indicates whether the window is in add mode.
        /// </summary>
        public bool IsAddMode => _courierId == 0;

        /// <summary>
        /// Determines whether the courier type field is editable.
        /// Editing is allowed only when adding a new courier
        /// and the current user has admin privileges.
        /// </summary>
        public bool CanEditCourierType =>
            IsAddMode && IsAdmin;


        /// <summary>
        /// Indicates whether the current user is an admin.
        /// </summary>
        public bool IsAdmin
        {
            get => (bool)GetValue(IsAdminProperty);
            set => SetValue(IsAdminProperty, value);
        }

        /// <summary>
        /// Dependency property backing store for IsAdmin.
        /// </summary> 
        /// 
        public static readonly DependencyProperty IsAdminProperty =
            DependencyProperty.Register(
                nameof(IsAdmin),
                typeof(bool),
                typeof(CourierAddUpdateWindow),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the courier currently being edited.
        /// </summary>
        public BO.Courier CurrentCourier
        {
            get => (BO.Courier)GetValue(CurrentCourierProperty);
            set => SetValue(CurrentCourierProperty, value);
        }

        /// <summary>
        /// Dependency property backing store for CurrentCourier.
        /// </summary>
        public static readonly DependencyProperty CurrentCourierProperty =
            DependencyProperty.Register(
                nameof(CurrentCourier),
                typeof(BO.Courier),
                typeof(CourierAddUpdateWindow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the text displayed on the main action button.
        /// </summary>
        public string ButtonText =>
            IsAddMode ? "Add Courier" : "Update Courier";

        /// <summary>
        /// Initializes the window.
        /// </summary>
        public CourierAddUpdateWindow(int id)
        {
            InitializeComponent();

            _courierId = id;
            IsAdmin = SessionManager.Role == UserRole.Admin;

            CourierTypeCombo.ItemsSource =
                Enum.GetValues(typeof(BO.CourierType));

            Loaded += CourierAddUpdateWindow_Loaded;

            DataContext = this;
        }

        /// <summary>
        /// Loads courier data asynchronously.
        /// </summary>
        private async void CourierAddUpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Courier courier;

                Mouse.OverrideCursor = Cursors.Wait;

                if (IsAddMode)
                {
                    courier = new BO.Courier { IsActive = true };
                }
                else
                {
                    courier = await Task.Run(() => s_bl.Couriers.Get(_courierId));
                }

                CurrentCourier = courier;
            }
            catch (BO.BLTemporaryNotAvailableException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Simulator is running",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Close();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Executes an asynchronous function while displaying a wait cursor
        /// and returns its result.
        /// </summary>
        private async Task<T> RunWithWaitCursorAsync<T>(Func<Task<T>> action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                return await action();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }



        /// <summary>
        /// Saves the courier by creating or updating it.
        /// </summary>
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                BO.Courier _courier = CurrentCourier;
                bool _isAdd = IsAddMode;

                await Task.Run(() =>
                {
                    if (_isAdd)
                        s_bl.Couriers.Create(_courier);
                    else
                        s_bl.Couriers.Update(_courier);
                });

                MessageBox.Show("Courier saved successfully", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (BlInvalidInputException ex) { MessageBox.Show(ex.Message, "Invalid Input"); }
            catch (BlAlreadyExistsException ex) { MessageBox.Show(ex.Message, "Already Exists"); }
            catch (BlNullPropertyException ex) { MessageBox.Show(ex.Message, "Missing Data"); }
            finally { Mouse.OverrideCursor = null; }
        }


        /// <summary>
        /// Deletes the current courier after confirmation.
        /// </summary>
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsAddMode) return;

            if (MessageBox.Show("Are you sure you want to delete this courier?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                int _id = CurrentCourier.Id;

                await Task.Run(() => s_bl.Couriers.Delete(_id));

                MessageBox.Show("Courier deleted successfully", "Deleted",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { Mouse.OverrideCursor = null; }
        }


        /// <summary>
        /// Runs an async action with a wait cursor.
        /// </summary>
        private async Task RunWithWaitCursorAsync(Func<Task> action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try { await action(); }
            finally { Mouse.OverrideCursor = null; }
        }
    }
}
