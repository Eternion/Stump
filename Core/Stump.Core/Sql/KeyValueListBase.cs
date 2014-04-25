using System.Collections.Generic;
using System.Linq;

namespace Stump.Core.Sql
{
    public class KeyValueListBase
    {
        public readonly string Table;

        public readonly List<KeyValuePair<string, object>> Pairs;
        public KeyValueListBase(string table)
        {
            Table = table;
            Pairs = new List<KeyValuePair<string, object>>();
        }

        public KeyValueListBase(string table, List<KeyValuePair<string, object>> pairs)
            : this(table)
        {
            Pairs = pairs;
        }

        public KeyValueListBase(string table, IEnumerable<KeyValuePair<string, object>> pairs)
            : this(table)
        {
            Pairs = pairs.ToList();
        }


        public void AddPair(string key, object value)
        {
            Pairs.Add(new KeyValuePair<string, object>(key, value));
        }
    }

    public class UpdateKeyValueList : KeyValueListBase
    {
        public List<KeyValuePair<string, object>> Where;

        public UpdateKeyValueList(string table)
            : base(table)
        {
            Where = new List<KeyValuePair<string, object>>();
        }

        public UpdateKeyValueList(string table, List<KeyValuePair<string, object>> where)
            : this(table)
        {
            Where = where;
        }

        public UpdateKeyValueList(string table, IEnumerable<KeyValuePair<string, object>> where)
            : this(table)
        {
            Where = where.ToList();
        }

        public void AddWherePair(string key, object value)
        {
            Where.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}