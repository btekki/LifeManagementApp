using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LifeManagementApp.Converters
{
    public sealed class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool booleanValue && !booleanValue;
        }

        public object ConvertBack(object? value,Type targetType,object? parameter,CultureInfo culture)
        {
            return value is bool booleanValue && !booleanValue;
        }
    }
}
