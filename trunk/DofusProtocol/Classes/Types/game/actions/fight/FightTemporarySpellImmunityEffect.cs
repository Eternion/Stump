using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightTemporarySpellImmunityEffect : AbstractFightDispellableEffect
	{
		public const uint protocolId = 366;
		public int immuneSpellId = 0;
		
		public FightTemporarySpellImmunityEffect()
		{
		}
		
		public FightTemporarySpellImmunityEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5, int arg6)
			: this()
		{
			initFightTemporarySpellImmunityEffect(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 366;
		}
		
		public FightTemporarySpellImmunityEffect initFightTemporarySpellImmunityEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0, int arg6 = 0)
		{
			base.initAbstractFightDispellableEffect(arg1, arg2, arg3, arg4, arg5);
			this.immuneSpellId = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.immuneSpellId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTemporarySpellImmunityEffect(arg1);
		}
		
		public void serializeAs_FightTemporarySpellImmunityEffect(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractFightDispellableEffect(arg1);
			arg1.WriteInt((int)this.immuneSpellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTemporarySpellImmunityEffect(arg1);
		}
		
		public void deserializeAs_FightTemporarySpellImmunityEffect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.immuneSpellId = (int)arg1.ReadInt();
		}
		
	}
}
