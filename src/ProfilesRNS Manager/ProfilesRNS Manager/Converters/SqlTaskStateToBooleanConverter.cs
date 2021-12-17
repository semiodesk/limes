using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProfilesRNS_Manager
{
    [ValueConversion(typeof(SqlTaskState), typeof(bool))]
    public class SqlTaskStateToBooleanConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SqlTaskState state = (SqlTaskState)value;

            if(state == SqlTaskState.Success)
            {
                return true;
            }
            else if(state == SqlTaskState.Error)
            {
                return null;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
