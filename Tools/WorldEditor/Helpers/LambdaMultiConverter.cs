using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorldEditor.Helpers
{
    internal sealed class LambdaMultiConverter<T1, T2, TResult> : IMultiValueConverter
    {
        private readonly Func<T1, T2, TResult> _lambda;
        public LambdaMultiConverter(Func<T1, T2, TResult> lambda)
        {
            _lambda = lambda;
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                throw new InvalidOperationException();
            }
            if (!targetType.IsAssignableFrom(typeof(TResult)))
            {
                throw new InvalidOperationException();
            }
            return _lambda((T1)((object)values[0]), (T2)((object)values[1]));
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
