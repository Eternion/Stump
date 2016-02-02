

// Generated on 02/02/2016 14:14:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
    {
        public const short Id = 207;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short boostedSpellId;
        
        public FightTemporarySpellBoostEffect()
        {
        }
        
        public FightTemporarySpellBoostEffect(int uid, double targetId, short turnDuration, sbyte dispelable, short spellId, int effectId, int parentBoostUid, short delta, short boostedSpellId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid, delta)
        {
            this.boostedSpellId = boostedSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(boostedSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            boostedSpellId = reader.ReadVarShort();
            if (boostedSpellId < 0)
                throw new Exception("Forbidden value on boostedSpellId = " + boostedSpellId + ", it doesn't respect the following condition : boostedSpellId < 0");
        }
        
        
    }
    
}