using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Helpers.Extentions
{
    public static class CollectionExtensions
    {
        public static T GetOrDefault<T>(this IList<T> list, int index)
        {
            return index >= list.Count ? default(T) : list[index];
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue val;
            return dict.TryGetValue(key, out val) ? val : default(TValue);
        }
    }
}
