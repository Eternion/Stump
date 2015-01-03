

// Generated on 12/29/2014 21:12:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EmoteAddMessage : Message
    {
        public const uint Id = 5644;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte emoteId;
        
        public EmoteAddMessage()
        {
        }
        
        public EmoteAddMessage(byte emoteId)
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