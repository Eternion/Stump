using Stump.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stump.DofusProtocol.D2oClasses.Tools.Ma3
{
    public class Ma3Reader
    {
        Stream m_stream;
        BigEndianReader m_reader;

        public Ma3Reader(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public Ma3Reader(string file)
            : this(File.Open(file, FileMode.Open))
        {
            FilePath = file;
        }

        public string FilePath
        {
            get;
        }

        public List<Ma3Item> ReadFile()
        {
            var items = new List<Ma3Item>();

            while (m_reader.BytesAvailable > 0)
            {
                var item = new Ma3Item()
                {
                    Id = m_reader.ReadUInt(),
                    TypeId = m_reader.ReadShort(),
                    Name = m_reader.ReadUTF(),
                    Level = m_reader.ReadShort(),
                    IconId = m_reader.ReadUInt(),
                    SkinId = m_reader.ReadUInt(),
                    Look = m_reader.ReadUTF()
                };

                items.Add(item);
            }

            return items.OrderBy(x => x.Id).ToList();
        }

        public void Dispose()
        {
            m_reader.Dispose();
        }
    }

    public class Ma3Item
    {
        public uint Id
        {
            get;
            set;
        }

        public short TypeId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public short Level
        {
            get;
            set;
        }

        public uint IconId
        {
            get;
            set;
        }

        public uint SkinId
        {
            get;
            set;
        }

        public string Look
        {
            get;
            set;
        }
    }
}
