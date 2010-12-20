using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInvitationAnswerMessage : Message
	{
		public const uint protocolId = 5556;
		internal Boolean _isInitialized = false;
		public Boolean accept = false;
		
		public GuildInvitationAnswerMessage()
		{
		}
		
		public GuildInvitationAnswerMessage(Boolean arg1)
			: this()
		{
			initGuildInvitationAnswerMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5556;
		}
		
		public GuildInvitationAnswerMessage initGuildInvitationAnswerMessage(Boolean arg1 = false)
		{
			this.accept = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.accept = false;
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
			this.serializeAs_GuildInvitationAnswerMessage(arg1);
		}
		
		public void serializeAs_GuildInvitationAnswerMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.accept);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInvitationAnswerMessage(arg1);
		}
		
		public void deserializeAs_GuildInvitationAnswerMessage(BigEndianReader arg1)
		{
			this.accept = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
