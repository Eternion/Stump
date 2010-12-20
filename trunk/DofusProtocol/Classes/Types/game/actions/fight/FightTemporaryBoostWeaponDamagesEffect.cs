using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightTemporaryBoostWeaponDamagesEffect : FightTemporaryBoostEffect
	{
		public const uint protocolId = 211;
		public int weaponTypeId = 0;
		
		public FightTemporaryBoostWeaponDamagesEffect()
		{
		}
		
		public FightTemporaryBoostWeaponDamagesEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5, int arg6, int arg7)
			: this()
		{
			initFightTemporaryBoostWeaponDamagesEffect(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 211;
		}
		
		public FightTemporaryBoostWeaponDamagesEffect initFightTemporaryBoostWeaponDamagesEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0, int arg6 = 0, int arg7 = 0)
		{
			base.initFightTemporaryBoostEffect(arg1, arg2, arg3, arg4, arg5, arg6);
			this.weaponTypeId = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.weaponTypeId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTemporaryBoostWeaponDamagesEffect(arg1);
		}
		
		public void serializeAs_FightTemporaryBoostWeaponDamagesEffect(BigEndianWriter arg1)
		{
			base.serializeAs_FightTemporaryBoostEffect(arg1);
			arg1.WriteShort((short)this.weaponTypeId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTemporaryBoostWeaponDamagesEffect(arg1);
		}
		
		public void deserializeAs_FightTemporaryBoostWeaponDamagesEffect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.weaponTypeId = (int)arg1.ReadShort();
		}
		
	}
}
