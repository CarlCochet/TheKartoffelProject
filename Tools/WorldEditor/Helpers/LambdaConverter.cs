using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorldEditor.Helpers
{
    internal sealed class LambdaConverter<TSource, TResult> : IValueConverter
    {
        private readonly Func<TSource, TResult> _lambda;
        private readonly Func<TResult, TSource> _lambdaBack;
        public LambdaConverter(Func<TSource, TResult> lambda, Func<TResult, TSource> lambdaBack)
        {
            _lambda = lambda;
            _lambdaBack = lambdaBack;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(TResult)))
            {
                throw new InvalidOperationException();
            }
            return _lambda((TSource)((object)value));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_lambdaBack == null)
            {
                throw new NotImplementedException();
            }
            if (!targetType.IsAssignableFrom(typeof(TSource)))
            {
                throw new InvalidOperationException();
            }
            return _lambdaBack((TResult)((object)value));
        }
    }
}
