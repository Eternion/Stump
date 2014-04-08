

// Generated on 03/02/2014 20:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EmoteRemoveMessage : Message
    {
        public const uint Id = 5687;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte emoteId;
        
        public EmoteRemoveMessage()
        {
        }
        
        public EmoteRemoveMessage(sbyte emoteId)
        {
            this.emoteId = emoteId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(emoteId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            emoteId = reader.ReadSByte();
            if (emoteId < 0)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}