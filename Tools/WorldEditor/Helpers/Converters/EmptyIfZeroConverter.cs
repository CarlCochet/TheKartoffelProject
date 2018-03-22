using System;
using System.Globalization;
using System.Windows.Data;

namespace WorldEditor.Helpers.Converters
{
    public class EmptyIfZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (System.Convert.ToInt32(value) == 0) ? string.Empty : value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
