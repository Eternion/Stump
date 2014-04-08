

// Generated on 03/06/2014 18:50:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EmotePlayErrorMessage : Message
    {
        public const uint Id = 5688;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte emoteId;
        
        public EmotePlayErrorMessage()
        {
        }
        
        public EmotePlayErrorMessage(byte emoteId)
        {
            this.emoteId = emoteId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(emoteId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            emoteId = reader.ReadByte();
            if (emoteId < 0 || emoteId > 255)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0 || emoteId > 255");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(byte);
        }
        
    }
    
}