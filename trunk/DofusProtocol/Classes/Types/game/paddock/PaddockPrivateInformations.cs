using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PaddockPrivateInformations : PaddockAbandonnedInformations
	{
		public const uint protocolId = 131;
		public GuildInformations guildInfo;
		
		public PaddockPrivateInformations()
		{
			this.guildInfo = new GuildInformations();
		}
		
		public PaddockPrivateInformations(uint arg1, uint arg2, uint arg3, int arg4, GuildInformations arg5)
			: this()
		{
			initPaddockPrivateInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 131;
		}
		
		public PaddockPrivateInformations initPaddockPrivateInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, int arg4 = 0, GuildInformations arg5 = null)
		{
			base.initPaddockAbandonnedInformations(arg1, arg2, arg3, arg4);
			this.guildInfo = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildInfo = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockPrivateInformations(arg1);
		}
		
		public void serializeAs_PaddockPrivateInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockAbandonnedInformations(arg1);
			this.guildInfo.serializeAs_GuildInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockPrivateInformations(arg1);
		}
		
		public void deserializeAs_PaddockPrivateInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildInfo = new GuildInformations();
			this.guildInfo.deserialize(arg1);
		}
		
	}
}
