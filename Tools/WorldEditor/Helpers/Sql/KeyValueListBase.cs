using System.Collections.Generic;
using System.Linq;

namespace WorldEditor.Helpers.Sql
{
    public class KeyValueListBase
    {
        public readonly string Table;
        public readonly List<KeyValuePair<string, object>> Pairs;

        public KeyValueListBase(string table)
        {
            this.Table = table;
            this.Pairs = new List<KeyValuePair<string, object>>();
        }

        public KeyValueListBase(string table, List<KeyValuePair<string, object>> pairs)
            : this(table)
        {
            this.Pairs = pairs;
        }

        public KeyValueListBase(string table, IEnumerable<KeyValuePair<string, object>> pairs)
            : this(table)
        {
            this.Pairs = pairs.ToList<KeyValuePair<string, object>>();
        }

        public void AddPair(string key, object value)
        {
            this.Pairs.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}