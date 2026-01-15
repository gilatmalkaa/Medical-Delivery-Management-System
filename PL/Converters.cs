using BO;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL
{
    /// <summary>
    /// Converts a button text value to a boolean indicating update mode.
    /// </summary>
    public class ConvertUpdateToTrue : IValueConverter
    {
        /// <summary>
        /// Returns true if the provided value equals "Update".
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string)value == "Update";

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a button text value to a Visibility value
    /// indicating update mode.
    /// </summary>
    public class ConvertUpdateToVisible : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the provided value equals "Update",
        /// otherwise returns Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (string)value == "Update"
                ? Visibility.Visible
                : Visibility.Collapsed;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a boolean value to a Visibility value.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the value is true, otherwise Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a null value to Visibility.Visible
    /// and a non-null value to Visibility.Collapsed.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the value is null, otherwise Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a non-null value to Visibility.Visible
    /// and a null value to Visibility.Collapsed.
    /// </summary>
    public class NotNullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the value is not null, otherwise Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts an identifier value to Visibility based on whether it is non-zero.
    /// </summary>
    public class IdToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the identifier is non-zero, otherwise Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && id != 0)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts an identifier value to a read-only boolean flag.
    /// </summary>
    public class IdToReadOnlyConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if the identifier is non-zero, otherwise false.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && id != 0)
                return true;

            return false;
        }

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a positive integer value to Visibility.Visible
    /// and zero or negative values to Visibility.Collapsed.
    /// </summary>
    public class IntToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns Visible if the integer value is greater than zero.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count && count > 0)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts an order status to a boolean indicating
    /// whether the order is open.
    /// </summary>
    public class OrderOpenOnlyConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if the order status is Created.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not OrderStatus status)
                return false;

            return status == OrderStatus.Created;
        }

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a null value to true and a non-null value to false.
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if the value is null.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Converts a non-null value to true and a null value to false.
    /// </summary>
    public class NotNullToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if the value is not null.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        /// <summary>
        /// Conversion back is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
