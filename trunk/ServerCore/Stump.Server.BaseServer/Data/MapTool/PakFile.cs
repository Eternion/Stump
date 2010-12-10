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
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.BaseServer.Data.MapTool
{
    public class PakFile
    {
        private readonly Dictionary<string, Tuple<int, int>> m_indexes = new Dictionary<string, Tuple<int, int>>();
        private readonly BigEndianReader m_reader;

        public PakFile(string filepath)
        {
            FilePath = filepath;

            m_reader =
                new BigEndianReader(File.Open(filepath, FileMode.Open));

            Init();
        }

        public string FilePath
        {
            get;
            private set;
        }

        private void Init()
        {
            int indextableOffset = m_reader.ReadInt();
            m_reader.Seek(indextableOffset, SeekOrigin.Begin);

            while (m_reader.BaseStream.Length - m_reader.BaseStream.Position > 0)
            {
                string indexname = m_reader.ReadUTF();
                int offset = m_reader.ReadInt();
                int bytescount = m_reader.ReadInt();

                m_indexes.Add(indexname, new Tuple<int, int>(offset, bytescount));
            }
        }

        public void ExtractAll(string dirpath)
        {
            dirpath = Path.GetFullPath(dirpath);

            if (!dirpath.EndsWith("/"))
                if (!dirpath.EndsWith("\\") && dirpath.Contains('\\'))
                    dirpath += "\\";
                else if (dirpath.Contains('/'))
                    dirpath += "/";

            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            foreach (var index in m_indexes)
            {
                if (!Directory.Exists(Path.GetDirectoryName(dirpath + index.Key)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dirpath + index.Key));

                m_reader.Seek(index.Value.Item1, SeekOrigin.Begin); // offset
                byte[] fileData = m_reader.ReadBytes(index.Value.Item2);

                File.WriteAllBytes(dirpath + index.Key, fileData);
            }
        }

        public void ExtractFile(string file, string destfile)
        {
            destfile = Path.GetFullPath(destfile);

            IEnumerable<KeyValuePair<string, Tuple<int, int>>> files = m_indexes.Where(entry => entry.Key == file);

            if (files.Count() <= 0)
                throw new Exception("Unknown file " + file);

            m_reader.Seek(files.First().Value.Item1, SeekOrigin.Begin); // offset
            byte[] fileData = m_reader.ReadBytes(files.First().Value.Item2);

            File.WriteAllBytes(destfile, fileData);
        }

        public bool ExistsFile(string file)
        {
            return GetFilesName().Contains(file);
        }

        public string[] GetFilesName()
        {
            return (from entry in m_indexes
                    select entry.Key).ToArray();
        }

        public void Close()
        {
            m_reader.Dispose();
        }
    }
}