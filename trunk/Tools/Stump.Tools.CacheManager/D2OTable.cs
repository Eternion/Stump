using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Tools.CacheManager
{
    public class D2OTable
    {
        private static readonly Dictionary<Tuple<Type, string>, FieldInfo> typeFields = new Dictionary<Tuple<Type, string>, FieldInfo>();
        private Dictionary<string, string> m_relations;

        public D2OTable(Type tableType)
        {
            TableType = tableType;
            ClassAttribute = tableType.GetCustomAttribute<D2OClassAttribute>();
            TableName = tableType.GetCustomAttribute<ActiveRecordAttribute>().Table;

            if (ClassAttribute == null)
                throw new Exception("A d2o table must have the D2OClass attribute");

            IEnumerable<D2OTableField> fields = from entry in tableType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                let attribute = entry.GetCustomAttribute<D2OFieldAttribute>()
                                                where attribute != null
                                                select new D2OTableField(entry);

            IEnumerable<D2OTableField> properties = from entry in tableType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                    let attribute = entry.GetCustomAttribute<D2OFieldAttribute>()
                                                    where attribute != null
                                                    select new D2OTableField(entry);

            Fields = (fields.Concat(properties)).ToArray();

            m_relations = DatabaseBuilder.GetNamesRelations(TableType);
        }

        public string TableName
        {
            get;
            set;
        }

        public D2OClassAttribute ClassAttribute
        {
            get;
            set;
        }

        public Type TableType
        {
            get;
            set;
        }

        public D2OTableField[] Fields
        {
            get;
            set;
        }

        public Dictionary<string, object> GenerateRow(object obj)
        {
            var objType = obj.GetType();
            var row = new Dictionary<string, object>();

            foreach (var field in Fields)
            {
                var tuple = Tuple.Create(objType, field.Attribute.FieldName);
                FieldInfo objField;

                lock (typeFields)
                {
                    if (!typeFields.ContainsKey(tuple))
                        typeFields.Add(tuple, objType.GetField(field.Attribute.FieldName));

                    objField = typeFields[tuple];
                }

                if (field.DBAttribute != null && field.DBAttribute.ColumnType == "Serializable")
                    row.Add(m_relations[field.Attribute.FieldName], "0x" + objField.GetValue(obj).ToBinary().ByteArrayToString());
                else
                    row.Add(m_relations[field.Attribute.FieldName], objField.GetValue(obj));
            }

            return row;
        }
    }
}