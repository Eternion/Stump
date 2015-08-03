

// Generated on 08/04/2015 00:37:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpellItemBoostMessage : Message
    {
        public const uint Id = 6011;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int statId;
        public short spellId;
        public short value;
        
        public SpellItemBoostMessage()
        {
        }
        
        public SpellItemBoostMessage(int statId, short spellId, short value)
        {
            this.statId = statId;
            this.spellId = spellId;
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(statId);
            writer.WriteVarShort(spellId);
            writer.WriteVarShort(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            statId = reader.ReadVarInt();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = reader.ReadVarShort();
        }
        
    }
    
}