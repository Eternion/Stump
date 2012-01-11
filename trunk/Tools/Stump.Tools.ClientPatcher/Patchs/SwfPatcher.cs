using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Stump.Tools.ClientPatcher.Patchs
{
    public class SwfPatcher
    {
        private byte[] m_fileBuffer;

        private string m_signature;
        private uint m_size;
        private byte[] m_swfBuffer;
        private byte m_version;

        public SwfPatcher(string swfFile)
        {
            SwfFile = swfFile;
        }

        public string SwfFile
        {
            get;
            private set;
        }

        public void Open()
        {
            if (m_fileBuffer != null)
                return;

            m_fileBuffer = File.ReadAllBytes(SwfFile);

            var reader = new BinaryReader(new MemoryStream(m_fileBuffer));
            m_signature = Encoding.ASCII.GetString(reader.ReadBytes(3));
            m_version = reader.ReadByte();
            m_size = reader.ReadUInt32();

            var size = (int) (reader.BaseStream.Length - reader.BaseStream.Position);

            if (m_signature[0] == 'C')
            {
                m_swfBuffer = new byte[m_size];
                reader.BaseStream.Position = 0;
                reader.Read(m_swfBuffer, 0, 8);

                byte[] compressed = reader.ReadBytes(size);
                var inflater = new Inflater();
                inflater.SetInput(compressed);
                inflater.Inflate(m_swfBuffer, 8, (int) m_size);
            }
            else
            {
                m_swfBuffer = m_fileBuffer;
            }
        }

        public void Patch(byte[] findPattern, byte[] replacePattern, string mask)
        {
            Open();

            if (findPattern.Length != replacePattern.Length || replacePattern.Length != mask.Length)
                throw new ArgumentException("findPattern.Length != replacePattern.Length || replacePattern.Length != mask.Length");

            var finder = new PatternFinder(m_swfBuffer);
            int[] indexes = finder.FindPattern(findPattern, mask);

            if (indexes.Length == 0)
                throw new Exception("Pattern not found");

            if (indexes.Length > 1)
                throw new Exception("Too many results");

            var index = indexes[0];

            for (int i = 0; i < replacePattern.Length; i++)
            {
                if (mask[i] == '?')
                    continue;

                m_swfBuffer[index + i] = replacePattern[i];
            }
        }

        public void Save(string destinationFile, bool compress = true)
        {
            var writer = new BinaryWriter(File.OpenWrite(destinationFile));

            m_signature = compress ? "CWS" : "FWS";

            writer.Write(Encoding.ASCII.GetBytes(m_signature));
            writer.Write(m_version);
            writer.Write(m_size);

            if (m_signature[0] == 'C')
            {
                var deflater = new Deflater();
                var compressor = new BinaryWriter(new DeflaterOutputStream(writer.BaseStream, new Deflater(Deflater.DEFLATED, false)));

                compressor.Write(m_swfBuffer, 8, (int) (m_size - 8));
                compressor.Close();
            }
            else
                writer.Write(m_swfBuffer, 8, (int) m_size - 8);

            writer.Close();
        }
    }
}