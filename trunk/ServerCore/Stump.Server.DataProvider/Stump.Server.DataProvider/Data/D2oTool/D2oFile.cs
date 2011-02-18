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
using System.IO;
using System.Linq;
using System.Text;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data.D2oTool;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class D2OFile
    {
        private readonly Dictionary<int, D2OClassDefinition> m_classes = new Dictionary<int, D2OClassDefinition>();
        private readonly Dictionary<int, int> m_indextable = new Dictionary<int, int>();
        private int m_classcount;
        private byte[] m_filebuffer;
        private int m_headeroffset;
        private int m_indextablelen;
        private BigEndianReader m_reader;

        /// <summary>
        ///   Create and initialise a new D2o file
        /// </summary>
        /// <param name = "name">Path of the file</param>
        public D2OFile(string name)
        {
            FilePath = name;
            m_classes = new Dictionary<int, D2OClassDefinition>();

            Init();
        }

        internal Stream StreamReader
        {
            get { return m_reader.BaseStream; }
        }

        public string FilePath
        {
            get;
            set;
        }

        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        public int IndexCount
        {
            get { return m_indextable.Count; }
        }

        public Dictionary<int, D2OClassDefinition> Classes
        {
            get { return m_classes; }
        }

        public Dictionary<int, int> Indexes
        {
            get { return m_indextable; }
        }

        private void Init()
        {
            m_filebuffer = File.ReadAllBytes(FilePath);
            m_reader = new BigEndianReader(new MemoryStream(m_filebuffer));

            string header = Encoding.Default.GetString(m_reader.ReadBytes(3));

            if (header != "D2O")
            {
                throw new Exception("Header doesn't equal the string \'D2O\' : Corrupted file");
            }

            m_headeroffset = m_reader.ReadInt();
            m_reader.Seek(m_headeroffset, SeekOrigin.Begin); // place the reader at the beginning of the indextable
            m_indextablelen = m_reader.ReadInt();

            // init table index
            for (int i = 0; i < m_indextablelen; i += 8)
            {
                m_indextable.Add(m_reader.ReadInt(), m_reader.ReadInt());
            }

            // init classes
            m_classcount = m_reader.ReadInt();
            for (int i = 0; i < m_classcount; i++)
            {
                int classId = m_reader.ReadInt();

                string classMembername = m_reader.ReadUTF();
                string classPackagename = m_reader.ReadUTF();

                m_classes.Add(classId, new D2OClassDefinition(classId, classMembername, classPackagename, m_reader, this));
            }
        }

        /// <summary>
        ///   Get all objects that corresponding to T associated to his index
        /// </summary>
        /// <typeparam name = "T">Corresponding class</typeparam>
        /// <param name = "allownulled">True to adding null instead of throwing an exception</param>
        /// <returns></returns>
        public Dictionary<int, T> ReadObjects<T>(bool allownulled = false)
        {
            if (typeof(T).GetCustomAttributes(false) == null ||
                typeof(T).GetCustomAttributes(false).Count(entry =>
                                                            entry is AttributeAssociatedFile &&
                                                            (entry as AttributeAssociatedFile).FilesName.Contains(
                                                                FileName)) == 0)
                throw new Exception("Targeted class hasn't correct AttributeAssociatedFile");

            var result = new Dictionary<int, T>(m_indextable.Count);

            using (var reader = new BigEndianReader(m_filebuffer))
            {
                foreach (var index in m_indextable)
                {
                    reader.Seek(index.Value, SeekOrigin.Begin);

                    int classid = reader.ReadInt();

                    if (m_classes[classid].ClassType == typeof(T) ||
                        m_classes[classid].ClassType.IsSubclassOf(typeof(T)))
                    {
                        try
                        {
                            result.Add(index.Key, m_classes[classid].BuildClassObject<T>(reader));
                        }
                        catch
                        {
                            if (allownulled)
                                result.Add(index.Key, default(T));
                            else
                                throw;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///   Get all objects that corresponding to T associated to his index
        /// </summary>
        /// <param name = "allownulled">True to adding null instead of throwing an exception</param>
        /// <returns></returns>
        public Dictionary<int, object> ReadObjects(bool allownulled = false)
        {
            var result = new Dictionary<int, object>(m_indextable.Count);

            using (var reader = new BigEndianReader(m_filebuffer))
            {

                foreach (var index in m_indextable)
                {
                    reader.Seek(index.Value, SeekOrigin.Begin);

                    var classid = reader.ReadInt();

                    try
                    {
                        result.Add(index.Key, m_classes[classid].BuildClassObject(reader, m_classes[classid].ClassType));
                    }
                    catch
                    {
                        if (allownulled)
                            result.Add(index.Key, null);
                        else
                            throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///   Get an object from his index
        /// </summary>
        /// <param name = "index"></param>
        /// <returns></returns>
        public object ReadObject(int index)
        {
            using (var reader = new BigEndianReader(m_filebuffer))
            {
                var offset = m_indextable[index];
                reader.Seek(offset, SeekOrigin.Begin);

                var classid = reader.ReadInt();

                var result = m_classes[classid].BuildClassObject(reader, m_classes[classid].ClassType);

                return result;
            }
        }

        /// <summary>
        ///   Get an object from his index
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "index"></param>
        /// <returns></returns>
        public T ReadObject<T>(int index)
        {
            if (typeof(T).GetCustomAttributes(false) == null ||
                typeof(T).GetCustomAttributes(false).Count(entry =>
                                                            entry is AttributeAssociatedFile &&
                                                            (entry as AttributeAssociatedFile).FilesName.Contains(
                                                                FileName)) == 0)
                throw new Exception("Targeted class hasn't correct AttributeAssociatedFile");


            using (var reader = new BigEndianReader(m_filebuffer))
            {
                var offset = m_indextable[index];
                reader.Seek(offset, SeekOrigin.Begin);

                var classid = reader.ReadInt();

                if (m_classes[classid].ClassType != typeof(T) &&
                    !m_classes[classid].ClassType.IsSubclassOf(typeof(T)))
                    throw new Exception(string.Format("Wrong type, try to read object with {1} instead of {0}",
                                                      typeof(T).Name, m_classes[classid].ClassType.Name));

                var result = m_classes[classid].BuildClassObject<T>(reader);

                return result;
            }
        }

        public Dictionary<int, D2OClassDefinition> GetClasses()
        {
            return m_indextable.ToDictionary(index => index.Key, index => GetClass(index.Key));
        }

        public D2OClassDefinition GetClass(int index)
        {
            using (var reader = new BigEndianReader(m_filebuffer))
            {
                int offset = m_indextable[index];
                reader.Seek(offset, SeekOrigin.Begin);

                int classid = reader.ReadInt();

                return m_classes[classid];
            }
        }
    }
}