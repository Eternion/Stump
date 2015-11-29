using Stump.Core.IO;
using SwfLib;
using SwfLib.Tags.ControlTags;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stump.DofusProtocol.D2oClasses.Tools.Swl
{
    public class SwfReader
    {
        Stream m_stream;
        BigEndianReader m_reader;

        public SwfReader(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);

            Classes = new List<string>();

            ReadFile();
        }

        public SwfReader(string file)
            : this(File.Open(file, FileMode.Open))
        {
            FilePath = file;
        }

        public string FilePath
        {
            get;
        }

        public byte Version
        {
            get;
            set;
        }

        public uint FrameRate
        {
            get;
            set;
        }

        public List<string> Classes
        {
            get;
            set;
        }

        public byte[] SwfData
        {
            get;
            set;
        }

        void ReadFile()
        {
            var swf = SwfFile.ReadFrom(m_reader.BaseStream);
            var tag = swf.Tags.FirstOrDefault(x => x is SymbolClassTag) as SymbolClassTag;

            if (tag == null)
                return;

            FrameRate = (uint)swf.Header.FrameRate;
            //Version = swf.FileInfo.Version;
            Version = 0; //It seem all swl have 0 for Version

            foreach (var reference in tag.References)
            {
                Classes.Add(reference.SymbolName);
            }

            m_reader.Seek(0, SeekOrigin.Begin);
            SwfData = m_reader.ReadBytes((int)m_reader.BytesAvailable);
        }

        public void ToSwl()
        {
            var writer = new BigEndianWriter();

            writer.WriteByte(76); //Header
            writer.WriteByte(Version);
            writer.WriteUInt(FrameRate);
            writer.WriteInt(Classes.Count);

            foreach (var classe in Classes)
            {
                writer.WriteUTF(classe);
            }

            writer.WriteBytes(SwfData);

            var swlName = FilePath.Replace(".swf", ".swl");
            var file = File.Create(swlName);
            file.Write(writer.Data, 0, writer.Data.Length);

            file.Dispose();
            writer.Dispose();
        }

        public void Dispose()
        {
            m_reader.Dispose();
        }
    }
}
