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

namespace Stump.Server.DataProvider.Data.MapTool
{
    public class PakFile : IDisposable
    {
        private Dictionary<string, Tuple<int, int, BigEndianReader>> m_indexes;
        private readonly Dictionary<string, Dictionary<string, string>> m_properties = new Dictionary<string, Dictionary<string, string>>();
        private readonly List<BigEndianReader> m_openReaders = new List<BigEndianReader>();
        private readonly string m_filePath;

        public string FilePath
        {
            get { return m_filePath; }
        }

        public PakFile(string filepath)
        {
            m_filePath = filepath;

            Init();
        }

        private void Init()
        {
            string linkFile = FilePath;
            bool linked = false;

           parse:
            var reader = new BigEndianReader(File.OpenRead(linkFile));
            m_openReaders.Add(reader);

            if (reader.ReadByte() != 2 || reader.ReadByte() != 1)
                throw new FileLoadException("Corrupted d2p header");

            reader.Seek(-24, SeekOrigin.End);
            var offsetBase = reader.ReadInt();
            var loc9 = reader.ReadUInt();
            var startOffset = reader.ReadInt();
            var elementsCount = reader.ReadUInt();
            var propertiesOffset = reader.ReadInt();
            var propertiesCount = reader.ReadUInt();

            reader.Seek(propertiesOffset, SeekOrigin.Begin);
            m_properties.Add(linkFile, new Dictionary<string, string>((int)propertiesCount));

            for (int i = 0; i < propertiesCount; i++)
            {
                var key = reader.ReadUTF();
                var property = reader.ReadUTF();

                m_properties[linkFile].Add(key, property);

                if (key == "link")
                {
                    linkFile = Path.GetDirectoryName(linkFile) + @"\" + property;
                    linked = true;
                }
            }

            reader.Seek(startOffset, SeekOrigin.Begin);
            m_indexes = new Dictionary<string, Tuple<int, int, BigEndianReader>>((int)elementsCount);
            for (int i = 0; i < elementsCount; i++)
            {
                string indexname = reader.ReadUTF();
                int offset = reader.ReadInt() + offsetBase;
                int bytescount = reader.ReadInt();

                m_indexes.Add(indexname, new Tuple<int, int, BigEndianReader>(offset, bytescount, reader));
            }

            if (linked)
            {
                linked = false;
                goto parse;
            }
        }

        public void ExtractAll(string dirpath)
        {
            dirpath = Path.GetFullPath(dirpath);

            foreach (var index in m_indexes)
            {
                CreateDirectoriesOnPath(dirpath + index.Key);

                index.Value.Item3.Seek(index.Value.Item1, SeekOrigin.Begin); // offset
                var fileData = index.Value.Item3.ReadBytes(index.Value.Item2);

                File.WriteAllBytes(dirpath + index.Key, fileData);
            }
        }

        public void ExtractFile(string file, string destfile)
        {
            destfile = Path.GetFullPath(destfile);

            CreateDirectoriesOnPath(destfile);

            Tuple<int, int, BigEndianReader> outvalue;
            if (!m_indexes.TryGetValue(file, out outvalue))
                throw new Exception("Unknown file " + file);

            outvalue.Item3.Seek(outvalue.Item1, SeekOrigin.Begin); // offset
            var fileData = outvalue.Item3.ReadBytes(outvalue.Item2);

            File.WriteAllBytes(destfile, fileData);
        }

        public byte[] ReadFile(string file)
        {
            Tuple<int, int, BigEndianReader> outvalue;
            if (!m_indexes.TryGetValue(file, out outvalue))
                throw new Exception("Unknown file " + file);

            outvalue.Item3.Seek(outvalue.Item1, SeekOrigin.Begin); // offset

            return outvalue.Item3.ReadBytes(outvalue.Item2);
        }

        public bool ExistsFile(string file)
        {
            return GetFilesName().Contains(file);
        }

        private static void CreateDirectoriesOnPath(string path)
        {
            var unexistantDirectories = new List<string>();
            string lastDir = Path.GetDirectoryName(path);
            while (!Directory.Exists(lastDir))
            {
                unexistantDirectories.Add(lastDir);

                lastDir = Path.GetDirectoryName(lastDir);
            }

            unexistantDirectories.Reverse();

            foreach (var unexistantDirectory in unexistantDirectories)
            {
                Directory.CreateDirectory(unexistantDirectory);
            }
        }

        public string[] GetFilesName()
        {
            return (from entry in m_indexes
                    select entry.Key).ToArray();
        }

        public IEnumerable<PakedFileInfo> GetFilesInfo()
        {
            foreach (var entry in m_indexes)
            {
                var directories = Path.GetDirectoryName(entry.Key).Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                yield return new PakedFileInfo
                    {
                        Name = entry.Key,
                        Directories = directories,
                        Index = entry.Value.Item1,
                        Size = entry.Value.Item2,
                        Container = Path.GetFileName(((FileStream) entry.Value.Item3.BaseStream).Name),
                    };
            }
        }

        public void Close()
        {
            foreach (var reader in m_openReaders)
            {
                reader.Dispose();
            }
        }

        public void Dispose()
        {
            Close();
        }

        public class PakedFileInfo
        {
            public string Name
            {
                get;
                set;
            }

            public string[] Directories
            {
                get;
                set;
            }

            public int Size
            {
                get;
                set;
            }

            public int Index
            {
                get;
                set;
            }

            public string Container
            {
                get;
                set;
            }
        }
    }
}