using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightEntityDispositionInformations : EntityDispositionInformations
	{
		public const uint protocolId = 217;
		public int carryingCharacterId = 0;
		
		public FightEntityDispositionInformations()
		{
		}
		
		public FightEntityDispositionInformations(int arg1, uint arg2, int arg3)
			: this()
		{
			initFightEntityDispositionInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 217;
		}
		
		public FightEntityDispositionInformations initFightEntityDispositionInformations(int arg1 = 0, uint arg2 = 1, int arg3 = 0)
		{
			base.initEntityDispositionInformations(arg1, arg2);
			this.carryingCharacterId = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.carryingCharacterId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightEntityDispositionInformations(arg1);
		}
		
		public void serializeAs_FightEntityDispositionInformations(BigEndianWriter arg1)
		{
			base.serializeAs_EntityDispositionInformations(arg1);
			arg1.WriteInt((int)this.carryingCharacterId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightEntityDispositionInformations(arg1);
		}
		
		public void deserializeAs_FightEntityDispositionInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.carryingCharacterId = (int)arg1.ReadInt();
		}
		
	}
}
