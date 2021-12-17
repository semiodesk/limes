﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProfilesRNS_Manager
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }
    }
}
