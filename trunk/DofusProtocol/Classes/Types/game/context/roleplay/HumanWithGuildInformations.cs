using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class HumanWithGuildInformations : HumanInformations
	{
		public const uint protocolId = 153;
		public GuildInformations guildInformations;
		
		public HumanWithGuildInformations()
		{
			this.guildInformations = new GuildInformations();
		}
		
		public HumanWithGuildInformations(List<EntityLook> arg1, int arg2, uint arg3, ActorRestrictionsInformations arg4, uint arg5, String arg6, GuildInformations arg7)
			: this()
		{
			initHumanWithGuildInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 153;
		}
		
		public HumanWithGuildInformations initHumanWithGuildInformations(List<EntityLook> arg1, int arg2 = 0, uint arg3 = 0, ActorRestrictionsInformations arg4 = null, uint arg5 = 0, String arg6 = "", GuildInformations arg7 = null)
		{
			base.initHumanInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.guildInformations = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildInformations = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HumanWithGuildInformations(arg1);
		}
		
		public void serializeAs_HumanWithGuildInformations(BigEndianWriter arg1)
		{
			base.serializeAs_HumanInformations(arg1);
			this.guildInformations.serializeAs_GuildInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HumanWithGuildInformations(arg1);
		}
		
		public void deserializeAs_HumanWithGuildInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildInformations = new GuildInformations();
			this.guildInformations.deserialize(arg1);
		}
		
	}
}
