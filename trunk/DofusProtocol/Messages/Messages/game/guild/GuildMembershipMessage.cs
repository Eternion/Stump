using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildMembershipMessage : GuildJoinedMessage
	{
		public const uint protocolId = 5835;
		internal Boolean _isInitialized = false;
		
		public GuildMembershipMessage()
		{
		}
		
		public GuildMembershipMessage(GuildInformations arg1, uint arg2)
			: this()
		{
			initGuildMembershipMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5835;
		}
		
		public GuildMembershipMessage initGuildMembershipMessage(GuildInformations arg1 = null, uint arg2 = 0)
		{
			base.initGuildJoinedMessage(arg1, arg2);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildMembershipMessage(arg1);
		}
		
		public void serializeAs_GuildMembershipMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GuildJoinedMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildMembershipMessage(arg1);
		}
		
		public void deserializeAs_GuildMembershipMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
