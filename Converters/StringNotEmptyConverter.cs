using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LifeManagementApp.Converters
{
    public sealed class StringNotEmptyConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType,object? parameter,CultureInfo culture)
        {
            return value is string text && !string.IsNullOrWhiteSpace(text);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
