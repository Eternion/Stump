

// Generated on 12/20/2015 16:36:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightNoSpellCastMessage : Message
    {
        public const uint Id = 6132;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int spellLevelId;
        
        public GameActionFightNoSpellCastMessage()
        {
        }
        
        public GameActionFightNoSpellCastMessage(int spellLevelId)
        {
            this.spellLevelId = spellLevelId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(spellLevelId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellLevelId = reader.ReadVarInt();
            if (spellLevelId < 0)
                throw new Exception("Forbidden value on spellLevelId = " + spellLevelId + ", it doesn't respect the following condition : spellLevelId < 0");
        }
        
    }
    
}