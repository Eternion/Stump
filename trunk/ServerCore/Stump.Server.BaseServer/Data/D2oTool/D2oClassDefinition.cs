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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.BaseServer.Data.D2oTool
{
    [DebuggerDisplay("Name = {Name}")]
    public class D2oClassDefinition
    {
        private readonly D2oFile m_file; // depending file, don't use without it !!

        public D2oClassDefinition(int id, string classname, string packagename, BigEndianReader reader, D2oFile file)
        {
            Id = id;
            Name = classname;
            PackageName = packagename;
            Fields = new Dictionary<string, D2oFieldDefinition>();
            m_file = file;

            InitFields(reader);
            CheckClass();
        }

        public Dictionary<string, D2oFieldDefinition> Fields
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
            for (int i = 0; i < fieldscount; i++)
            {
                string fieldname = reader.ReadUTF();
                int fieldtype = reader.ReadInt();

                Fields.Add(fieldname, new D2oFieldDefinition(fieldname, fieldtype, reader, m_file));
            }
        }

        private void CheckClass()
        {
            ClassType = Type.GetType(typeof (AttributeAssociatedFile).Assembly.GetName().Name + "." + Name, false);

            if (ClassType == null)
                throw new Exception(string.Format("Unknown class \'{0}\'", Name));


            List<FieldInfo> fieldsinfo = ClassType.GetFields().ToList();


            // check each fields
            foreach (var field in Fields)
            {
                if (fieldsinfo.Count(entry => entry.Name == field.Key) > 0)
                {
                    FieldInfo nativeField = fieldsinfo.Where(entry => entry.Name == field.Key).First();

                    if (field.Value.KnowType != typeof (object) &&
                        field.Value.KnowType != typeof (List<object>))

                        if (field.Value.KnowType != nativeField.FieldType)
                            throw new Exception(
                                string.Format("Uncorresponding type : {0} must be of type {1} instead of {2}",
                                              field.Key, field.Value.KnowType.Name, nativeField.FieldType.Name));
                }
                else
                    throw new Exception(string.Format("Missed field \'{0}\'", field.Key));
            }
        }

        internal T BuildClassObject<T>(BigEndianReader reader)
        {
            // call constructor
            var result = (T) typeof (T).GetConstructor(new Type[0]).Invoke(new object[0]);

            Type resultType = typeof (T);

            foreach (var field in Fields)
            {
                var obj = field.Value.ReadValue<object>(reader);

                // can manage only 2 dimensions
                if (obj is List<object>)
                {
                    Type convertType = ClassType.GetField(field.Key).FieldType.GetGenericArguments()[0];
                    Type listType = typeof (List<>).MakeGenericType(convertType);
                    var objList = (IList) Assembly.GetAssembly(listType).CreateInstance(listType.FullName);

                    foreach (object item in (List<object>) obj)
                    {
                        if (item is List<object>)
                        {
                            Type _convertType = convertType.GetGenericArguments()[0];
                            Type _listType = typeof (List<>).MakeGenericType(_convertType);
                            var _obj_list = (IList) Assembly.GetAssembly(_listType).CreateInstance(_listType.FullName);

                            foreach (object _item in (List<object>) item)
                            {
                                if (_item is IConvertible)
                                    _obj_list.Add(Convert.ChangeType(_item, _convertType));
                                else
                                    _obj_list.Add(_item);
                            }

                            objList.Add(_obj_list);
                        }
                        else
                        {
                            if (item is IConvertible)
                                objList.Add(Convert.ChangeType(item, convertType));
                            else
                                objList.Add(item);
                        }
                    }

                    ClassType.GetField(field.Key).SetValue(result, objList);
                }
                else
                    ClassType.GetField(field.Key).SetValue(result, obj);
            }

            return result;
        }

        internal object BuildClassObject(Type objType, BigEndianReader reader)
        {
            // call constructor
            object result = objType.GetConstructor(new Type[0]).Invoke(new object[0]);

            Type resultType = objType;

            foreach (var field in Fields)
            {
                var obj = field.Value.ReadValue<object>(reader);

                // can manage only 2 dimensions
                if (obj is List<object>)
                {
                    Type convertType = ClassType.GetField(field.Key).FieldType.GetGenericArguments()[0];
                    Type listType = typeof (List<>).MakeGenericType(convertType);
                    var obj_list = (IList) Assembly.GetAssembly(listType).CreateInstance(listType.FullName);

                    foreach (object item in (List<object>) obj)
                    {
                        if (item is List<object>)
                        {
                            Type _convertType = convertType.GetGenericArguments()[0];
                            Type _listType = typeof (List<>).MakeGenericType(_convertType);
                            var _obj_list = (IList) Assembly.GetAssembly(_listType).CreateInstance(_listType.FullName);

                            foreach (object _item in (List<object>) item)
                            {
                                if (_item is IConvertible)
                                    _obj_list.Add(Convert.ChangeType(_item, _convertType));
                                else
                                    _obj_list.Add(_item);
                            }

                            obj_list.Add(_obj_list);
                        }
                        else
                        {
                            if (item is IConvertible)
                                obj_list.Add(Convert.ChangeType(item, convertType));
                            else
                                obj_list.Add(item);
                        }
                    }

                    ClassType.GetField(field.Key).SetValue(result, obj_list);
                }
                else
                    ClassType.GetField(field.Key).SetValue(result, obj);
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
            using (var reader = new BigEndianReader(m_file.StreamReader))
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
            using (var reader = new BigEndianReader(m_file.StreamReader))
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