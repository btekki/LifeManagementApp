using System.Globalization;
using Microsoft.Maui.Controls;

namespace LifeManagementApp.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int priority)
                return Colors.Gray;
            

            return priority switch
            {
                1 => Colors.Red,
                2 => Colors.Yellow,
                3 => Colors.Green,
                _ => Colors.Gray
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}