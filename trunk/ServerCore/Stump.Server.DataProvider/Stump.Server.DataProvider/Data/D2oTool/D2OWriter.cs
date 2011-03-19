using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class D2OWriter
    {
        private const int NullIdentifier = unchecked((int)0xAAAAAAAA);

        private Dictionary<int, D2OClassDefinition> m_classes;
        private Dictionary<int, int> m_indexTable;
        private Dictionary<Type, int> m_allocatedClassId = new Dictionary<Type, int>();
        private Dictionary<int, object> m_objects = new Dictionary<int, object>();

        private object m_writingSync = new object();
        private bool m_writing;
        private bool m_needToBeSync;
        private BigEndianWriter m_writer;


        public static void CreateEmptyFile(string path)
        {
            if (File.Exists(path))
                throw new Exception("File already exists, delete before overwrite");

            var writer = new BinaryWriter(File.OpenWrite(path));

            writer.Write("D2O");
            writer.Write((int) writer.BaseStream.Position + 4); // index table offset

            writer.Write(0); // index table len
            writer.Write(0); // class count

            writer.Flush();
            writer.Close();
        }

        public D2OWriter(string filename)
        {
            if (!File.Exists(filename))
                CreateWrite(filename);

            OpenWrite(File.Open(filename, FileMode.Open));
        }

        public D2OWriter(Stream stream)
        {
            OpenWrite(stream);
        }

        private void ResetMembersByReading()
        {
            var reader = new D2OReader(m_writer.BaseStream);

            m_indexTable = reader.Indexes;
            m_classes = reader.Classes;
            m_objects = reader.ReadObjects();
        }

        private void OpenWrite(Stream stream)
        {
            m_writer = new BigEndianWriter(stream);

            ResetMembersByReading();
        }

        private void CreateWrite(string filename)
        {
            m_writer = new BigEndianWriter(File.Create(filename));
            m_indexTable = new Dictionary<int, int>();
            m_classes = new Dictionary<int, D2OClassDefinition>();
            m_objects = new Dictionary<int, object>();
        }

        public void StartWriting()
        {
            m_writing = true;
            lock (m_writingSync)
            {
                if (m_needToBeSync)
                {
                    ResetMembersByReading();
                }
            }
        }

        public void EndWriting()
        {
            lock (m_writingSync)
            {
                m_writing = false;
                m_needToBeSync = false;

                WriteHeader();

                foreach (var obj in m_objects)
                {
                    WriteObject(obj, obj.GetType());
                }
            }
        }

        private void WriteHeader()
        {
            m_writer.WriteUTFBytes("D2O");
            m_writer.WriteInt(0); // allocate space to write the correct index table offset
        }

        public void Write<T>(T obj)
        {
            Write(obj, m_objects.Count > 0 ? m_objects.Keys.Max() + 1 : 0);
        }

        public void Write<T>(T obj, int index)
        {
            if (!m_writing)
                StartWriting();

            lock (m_writingSync)
            {
                m_needToBeSync = true;

                if (!m_allocatedClassId.ContainsKey(typeof (T)))
                    // if the class is not allocated then the class is not defined
                    DefineClassDefinition(typeof (T));

                if (m_objects.ContainsKey(index))
                    m_objects[index] = obj;
                else
                    m_objects.Add(index, obj);
            }
        }

        private int AllocateClassId(Type classType)
        {
            int id = m_allocatedClassId.Count > 0 ? m_allocatedClassId.Values.Max() + 1 : 0;
            AllocateClassId(classType, id);

            return id;
        }

        private void AllocateClassId(Type classType, int classId)
        {
            m_allocatedClassId.Add(classType, classId);
        }

        private void DefineClassDefinition(Type classType)
        {
            if (m_classes.Count(entry => entry.Value.ClassType == (classType)) > 0) // already define
                return;
            
            AllocateClassId(classType);

            object[] attributes = classType.GetCustomAttributes(typeof (D2OClassAttribute), false);

            if (attributes.Length != 1)
                throw new Exception("The given class has no D2OClassAttribute attribute and cannot be wrote");

            string package = ((D2OClassAttribute) attributes[0]).PackageName;

            var fields = (from field in classType.GetFields()
                          let fieldTypeId = GetIdByType(field.FieldType)
                          let vectorTypes = GetVectorTypes(field.FieldType)
                          select new D2OFieldDefinition(field.Name, fieldTypeId, field, -1, vectorTypes));

            m_classes.Add(m_allocatedClassId[classType],
                new D2OClassDefinition(m_allocatedClassId[classType], classType.Name, package, classType, fields, -1));

            DefineAllocatedTypes(); // build class definition of allocated types that aren't define
        }

        private void DefineAllocatedTypes()
        {
            foreach (var allocatedClass in m_allocatedClassId.Where(entry => !m_classes.ContainsKey(entry.Value)))
            {
                DefineClassDefinition(allocatedClass.Key);
            }
        }

        private int GetIdByType(Type fieldType)
        {
            if (fieldType == typeof (int))
                return -1;
            if (fieldType == typeof (bool))
                return -2;
            if (fieldType == typeof (string))
                return -3;
            if (fieldType == typeof (double))
                return -4;
            if (fieldType == typeof (int))
                return -5;
            if (fieldType == typeof (uint))
                return -6;
            if (fieldType.GetGenericTypeDefinition() == typeof (List<>))
                return -99;

            int classId;
            if (m_allocatedClassId.ContainsKey(fieldType))
            {
                classId = AllocateClassId(fieldType);

                m_allocatedClassId.Add(fieldType, classId);
            }

            classId = m_allocatedClassId[fieldType];

            return classId;
        }

        private int[] GetVectorTypes(Type vectorType)
        {
            var ids = new List<int>();

            if (vectorType.IsGenericType)
            {
                Type currentGenericType = vectorType;
                Type[] genericArguments = currentGenericType.GetGenericArguments();

                while (genericArguments.Length > 0)
                {
                    ids.Add(GetIdByType(genericArguments[0]));

                    currentGenericType = genericArguments[0];
                    genericArguments = currentGenericType.GetGenericArguments();
                }
            }

            return ids.ToArray();
        }

        private void WriteObject(object obj, Type type)
        {
            if (!m_allocatedClassId.ContainsKey(obj.GetType()))
                throw new Exception(string.Format("Unexpected object of type {0} (was not registered)", obj.GetType()));

            var @class = m_classes[m_allocatedClassId[type]];

            foreach (var field in @class.Fields)
            {
                object fieldValue = field.Value.FieldInfo.GetValue(obj);

                WriteField(m_writer, field.Value, fieldValue);
            }
        }

        private void WriteField(BigEndianWriter writer, D2OFieldDefinition field, dynamic obj, int vectorDimension = 0)
        {
            switch (field.TypeId)
            {
                case -1:
                    WriteFieldInt(writer, obj);
                    break;
                case -2:
                    WriteFieldBool(writer, obj);
                    break;
                case -3:
                    WriteFieldUTF(writer, obj);
                    break;
                case -4:
                    WriteFieldDouble(writer, obj);
                    break;
                case -5:
                    WriteFieldI18n(writer, obj);
                    break;
                case -6:
                    WriteFieldUInt(writer, obj);
                    break;
                case -99:
                    WriteFieldVector(writer, field, obj, vectorDimension);
                    break;
                default:
                    WriteFieldObject(writer, obj);
                    break;
            }
        }


        private void WriteFieldVector(BigEndianWriter writer, D2OFieldDefinition field, IList list, int vectorDimension)
        {
            writer.WriteInt(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                WriteField(writer, field, field.VectorTypesId[vectorDimension], ++vectorDimension);
            }
        }

        private void WriteFieldObject(BigEndianWriter writer, object obj)
        {
            if (obj == null)
                writer.WriteInt(NullIdentifier);
            else
            {
                if (!m_allocatedClassId.ContainsKey(obj.GetType()))
                    throw new Exception(string.Format("Unexpected object of type {0} (was not registered)", obj.GetType()));

                int classid = m_allocatedClassId[obj.GetType()];
                writer.WriteInt(classid);

                WriteObject(obj, obj.GetType());
            }
        }

        private static void WriteFieldInt(BigEndianWriter writer, int value)
        {
            writer.WriteInt(value);
        }

        private static void WriteFieldUInt(BigEndianWriter writer, uint value)
        {
            writer.WriteUInt(value);
        }

        private static void WriteFieldBool(BigEndianWriter writer, bool value)
        {
            writer.WriteBoolean(value);
        }

        private static void WriteFieldUTF(BigEndianWriter writer, string value)
        {
            writer.WriteUTF(value);
        }

        private static void WriteFieldDouble(BigEndianWriter writer, double value)
        {
            writer.WriteDouble(value);
        }

        private static void WriteFieldI18n(BigEndianWriter writer, int value)
        {
            writer.WriteInt(value);
        }
    }
}