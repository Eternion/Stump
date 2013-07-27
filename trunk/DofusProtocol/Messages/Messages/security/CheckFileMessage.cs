

// Generated on 07/26/2013 22:51:09
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CheckFileMessage : Message
    {
        public const uint Id = 6156;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string filenameHash;
        public sbyte type;
        public string value;
        
        public CheckFileMessage()
        {
        }
        
        public CheckFileMessage(string filenameHash, sbyte type, string value)
        {
            this.filenameHash = filenameHash;
            this.type = type;
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(filenameHash);
            writer.WriteSByte(type);
            writer.WriteUTF(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            filenameHash = reader.ReadUTF();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            value = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + filenameHash.Length + sizeof(sbyte) + sizeof(short) + value.Length;
        }
        
    }
    
}