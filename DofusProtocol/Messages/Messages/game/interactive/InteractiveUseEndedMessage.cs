

// Generated on 12/20/2015 16:37:01
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveUseEndedMessage : Message
    {
        public const uint Id = 6112;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int elemId;
        public short skillId;
        
        public InteractiveUseEndedMessage()
        {
        }
        
        public InteractiveUseEndedMessage(int elemId, short skillId)
        {
            this.elemId = elemId;
            this.skillId = skillId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(elemId);
            writer.WriteVarShort(skillId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            elemId = reader.ReadVarInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillId = reader.ReadVarShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
    }
    
}