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

    public class ConvertUpdateToTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string)value == "Update";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class ConvertUpdateToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string)value == "Update"
                ? Visibility.Visible
                : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }


    public partial class OrderDetailsWindow : Window
    {
        public int Id { get; set; }
        public string ButtonText { get; set; }

        static readonly IBl s_bl = BlApi.Factory.Get();


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

            if (id == 0)
            {
                CurrentOrder = new BO.Order();
                ButtonText = "Add";
            }
            else
            {
                CurrentOrder = s_bl.Orders.Get(id);
                ButtonText = "Update";
            }
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
            catch (BO.BlException ex)
            {
                MessageBox.Show(ex.Message,
                                "Validation / Business Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error:\n" + ex.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

    }
}
