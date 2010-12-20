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
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.BaseServer.Data.D2oTool
{
    public class D2oFieldDefinition
    {
        public const int NullIdentifier = -1431660000; // wtf ?!

        private readonly D2oFile m_file;

        private readonly List<Func<BigEndianReader, int, object>> m_tempVectorMethod;
        private readonly List<Type> m_tempVectorType;

        public D2oFieldDefinition(string name, int type, BigEndianReader reader, D2oFile file)
        {
            Name = name;
            FieldType = type;
            m_file = file;
            m_tempVectorMethod = new List<Func<BigEndianReader, int, object>>();
            m_tempVectorType = new List<Type>();

            AssociatedMethod = GetMethodByType(type, reader);
            KnowType = GetTypeById(type);
        }

        public string Name
        {
            get;
            set;
        }

        public int FieldType
        {
            get;
            set;
        }

        public Type KnowType
        {
            get;
            set;
        }

        public Func<BigEndianReader, int, object> AssociatedMethod
        {
            get;
            set;
        }

        internal T ReadValue<T>(BigEndianReader reader)
        {
            try
            {
                object obj = AssociatedMethod.DynamicInvoke(reader, 0);

                if (obj is IConvertible)
                    return (T) Convert.ChangeType(obj, typeof (T));
                else
                    return (T) obj;
            }
            catch
            {
                return default(T);
            }
        }

        internal T ReadValue<T>(BigEndianReader reader, T defaultvalue)
        {
            try
            {
                object obj = AssociatedMethod.DynamicInvoke(reader, 0);

                if (obj is IConvertible)
                    return (T) Convert.ChangeType(obj, typeof (T));
                else
                    return (T) obj;
            }
            catch
            {
                return defaultvalue;
            }
        }

        internal Type GetTypeById(int typeid)
        {
            switch (typeid)
            {
                case -1:
                    return typeof (int);

                case -2:
                    return typeof (bool);

                case -3:
                    return typeof (string);

                case -4:
                    return typeof (double);

                case -5:
                    return typeof (int);

                case -6:
                    return typeof (uint);

                case -99:
                    return typeof (List<object>);

                default:
                    return typeof (object);
            }
        }

        internal Func<BigEndianReader, int, object> GetMethodByType(int typeid, BigEndianReader reader)
        {
            switch (typeid)
            {
                case -1:
                    return ReadInt;
                case -2:
                    return ReadBool;
                case -3:
                    return ReadUTF;
                case -4:
                    return ReadDouble;
                case -5:
                    return ReadI18n;
                case -6:
                    return ReadUInt;
                case -99:
                    // found Array type
                    /*string vectorTypename = */reader.ReadUTF(); // useless
                    int vectorTypeid = reader.ReadInt();

                    m_tempVectorMethod.Insert(0, GetMethodByType(vectorTypeid, reader));
                    m_tempVectorType.Insert(0, GetTypeById(vectorTypeid));

                    return ReadVector;
                default:
                    return ReadObject;
            }
        }

        private object ReadVector(BigEndianReader reader, int vectorDimension)
        {
            int count = reader.ReadInt();
            var result = new List<object>();

            for (int i = 0; i < count; i++)
            {
                result.Add(m_tempVectorMethod[vectorDimension].DynamicInvoke(reader, vectorDimension + 1));
            }


            return result;
        }

        private object ReadObject(BigEndianReader reader, int vectorDimension)
        {
            int classid = reader.ReadInt();

            if (classid == NullIdentifier)
                return null;

            if (m_file.Classes.Keys.Contains(classid))
                return m_file.Classes[classid].BuildClassObject(reader, m_file.Classes[classid].ClassType);
            else
                return null;
        }

        private static object ReadInt(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadInt();
        }

        private static object ReadUInt(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadUInt();
        }

        private static object ReadBool(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadByte() != 0;
        }

        private static object ReadUTF(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadUTF();
        }

        private static object ReadDouble(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadDouble();
        }

        private static object ReadI18n(BigEndianReader reader, int vectorDimension)
        {
            return reader.ReadInt();
        }
    }
}