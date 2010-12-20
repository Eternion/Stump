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

namespace Stump.Server.BaseServer.Data.D2oTool
{
    public class I18nFile : IDisposable
    {
        private readonly Dictionary<int, int> m_indextable = new Dictionary<int, int>();
        private readonly Dictionary<string, int> m_texttable = new Dictionary<string, int>();
        private Stream m_stream;
        private BigEndianReader m_reader;


        public I18nFile(string name)
        {
            FilePath = name;

            Init();
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

        private void Init()
        {
            m_stream = new MemoryStream(File.ReadAllBytes(FilePath));

            m_reader = new BigEndianReader(m_stream);

            int headeroffset = m_reader.ReadInt();
            m_reader.Seek(headeroffset, SeekOrigin.Begin);

            int indexlen = m_reader.ReadInt();
            for (int i = 0; i < indexlen; i += 8)
            {
                int index = m_reader.ReadInt();
                int offset = m_reader.ReadInt();

                if (m_indextable.ContainsKey(index))
                {
                    Debug.WriteLine(index + " already set"); // it don't throw an exception
                    continue;
                }

                m_indextable.Add(
                    index,
                    offset);
            }

            while (m_reader.BaseStream.Position < m_reader.BaseStream.Length)
            {
                m_texttable.Add(
                    m_reader.ReadUTF(),
                    m_reader.ReadInt());
            }
        }

        public void Close()
        {
            m_stream.Close();
        }

        public string ReadText(int index)
        {
            if (!m_indextable.ContainsKey(index))
                return "";

            m_reader.Seek(m_indextable[index], SeekOrigin.Begin);

            return m_reader.ReadUTF();
        }

        public string ReadUIText(string nameindex)
        {
            if (!m_texttable.ContainsKey(nameindex))
                return "";

            m_reader.Seek(m_texttable[nameindex], SeekOrigin.Begin);

            return m_reader.ReadUTF();
        }

        public Dictionary<int, string> ReadAllText()
        {
            return m_indextable.ToDictionary(index => index.Key, index => ReadText(index.Key));
        }

        public Dictionary<string, string> ReadAllUIText()
        {
            return m_texttable.ToDictionary(index => index.Key, index => ReadUIText(index.Key));
        }

        public bool Exists(int index)
        {
            return m_indextable.ContainsKey(index);
        }

        public bool ExistsUI(string nameindex)
        {
            return m_texttable.ContainsKey(nameindex);
        }

        public void Dispose()
        {
            Close();
        }
    }
}