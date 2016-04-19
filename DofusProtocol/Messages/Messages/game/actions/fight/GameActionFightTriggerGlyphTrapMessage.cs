

// Generated on 04/19/2016 10:17:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightTriggerGlyphTrapMessage : AbstractGameActionMessage
    {
        public const uint Id = 5741;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short markId;
        public double triggeringCharacterId;
        public short triggeredSpellId;
        
        public GameActionFightTriggerGlyphTrapMessage()
        {
        }
        
        public GameActionFightTriggerGlyphTrapMessage(short actionId, double sourceId, short markId, double triggeringCharacterId, short triggeredSpellId)
         : base(actionId, sourceId)
        {
            this.markId = markId;
            this.triggeringCharacterId = triggeringCharacterId;
            this.triggeredSpellId = triggeredSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
            writer.WriteDouble(triggeringCharacterId);
            writer.WriteVarShort(triggeredSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
            triggeringCharacterId = reader.ReadDouble();
            if (triggeringCharacterId < -9007199254740990 || triggeringCharacterId > 9007199254740990)
                throw new Exception("Forbidden value on triggeringCharacterId = " + triggeringCharacterId + ", it doesn't respect the following condition : triggeringCharacterId < -9007199254740990 || triggeringCharacterId > 9007199254740990");
            triggeredSpellId = reader.ReadVarShort();
            if (triggeredSpellId < 0)
                throw new Exception("Forbidden value on triggeredSpellId = " + triggeredSpellId + ", it doesn't respect the following condition : triggeredSpellId < 0");
        }
        
    }
    
}