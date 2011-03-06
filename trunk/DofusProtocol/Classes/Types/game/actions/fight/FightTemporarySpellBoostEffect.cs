using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
	{
		public const uint protocolId = 207;
		public uint boostedSpellId = 0;
		
		public FightTemporarySpellBoostEffect()
		{
		}
		
		public FightTemporarySpellBoostEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5, int arg6, uint arg7)
			: this()
		{
			initFightTemporarySpellBoostEffect(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 207;
		}
		
		public FightTemporarySpellBoostEffect initFightTemporarySpellBoostEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0, int arg6 = 0, uint arg7 = 0)
		{
			base.initFightTemporaryBoostEffect(arg1, arg2, arg3, arg4, arg5, arg6);
			this.boostedSpellId = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.boostedSpellId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTemporarySpellBoostEffect(arg1);
		}
		
		public void serializeAs_FightTemporarySpellBoostEffect(BigEndianWriter arg1)
		{
			base.serializeAs_FightTemporaryBoostEffect(arg1);
			if ( this.boostedSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostedSpellId + ") on element boostedSpellId.");
			}
			arg1.WriteShort((short)this.boostedSpellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTemporarySpellBoostEffect(arg1);
		}
		
		public void deserializeAs_FightTemporarySpellBoostEffect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.boostedSpellId = (uint)arg1.ReadShort();
			if ( this.boostedSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostedSpellId + ") on element of FightTemporarySpellBoostEffect.boostedSpellId.");
			}
		}
		
	}
}
