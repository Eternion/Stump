using System;
using System.IO;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class DlmReader : IDisposable
    {
        private BigEndianReader m_reader;
        private Stream m_stream;

        public DlmReader(string filePath)
        {
            m_stream = File.OpenRead(filePath);
            m_reader = new BigEndianReader(m_stream);
        }

        public DlmReader(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public DlmMap ReadMap()
        {
            m_reader.Seek(0, SeekOrigin.Begin);

            int header = m_reader.ReadByte();

            if (header != 77)
            {
                try
                {
                    var uncompress = ZipHelper.Uncompress(m_reader.ReadBytes((int) m_reader.BytesAvailable));

                    if (uncompress.Length <= 0 || uncompress[0] != 77)
                        throw new FileLoadException("Wrong header file");

                    ChangeStream(new MemoryStream(uncompress));
                }
                catch (Exception)
                {
                    throw new FileLoadException("Wrong header file");
                }
            }

            var map = DlmMap.ReadFromStream(m_reader);

            return map;
        }

        private void ChangeStream(Stream stream)
        {
            m_stream.Dispose();
            m_reader.Dispose();

            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public void Dispose()
        {
            m_stream.Dispose();
            m_reader.Dispose();
        }
    }
}