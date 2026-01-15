using BlApi;
using BO;
using System.Collections.ObjectModel;
using System.Windows;

namespace PL.Courier;


/// <summary>
/// Provides a window that allows a courier to choose and assign
/// an available order based on eligibility and distance constraints.
/// </summary>
public partial class ChooseOrderWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    /// <summary>
    /// Gets the collection of open orders available for assignment.
    /// </summary>
    public ObservableCollection<OpenOrderInList> OpenOrders { get; }

    /// <summary>
    /// Stores the identifier of the current courier.
    /// </summary>
    private int _courierId;

    /// <summary>
    /// Initializes the window and loads available orders
    /// for the specified courier.
    /// </summary>
    public ChooseOrderWindow(int courierId)
    {
        InitializeComponent();

        _courierId = courierId;

        var courier = s_bl.Couriers.Get(courierId);
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

        OpenOrders = new ObservableCollection<OpenOrderInList>(
             s_bl.Couriers.GetOpenOrdersForCourier(courierId));

        DataContext = this;
    }

    /// <summary>
    /// Assigns the selected order to the courier
    /// and closes the window upon success.
    /// </summary>
    private void BtnAssign_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext
            is not OpenOrderInList order)
            return;

        try
        {
            s_bl.Couriers.AssignOrder(_courierId, order.OrderId);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Cannot assign order");
        }
    }
}
