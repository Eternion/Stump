using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.AuthServer.Database;
using Stump.Server.WorldServer.Database;

namespace Stump.Tools.CacheManager
{
    public class D2OTable
    {
        private static readonly Dictionary<Tuple<Type, string>, FieldInfo> typeFields = new Dictionary<Tuple<Type, string>, FieldInfo>();
        private readonly Dictionary<string, string> m_relations;

        public D2OTable(Type tableType)
        {
            TableType = tableType;
            ClassAttribute = tableType.GetCustomAttribute<D2OClassAttribute>();

            if (ClassAttribute == null)
                throw new Exception("A d2o table must have the D2OClass attribute");

            RecordAttribute = tableType.GetCustomAttribute<ActiveRecordAttribute>();

            if (RecordAttribute == null)
                throw new Exception("A d2o table must have the ActiveRecord attribute");

            if (!(tableType.BaseType.IsGenericType && tableType.BaseType.GetGenericTypeDefinition() == typeof (WorldBaseRecord<>)) &&
                !(tableType.BaseType.IsGenericType && tableType.BaseType.GetGenericTypeDefinition() == typeof (AuthBaseRecord<>)))
                Inheritance = tableType.BaseType;

            if (string.IsNullOrEmpty(RecordAttribute.Table) && Inheritance != null)
                TableName = Inheritance.GetCustomAttribute<ActiveRecordAttribute>().Table;
            else
                TableName = RecordAttribute.Table;

            Fields = FindD2OFields(tableType);
            m_relations = DatabaseBuilder.GetNamesRelations(TableType);
        }

        public ActiveRecordAttribute RecordAttribute
        {
            get;
            set;
        }

        public Type Inheritance
        {
            get;
            set;
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

        public D2OField[] Fields
        {
            get;
            set;
        }

        public Dictionary<string, object> GenerateRow(object obj)
        {
            Type objType = obj.GetType();
            var row = new Dictionary<string, object>();

            foreach (D2OField field in Fields)
            {
                Tuple<Type, string> tuple = Tuple.Create(objType, field.D2OAttr.FieldName);
                FieldInfo objField;

                lock (typeFields)
                {
                    if (!typeFields.ContainsKey(tuple))
                    {
                        FieldInfo fieldInfo = objType.GetField(field.D2OAttr.FieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                        if (fieldInfo == null)
                            throw new Exception(string.Format("Field '{0}.{1}' not found", objType.Name, field.D2OAttr.FieldName));

                        typeFields.Add(tuple, fieldInfo);
                    }

                    objField = typeFields[tuple];
                }

                string columnName = field.GetDatabaseFieldName();
                object value;
                object fieldValue = objField.GetValue(obj);

                if (field.DbPropAttr != null && field.DbPropAttr.ColumnType == "Serializable")
                    value = fieldValue.ToBinary();
                else if (fieldValue is bool)
                    value = ((bool) fieldValue) ? 1 : 0;
                else
                    value = fieldValue;

                row.Add(columnName, value);
            }

            if (!string.IsNullOrEmpty(RecordAttribute.DiscriminatorValue))
                row.Add("RecognizerType", RecordAttribute.DiscriminatorValue);

            return row;
        }

        private static D2OField[] FindD2OFields(Type type)
        {
            var result = new List<D2OField>();

            result.AddRange(from entry in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                            let attribute = entry.GetCustomAttribute<D2OFieldAttribute>()
                            where attribute != null
                            select new D2OField(entry));

            result.AddRange(from entry in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                            let attribute = entry.GetCustomAttribute<D2OFieldAttribute>()
                            where attribute != null
                            select new D2OField(entry));

            return result.ToArray();
        }
    }
}