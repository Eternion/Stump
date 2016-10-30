

// Generated on 10/30/2016 16:20:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTemporarySpellImmunityEffect : AbstractFightDispellableEffect
    {
        public const short Id = 366;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int immuneSpellId;
        
        public FightTemporarySpellImmunityEffect()
        {
        }
        
        public FightTemporarySpellImmunityEffect(int uid, double targetId, short turnDuration, sbyte dispelable, short spellId, int effectId, int parentBoostUid, int immuneSpellId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid)
        {
            this.immuneSpellId = immuneSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(immuneSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            immuneSpellId = reader.ReadInt();
        }
        
        
    }
    
}