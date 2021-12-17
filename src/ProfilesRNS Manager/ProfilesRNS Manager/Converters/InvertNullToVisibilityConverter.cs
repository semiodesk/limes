using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProfilesRNS_Manager
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class InvertNullToVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
