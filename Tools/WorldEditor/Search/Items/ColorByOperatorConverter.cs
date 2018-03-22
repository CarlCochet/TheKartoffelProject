using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace WorldEditor.Search.Items
{
    public class ColorByOperatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string @operator = value as string;
            if (@operator == null)
            {
                return Brushes.Black;
            }
            else if (@operator == "+")
            {
                return Brushes.Green;
            }
            else if (@operator == "-")
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
