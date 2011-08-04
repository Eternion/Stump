using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.Extensions;

namespace Stump.Tools.CacheManager.SQL
{
    public class SqlBuilder
    {
        public static string BuildDelete(string table, string whereClause = "")
        {
            var builder = new StringBuilder();

            builder.Append("DELETE ");
            builder.Append("FROM ");
            builder.Append(table);

            if (!string.IsNullOrEmpty(whereClause))
            {
                builder.Append(" WHERE ");
                builder.Append(whereClause);
            }

            return builder.ToString();
        }

        public static string BuildInsertInto(string table, IDictionary<string, object> values)
        {
            var builder = new StringBuilder();

            builder.Append("INSERT INTO ");
            builder.Append(table);
            builder.Append("(");

            foreach (var value in values)
            {
                builder.Append("`");
                builder.Append(value.Key);
                builder.Append("`");

                if (!value.Equals(values.Last()))
                    builder.Append(",");
            }
            builder.Append(")");

            builder.Append(" VALUES (");

            foreach (var value in values)
            {
                if (value.Value is byte[])
                {
                    builder.Append("0x" + ((byte[])value.Value).ByteArrayToString());
                }
                else
                {
                    builder.Append("'");
                    builder.Append(value.Value.ToString().Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("‘", "\\‘"));
                    builder.Append("'");
                }

                if (!value.Equals(values.Last()))
                    builder.Append(",");
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}