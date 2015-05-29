#region License GNU GPL

// D2IFile.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tools.D2i
{
    public class D2IFile
    {
        private readonly Dictionary<int, string> m_indexes = new Dictionary<int, string>();
        private readonly Dictionary<string, string> m_textIndexes = new Dictionary<string, string>();
        private readonly string m_uri;

        public D2IFile(string uri)
        {
            m_uri = uri;
            if (File.Exists(m_uri))
                Initialize();
        }

        public string FilePath
        {
            get { return m_uri; }
        }

        private void Initialize()
        {
            using (var reader = new FastBigEndianReader(File.ReadAllBytes(m_uri)))
            {
                int indexPos = reader.ReadInt();
                reader.Seek(indexPos, SeekOrigin.Begin);
                int indexLen = reader.ReadInt();

                for (int i = 0; i < indexLen; i += 8)
                {
                    int key = reader.ReadInt();
                    int dataPos = reader.ReadInt();
                    var pos = (int) reader.Position;
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    m_indexes.Add(key, reader.ReadUTF());
                    reader.Seek(pos, SeekOrigin.Begin);
                }

                while (reader.BytesAvailable > 0)
                {
                    string key = reader.ReadUTF();
                    int dataPos = reader.ReadInt();
                    var pos = (int) reader.Position;
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    m_textIndexes.Add(key, reader.ReadUTF());
                    reader.Seek(pos, SeekOrigin.Begin);
                }
            }
        }

        public string GetText(int id)
        {
            if (m_indexes.ContainsKey(id))
                return m_indexes[id];
            return "{null}";
        }

        public string GetText(string id)
        {
            if (m_textIndexes.ContainsKey(id))
                return m_textIndexes[id];
            return "{null}";
        }

        public void SetText(int id, string value)
        {
            if (m_indexes.ContainsKey(id))
                m_indexes[id] = value;
            else
                m_indexes.Add(id, value);
        }

        public void SetText(string id, string value)
        {
            if (m_textIndexes.ContainsKey(id))
                m_textIndexes[id] = value;
            else
                m_textIndexes.Add(id, value);
        }

        public bool DeleteText(int id)
        {
            return m_indexes.Remove(id);
        }

        public bool DeleteText(string id)
        {
            return m_textIndexes.Remove(id);
        }

        public Dictionary<int, string> GetAllText()
        {
            return m_indexes;
        }

        public Dictionary<string, string> GetAllUiText()
        {
            return m_textIndexes;
        }

        public int FindFreeId()
        {
            return m_indexes.Keys.Max() + 1;
        }

        public void Save()
        {
            Save(m_uri);
        }

        public void Save(string uri)
        {
            using (var writer = new BigEndianWriter(new StreamWriter(uri).BaseStream))
            {
                var indexTable = new BigEndianWriter();
                writer.Seek(4, SeekOrigin.Begin);

                foreach (var index in m_indexes.Where(x => x.Value != null))
                {
                    indexTable.WriteInt(index.Key);
                    indexTable.WriteInt((int) writer.Position);
                    writer.WriteUTF(index.Value);
                }

                var indexLen = (int) indexTable.Position;

                foreach (var index in m_textIndexes.Where(x => x.Value != null))
                {
                    indexTable.WriteUTF(index.Key);
                    indexTable.WriteInt((int) writer.Position);
                    writer.WriteUTF(index.Value);
                }

                var indexPos = (int) writer.Position;

                /* write index at end */
                byte[] indexData = indexTable.Data;
                writer.WriteInt(indexLen);
                writer.WriteBytes(indexData);

                /* write index pos at begin */
                writer.Seek(0, SeekOrigin.Begin);
                writer.WriteInt(indexPos);
            }
        }
    }
}