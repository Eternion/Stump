using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GuildInformations : BasicGuildInformations
	{
		public const uint protocolId = 127;
		public GuildEmblem guildEmblem;
		
		public GuildInformations()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public GuildInformations(uint arg1, String arg2, GuildEmblem arg3)
			: this()
		{
			initGuildInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 127;
		}
		
		public GuildInformations initGuildInformations(uint arg1 = 0, String arg2 = "", GuildEmblem arg3 = null)
		{
			base.initBasicGuildInformations(arg1, arg2);
			this.guildEmblem = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildEmblem = new GuildEmblem();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildInformations(arg1);
		}
		
		public void serializeAs_GuildInformations(BigEndianWriter arg1)
		{
			base.serializeAs_BasicGuildInformations(arg1);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformations(arg1);
		}
		
		public void deserializeAs_GuildInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
