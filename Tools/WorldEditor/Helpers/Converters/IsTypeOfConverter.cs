using System;
using System.Globalization;
using System.Windows.Data;

namespace WorldEditor.Helpers.Converters
{
    public class IsTypeOfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            if (value == null || parameter == null || !(parameter is Type))
            {
                result = false;
            }
            else
            {
                result = (parameter as Type).IsInstanceOfType(value);
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}