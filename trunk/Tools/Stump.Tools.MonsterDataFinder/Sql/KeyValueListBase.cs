using System.Collections.Generic;

namespace Stump.Tools.MonsterDataFinder.Sql
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

        public void AddPair(string key, object value)
        {
            Pairs.Add(new KeyValuePair<string, object>(key, value));
        }
    }

    public class UpdateKeyValueList : KeyValueListBase
    {
        public readonly List<KeyValuePair<string, object>> Where;

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
    }
}