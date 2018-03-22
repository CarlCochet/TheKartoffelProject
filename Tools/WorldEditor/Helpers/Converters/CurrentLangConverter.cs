using System;
using System.Globalization;
using System.Windows.Data;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Helpers.Converters
{
    public class CurrentLangConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LangText)
            {
                return (value as LangText).GetText(I18NDataManager.Instance.DefaultLanguage);
            }
            else if (value is LangTextUi)
            {
                return (value as LangTextUi).GetText(I18NDataManager.Instance.DefaultLanguage);
            }
            else
            {
                return "{not a valid lang record}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
