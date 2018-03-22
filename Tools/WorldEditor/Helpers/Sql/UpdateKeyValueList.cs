using System.Collections.Generic;
using System.Linq;

namespace WorldEditor.Helpers.Sql
{
    public class UpdateKeyValueList : KeyValueListBase
    {
        public List<KeyValuePair<string, object>> Where;

        public UpdateKeyValueList(string table)
            : base(table)
        {
            this.Where = new List<KeyValuePair<string, object>>();
        }

        public UpdateKeyValueList(string table, List<KeyValuePair<string, object>> where)
            : this(table)
        {
            this.Where = where;
        }

        public UpdateKeyValueList(string table, IEnumerable<KeyValuePair<string, object>> where)
            : this(table)
        {
            this.Where = where.ToList<KeyValuePair<string, object>>();
        }

        public void AddWherePair(string key, object value)
        {
            this.Where.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}