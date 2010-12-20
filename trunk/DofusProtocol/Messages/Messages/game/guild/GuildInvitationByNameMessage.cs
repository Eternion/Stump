using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInvitationByNameMessage : Message
	{
		public const uint protocolId = 6115;
		internal Boolean _isInitialized = false;
		public String name = "";
		
		public GuildInvitationByNameMessage()
		{
		}
		
		public GuildInvitationByNameMessage(String arg1)
			: this()
		{
			initGuildInvitationByNameMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6115;
		}
		
		public GuildInvitationByNameMessage initGuildInvitationByNameMessage(String arg1 = "")
		{
			this.name = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildInvitationByNameMessage(arg1);
		}
		
		public void serializeAs_GuildInvitationByNameMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInvitationByNameMessage(arg1);
		}
		
		public void deserializeAs_GuildInvitationByNameMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
