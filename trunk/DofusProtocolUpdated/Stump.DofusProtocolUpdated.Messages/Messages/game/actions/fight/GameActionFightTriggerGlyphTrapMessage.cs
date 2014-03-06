

// Generated on 03/06/2014 18:50:02
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
        public int triggeringCharacterId;
        public short triggeredSpellId;
        
        public GameActionFightTriggerGlyphTrapMessage()
        {
        }
        
        public GameActionFightTriggerGlyphTrapMessage(short actionId, int sourceId, short markId, int triggeringCharacterId, short triggeredSpellId)
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
            writer.WriteInt(triggeringCharacterId);
            writer.WriteShort(triggeredSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
            triggeringCharacterId = reader.ReadInt();
            triggeredSpellId = reader.ReadShort();
            if (triggeredSpellId < 0)
                throw new Exception("Forbidden value on triggeredSpellId = " + triggeredSpellId + ", it doesn't respect the following condition : triggeredSpellId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(int) + sizeof(short);
        }
        
    }
    
}