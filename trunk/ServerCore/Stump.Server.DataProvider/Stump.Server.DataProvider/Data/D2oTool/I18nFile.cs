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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class I18NFile : IDisposable
    {
        private Dictionary<int, int> m_indexTable;
        private Dictionary<string, int> m_textTable = new Dictionary<string, int>(500);
        private readonly string m_filePath;
        private BigEndianReader m_reader;

        public string FilePath
        {
            get { return m_filePath; }
        }

        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        public I18NFile(string path)
        {
            m_filePath = path;

            Init();
        }

        private void Init()
        {
            m_reader = new BigEndianReader(File.ReadAllBytes(FilePath));

            var headeroffset = m_reader.ReadInt();

            m_reader.Seek(headeroffset, SeekOrigin.Begin);

            var indexlen = m_reader.ReadInt();
            m_indexTable = new Dictionary<int, int>(indexlen / 8);

            for (int i = 0; i < indexlen; i += 8)
            {
                int index = m_reader.ReadInt();
                int offset = m_reader.ReadInt();

                if (m_indexTable.ContainsKey(index))
                {
                    Debug.WriteLine(index + " already set"); // it don't throw an exception
                    continue;
                }

                m_indexTable.Add(index, offset);
            }

            while (m_reader.BaseStream.Position < m_reader.BaseStream.Length)
            {
                m_textTable.Add(m_reader.ReadUTF(), m_reader.ReadInt());
            }
        }

        public string ReadText(int index)
        {
            if (!m_indexTable.ContainsKey(index))
                return "{undefined}";

            m_reader.Seek(m_indexTable[index], SeekOrigin.Begin);

            return m_reader.ReadUTF();
        }

        public string ReadUiText(string nameIndex)
        {
            if (!m_textTable.ContainsKey(nameIndex))
                return "{undefined}";

            m_reader.Seek(m_textTable[nameIndex], SeekOrigin.Begin);

            return m_reader.ReadUTF();
        }

        public Dictionary<int, string> ReadAllText()
        {
            return m_indexTable.ToDictionary(index => index.Key, index => ReadText(index.Key));
        }

        public Dictionary<string, string> ReadAllUiText()
        {
            return m_textTable.ToDictionary(index => index.Key, index => ReadUiText(index.Key));
        }

        public bool Exists(int index)
        {
            return m_indexTable.ContainsKey(index);
        }

        public bool ExistsUi(string nameIndex)
        {
            return m_textTable.ContainsKey(nameIndex);
        }

        public void Dispose()
        {
            m_indexTable = null;
            m_textTable = null;
            m_reader.Dispose();
            m_reader = null;
        }
    }
}