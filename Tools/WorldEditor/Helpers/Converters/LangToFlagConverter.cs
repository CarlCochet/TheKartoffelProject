using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Helpers.Converters
{
    public class LangToFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            if (!(value is Languages))
            {
                result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/all.png"));
            }
            else
            {
                switch ((Languages)value)
                {
                    case Languages.English:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/gb.png"));
                        break;
                    case Languages.French:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/fr.png"));
                        break;
                    case Languages.German:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/de.png"));
                        break;
                    case Languages.Spanish:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/es.png"));
                        break;
                    case Languages.Italian:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/it.png"));
                        break;
                    case Languages.Japanish:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/jp.png"));
                        break;
                    case Languages.Dutsh:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/nl.png"));
                        break;
                    case Languages.Portugese:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/pt.png"));
                        break;
                    case Languages.Russish:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/ru.png"));
                        break;
                    default:
                        result = new BitmapImage(new Uri("pack://application:,,,/Images/flags/all.png"));
                        break;
                }
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
