

// Generated on 02/02/2016 14:14:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveUsedMessage : Message
    {
        public const uint Id = 5745;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long entityId;
        public int elemId;
        public short skillId;
        public short duration;
        
        public InteractiveUsedMessage()
        {
        }
        
        public InteractiveUsedMessage(long entityId, int elemId, short skillId, short duration)
        {
            this.entityId = entityId;
            this.elemId = elemId;
            this.skillId = skillId;
            this.duration = duration;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(entityId);
            writer.WriteVarInt(elemId);
            writer.WriteVarShort(skillId);
            writer.WriteVarShort(duration);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadVarLong();
            if (entityId < 0 || entityId > 9.007199254740992E15)
                throw new Exception("Forbidden value on entityId = " + entityId + ", it doesn't respect the following condition : entityId < 0 || entityId > 9.007199254740992E15");
            elemId = reader.ReadVarInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillId = reader.ReadVarShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            duration = reader.ReadVarShort();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
        }
        
    }
    
}