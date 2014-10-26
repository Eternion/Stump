

// Generated on 10/26/2014 23:30:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTemporaryBoostWeaponDamagesEffect : FightTemporaryBoostEffect
    {
        public const short Id = 211;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short weaponTypeId;
        
        public FightTemporaryBoostWeaponDamagesEffect()
        {
        }
        
        public FightTemporaryBoostWeaponDamagesEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int effectId, int parentBoostUid, short delta, short weaponTypeId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid, delta)
        {
            this.weaponTypeId = weaponTypeId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(weaponTypeId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponTypeId = reader.ReadShort();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short);
        }
        
    }
    
}