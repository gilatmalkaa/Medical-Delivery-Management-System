using BlApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    /// 

    public partial class OrderDetailsWindow : Window
    {
        public int Id { get; set; }
        public string ButtonText { get; set; }

        static readonly IBl s_bl = BlApi.Factory.Get();

        private readonly Action _orderObserver;


        public BO.Order? CurrentOrder
        {
            get => (BO.Order?)GetValue(CurrentOrderProperty);
            set => SetValue(CurrentOrderProperty, value);
        }

        public static readonly DependencyProperty CurrentOrderProperty =
            DependencyProperty.Register(
                nameof(CurrentOrder),
                typeof(BO.Order),
                typeof(OrderDetailsWindow),
                new PropertyMetadata(null));

        public OrderDetailsWindow(int id)
        {
            InitializeComponent();
            DataContext = this;

            _orderObserver = () =>
            {
                if (CurrentOrder != null)
                    CurrentOrder = s_bl.Orders.Get(CurrentOrder.Id); 
            };

            if (id == 0)
            {
                CurrentOrder = new BO.Order();
                ButtonText = "Add";
            }
            else
            {
                CurrentOrder = s_bl.Orders.Get(id);
                ButtonText = "Update";

                if (CurrentOrder!.Id != 0)
                    s_bl.Orders.AddObserver(CurrentOrder.Id, _orderObserver);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (CurrentOrder != null && CurrentOrder.Id != 0)
                s_bl.Orders.RemoveObserver(CurrentOrder.Id, _orderObserver);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonText == "Add")
                    s_bl.Orders.Create(CurrentOrder);  
                else
                    s_bl.Orders.Update(CurrentOrder);  

                MessageBox.Show("Operation completed successfully",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();   
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message,
                    "Validation / Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message,
                    "Validation / Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (BO.BlNullPropertyException ex)
            {
                MessageBox.Show(ex.Message,
                    "Validation / Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}
