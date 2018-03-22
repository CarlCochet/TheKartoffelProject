using System;
using System.Globalization;
using System.Windows.Data;
using WorldEditor.Loaders.Icons;

namespace WorldEditor.Helpers.Converters
{
    public class IdToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)value;
            Icon icon = IconsManager.Instance.GetIcon(id);
            return icon.Image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
