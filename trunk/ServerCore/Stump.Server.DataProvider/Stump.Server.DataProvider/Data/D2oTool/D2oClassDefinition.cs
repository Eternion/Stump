// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Reflection;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    [DebuggerDisplay("Name = {Name}")]
    public class D2OClassDefinition
    {
        private static readonly Dictionary<Type, Type> m_toleratedType = new Dictionary<Type, Type>
            {
                {typeof (uint), typeof (int)},
                {typeof (int), typeof (uint)},
            };

        private readonly D2OFile m_file; // depending file, don't use without it !!

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, AccessorBuilder.SetFieldValueUnboundDelegate>> m_fieldsAccessors = new ConcurrentDictionary<Type, ConcurrentDictionary<string, AccessorBuilder.SetFieldValueUnboundDelegate>>();
        private static readonly ConcurrentDictionary<Type, Func<object>> m_typesConstructors = new ConcurrentDictionary<Type, Func<object>>();


        public D2OClassDefinition(int id, string classname, string packagename, BigEndianReader reader, D2OFile file)
        {
            Id = id;
            Name = classname;
            PackageName = packagename;
            m_file = file;

            InitFields(reader);
            CheckClass();
        }

        public Dictionary<string, D2OFieldDefinition> Fields
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string PackageName
        {
            get;
            set;
        }

        public Type ClassType
        {
            get;
            set;
        }

        private void InitFields(BigEndianReader reader)
        {
            int fieldscount = reader.ReadInt();
            Fields = new Dictionary<string, D2OFieldDefinition>(fieldscount);
            for (var i = 0; i < fieldscount; i++)
            {
                string fieldname = reader.ReadUTF();
                int fieldtype = reader.ReadInt();

                Fields.Add(fieldname, new D2OFieldDefinition(fieldname, fieldtype, reader, m_file));
            }
        }

        private void CheckClass()
        {
            ClassType =
                typeof (AttributeAssociatedFile).Assembly.GetType(typeof (AttributeAssociatedFile).Namespace + "." + Name);

            if (ClassType == null)
                throw new Exception(string.Format("Unknown class \'{0}\'", Name));

            var fieldsinfo = ClassType.GetFields().ToList();

            // check each fields
            foreach (var field in Fields)
            {
                if (fieldsinfo.Count(entry => entry.Name == field.Key) > 0)
                {
                    FieldInfo nativeField = fieldsinfo.Where(entry => entry.Name == field.Key).First();

                    if (field.Value.KnowType != typeof (object) &&
                        field.Value.KnowType != typeof (List<object>))

                        if (field.Value.KnowType != nativeField.FieldType &&
                            (!m_toleratedType.ContainsKey(field.Value.KnowType) ||
                             m_toleratedType[field.Value.KnowType] != nativeField.FieldType))
                            throw new Exception(
                                string.Format("Uncorresponding type : {0} must be of type {1} instead of {2}",
                                              field.Key, field.Value.KnowType.Name, nativeField.FieldType.Name));
                }
                else
                    throw new Exception(string.Format("Missed field \'{0}\'", field.Key));
            }
        }

        private static IList BuildList(object obj, Type expectedType)
        {
            if (!(obj is List<object>))
                return null;

            Type convertType = expectedType.GetGenericArguments()[0];

            Type objType = typeof (List<>).MakeGenericType(convertType);
            var objList = (IList) Assembly.GetAssembly(objType).CreateInstance(objType.FullName);

            foreach (object item in (List<object>) obj)
            {
                if (item is List<object>)
                    objList.Add(BuildList(item, convertType));
                else
                {
                    if (item is IConvertible)
                        objList.Add(Convert.ChangeType(item, convertType));
                    else
                    {
                        objList.Add(item);
                    }
                }
            }
            return objList;
        }

        internal T BuildClassObject<T>(BigEndianReader reader)
        {
            return (T) BuildClassObject(reader, typeof (T));
        }

        internal object BuildClassObject(BigEndianReader reader, Type objType)
        {
            if (!m_typesConstructors.ContainsKey(objType))
            {
                var ctor = objType.GetConstructor(Type.EmptyTypes);
                m_typesConstructors.TryAdd(objType, ctor.CreateDelegate<object>());
            }

            object result = m_typesConstructors[objType]();

            foreach (var field in Fields)
            {
                var obj = field.Value.ReadValue<object>(reader);

                if (!m_fieldsAccessors.ContainsKey(ClassType))
                    m_fieldsAccessors.TryAdd(ClassType, new ConcurrentDictionary<string, AccessorBuilder.SetFieldValueUnboundDelegate>());

                if (!m_fieldsAccessors[ClassType].ContainsKey(field.Key))
                {
                    var fieldInfo = ClassType.GetField(field.Key);
                    m_fieldsAccessors[ClassType].TryAdd(field.Key, AccessorBuilder.CreateSetter(ClassType, fieldInfo.FieldType, field.Key));
                }

                if (obj is List<object>)
                {
                    IList objList = BuildList(obj, ClassType.GetField(field.Key).FieldType);

                    m_fieldsAccessors[ClassType][field.Key](result, objList);
                }
                else
                {
                    if (field.Value.KnowType != ClassType.GetField(field.Key).FieldType)
                        obj = Convert.ChangeType(obj, ClassType.GetField(field.Key).FieldType);

                    m_fieldsAccessors[ClassType][field.Key](result, obj);
                }
            }

            return result;
        }

        /// <summary>
        ///   Read a defined field from a class by his index
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "index"></param>
        /// <param name = "field"></param>
        /// <returns></returns>
        public T ReadValue<T>(int index, string field)
        {
            using (var reader = new BigEndianReader(m_file.m_filebuffer))
            {
                if (!m_file.Indexes.ContainsKey(index))
                    return default(T);

                int offset = m_file.Indexes[index];

                reader.Seek(offset, SeekOrigin.Begin);

                return Fields[field].ReadValue<T>(reader);
            }
        }


        /// <summary>
        ///   Read a defined field from a class by his index
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "index"></param>
        /// <param name = "field"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public T ReadValue<T>(int index, string field, T defaultValue)
        {
            using (var reader = new BigEndianReader(m_file.m_filebuffer))
            {
                if (!m_file.Indexes.ContainsKey(index))
                    return defaultValue;

                int offset = m_file.Indexes[index];

                reader.Seek(offset, SeekOrigin.Begin);

                return Fields[field].ReadValue(reader, defaultValue);
            }
        }

        internal T ReadValue<T>(string field, BigEndianReader reader)
        {
            return Fields[field].ReadValue<T>(reader);
        }

        internal T ReadValue<T>(string field, BigEndianReader reader, T defaultvalue)
        {
            return Fields[field].ReadValue(reader, defaultvalue);
        }
    }
}