

// Generated on 09/01/2015 10:48:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTemporaryBoostStateEffect : FightTemporaryBoostEffect
    {
        public const short Id = 214;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short stateId;
        
        public FightTemporaryBoostStateEffect()
        {
        }
        
        public FightTemporaryBoostStateEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int effectId, int parentBoostUid, short delta, short stateId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid, delta)
        {
            this.stateId = stateId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(stateId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            stateId = reader.ReadShort();
        }
        
        
    }
    
}