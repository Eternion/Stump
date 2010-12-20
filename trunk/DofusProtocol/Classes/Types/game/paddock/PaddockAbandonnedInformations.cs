using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PaddockAbandonnedInformations : PaddockBuyableInformations
	{
		public const uint protocolId = 133;
		public int guildId = 0;
		
		public PaddockAbandonnedInformations()
		{
		}
		
		public PaddockAbandonnedInformations(uint arg1, uint arg2, uint arg3, int arg4)
			: this()
		{
			initPaddockAbandonnedInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 133;
		}
		
		public PaddockAbandonnedInformations initPaddockAbandonnedInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, int arg4 = 0)
		{
			base.initPaddockBuyableInformations(arg1, arg2, arg3);
			this.guildId = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockAbandonnedInformations(arg1);
		}
		
		public void serializeAs_PaddockAbandonnedInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockBuyableInformations(arg1);
			arg1.WriteInt((int)this.guildId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockAbandonnedInformations(arg1);
		}
		
		public void deserializeAs_PaddockAbandonnedInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildId = (int)arg1.ReadInt();
		}
		
	}
}
