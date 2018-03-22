using System.Collections.Generic;
using System.Text;

namespace WorldEditor.Helpers.Sql
{
    public class SqlBuilder
    {
        public static string BuildSelect(string[] columns, string from)
        {
            return SqlBuilder.BuildSelect(columns, from, null);
        }

        public static string BuildSelect(string[] columns, string from, string suffix)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            for (int i = 0; i < columns.Length; i++)
            {
                string table = columns[i];
                stringBuilder.Append(SqlBuilder.QuoteForColumnName(table));
                if (i < columns.Length - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(" FROM ");
            stringBuilder.Append(from);
            if (suffix != null)
            {
                stringBuilder.Append(" " + suffix);
            }
            return stringBuilder.ToString();
        }

        public static string BuildInsert(KeyValueListBase liste)
        {
            List<KeyValuePair<string, object>> pairs = liste.Pairs;
            int count = pairs.Count;
            StringBuilder stringBuilder = SqlBuilder.PrepareInsertBuilder(liste);
            stringBuilder.Append("(");
            for (int i = 0; i < count; i++)
            {
                stringBuilder.Append(SqlBuilder.GetValueString(pairs[i].Value));
                if (i < count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        public static string BuildUpdate(UpdateKeyValueList list)
        {
            return SqlBuilder.BuildUpdate(list, SqlBuilder.BuildWhere(list.Where));
        }

        public static string BuildUpdate(KeyValueListBase liste, string where)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE " + liste.Table + " SET ");
            SqlBuilder.AppendKeyValuePairs(stringBuilder, liste.Pairs, ", ");
            stringBuilder.Append(" WHERE ");
            stringBuilder.Append(where);
            return stringBuilder.ToString();
        }

        public static string BuildDelete(string table, string where = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DELETE FROM ");
            stringBuilder.Append(table);
            if (!string.IsNullOrEmpty(where))
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(where);
            }
            return stringBuilder.ToString();
        }

        public static string BuildWhere(List<KeyValuePair<string, object>> pairs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SqlBuilder.AppendKeyValuePairs(stringBuilder, pairs, " AND ");
            return stringBuilder.ToString();
        }

        public static void AppendKeyValuePairs(StringBuilder sb, List<KeyValuePair<string, object>> pairs, string connector)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                KeyValuePair<string, object> keyValuePair = pairs[i];
                sb.Append(SqlBuilder.QuoteForColumnName(keyValuePair.Key) + " = " + SqlBuilder.GetValueString(keyValuePair.Value));
                if (i < pairs.Count - 1)
                {
                    sb.Append(connector);
                }
            }
        }

        private static StringBuilder PrepareInsertBuilder(KeyValueListBase liste)
        {
            List<KeyValuePair<string, object>> pairs = liste.Pairs;
            int count = pairs.Count;
            StringBuilder stringBuilder = new StringBuilder(150);
            stringBuilder.Append("INSERT INTO " + SqlBuilder.QuoteForTableName(liste.Table) + " (");
            for (int i = 0; i < count; i++)
            {
                stringBuilder.Append(SqlBuilder.QuoteForColumnName(pairs[i].Key));
                if (i < count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(") VALUES ");
            return stringBuilder;
        }

        public static string GetValueString(object obj)
        {
            string result;
            if (obj is RawData)
            {
                result = ((RawData)obj).Data.ToString();
            }
            else
            {
                result = "'" + SqlBuilder.EscapeField(obj.ToString()) + "'";
            }
            return result;
        }

        private static string QuoteForTableName(string table)
        {
            return "`" + table + "`";
        }

        private static string QuoteForColumnName(string table)
        {
            return "`" + SqlBuilder.EscapeField(table) + "`";
        }

        public static string EscapeField(string field)
        {
            return field.Replace("`", "").Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("‘", "\\‘").Replace("@", "@@");
        }

        public static string EscapeValue(string field)
        {
            return field.Replace("'", "\\'").Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("‘", "\\‘").Replace("@", "@@");
        }
    }
}