using System;
using System.Globalization;
using System.Windows.Data;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Helpers.Converters
{
    public class IdToI18NTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return I18NDataManager.Instance.ReadText((int)value);
            }
            else if (value is uint)
            {
                return I18NDataManager.Instance.ReadText((uint)value);
            }
            else
            {
                return "{null}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
