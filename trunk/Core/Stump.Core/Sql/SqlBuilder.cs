using System.Collections.Generic;
using System.Text;

namespace Stump.Core.Sql
{
    public class SqlBuilder
    {
        public static string BuildSelect(string[] columns, string from)
        {
            return BuildSelect(columns, from, null);
        }

        public static string BuildSelect(string[] columns, string from, string suffix)
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                sb.Append(QuoteForColumnName(column));
                if (i < columns.Length - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(" FROM ");
            sb.Append(from);

            if (suffix != null)
            {
                sb.Append(" " + suffix);
            }

            return sb.ToString();
        }

        public static string BuildInsert(KeyValueListBase liste)
        {
            var pairs = liste.Pairs;
            var count = pairs.Count;

            // create the basic statement
            var sb = PrepareInsertBuilder(liste);

            // append values
            sb.Append("(");
            for (var i = 0; i < count; i++)
            {
                var pair = pairs[i];
                sb.Append(GetValueString(pair.Value));
                if (i < count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");

            return sb.ToString();
        }

        public static string BuildUpdate(UpdateKeyValueList list)
        {
            return BuildUpdate(list, BuildWhere(list.Where));
        }

        public static string BuildUpdate(KeyValueListBase liste, string where)
        {
            var sb = new StringBuilder();
            sb.Append("UPDATE " + liste.Table + " SET ");
            AppendKeyValuePairs(sb, liste.Pairs, ", ");
            sb.Append(" WHERE ");
            sb.Append(where);

            return sb.ToString();
        }

        public static string BuildDelete(string table, string where = "")
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(table);
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(" WHERE ");
                sb.Append(where);
            }

            return sb.ToString();
        }

        public static string BuildWhere(List<KeyValuePair<string, object>> pairs)
        {
            var sb = new StringBuilder();
            AppendKeyValuePairs(sb, pairs, " AND ");
            return sb.ToString();
        }

        public static void AppendKeyValuePairs(StringBuilder sb, List<KeyValuePair<string, object>> pairs, string connector)
        {
            for (var i = 0; i < pairs.Count; i++)
            {
                var pair = pairs[i];
                sb.Append(QuoteForColumnName(pair.Key) + " = " + GetValueString(pair.Value));
                if (i < pairs.Count - 1)
                {
                    sb.Append(connector);
                }
            }
        }

        private static StringBuilder PrepareInsertBuilder(KeyValueListBase liste)
        {
            var pairs = liste.Pairs;
            var count = pairs.Count;

            var sb = new StringBuilder(150);
            sb.Append("INSERT INTO " + QuoteForTableName(liste.Table) + " (");
            for (var i = 0; i < count; i++)
            {
                var pair = pairs[i];
                sb.Append(QuoteForColumnName(pair.Key));
                if (i < count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(") VALUES ");
            return sb;
        }

        public static string GetValueString(object obj)
        {
            if (obj is RawData)
                return ( (RawData)obj ).Data.ToString();

            return "'" + EscapeField(obj.ToString()) + "'";
        }

        private static string QuoteForTableName(string table)
        {
            return "`" + table + "`";
        }

        private static string QuoteForColumnName(string table)
        {
            return "`" + EscapeField(table) + "`";
        }

        public static string EscapeField(string field)
        {
            return field.Replace("`", "").Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("‘", "\\‘").Replace("@", "@@");
        }

        public static string EscapeValue(string field)
        {
            return field.Replace("'", "\\'").Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("‘", "\\‘").Replace("@", "@@");
        }
    }
}