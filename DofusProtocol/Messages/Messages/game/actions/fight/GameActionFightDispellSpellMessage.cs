

// Generated on 04/19/2016 10:17:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightDispellSpellMessage : GameActionFightDispellMessage
    {
        public const uint Id = 6176;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        
        public GameActionFightDispellSpellMessage()
        {
        }
        
        public GameActionFightDispellSpellMessage(short actionId, double sourceId, double targetId, short spellId)
         : base(actionId, sourceId, targetId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}