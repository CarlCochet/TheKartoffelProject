using System;
using System.Globalization;
using System.Windows.Data;
namespace WorldEditor.Helpers.Converters
{
    public class IsGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = (int)value;
            int comp = (int)System.Convert.ChangeType(parameter, typeof(int));
            return i > comp;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
