

// Generated on 10/28/2014 16:37:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CheckFileRequestMessage : Message
    {
        public const uint Id = 6154;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string filename;
        public sbyte type;
        
        public CheckFileRequestMessage()
        {
        }
        
        public CheckFileRequestMessage(string filename, sbyte type)
        {
            this.filename = filename;
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(filename);
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            filename = reader.ReadUTF();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(filename) + sizeof(sbyte);
        }
        
    }
    
}