using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorldEditor.Helpers
{
    public static class LambdaBinding
    {
        public static Binding New<TSource, TResult>(Binding binding, Func<TSource, TResult> lambda, Func<TResult, TSource> lambdaBack = null)
        {
            binding.Converter = new LambdaConverter<TSource, TResult>(lambda, lambdaBack);
            return binding;
        }

        public static MultiBinding New<T1, T2, TResult>(Binding b1, Binding b2, Func<T1, T2, TResult> lambda)
        {
            return new MultiBinding
            {
                Bindings = 
				{
					b1,
					b2
				},
                Converter = new LambdaMultiConverter<T1, T2, TResult>(lambda)
            };
        }
    }
}
