using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProfilesRNS_Manager
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class SqlTaskStateToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((SqlTaskState)value)
            {
                case SqlTaskState.Executing:
                    return "/Assets/arrow-small.png";
                case SqlTaskState.Success:
                    return "/Assets/check-small.png";
                case SqlTaskState.Error:
                    return "/Assets/x-small.png";
                default:
                    return "/Assets/transparent.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
