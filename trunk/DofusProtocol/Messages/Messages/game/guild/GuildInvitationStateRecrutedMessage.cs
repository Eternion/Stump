using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInvitationStateRecrutedMessage : Message
	{
		public const uint protocolId = 5548;
		internal Boolean _isInitialized = false;
		public uint invitationState = 0;
		
		public GuildInvitationStateRecrutedMessage()
		{
		}
		
		public GuildInvitationStateRecrutedMessage(uint arg1)
			: this()
		{
			initGuildInvitationStateRecrutedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5548;
		}
		
		public GuildInvitationStateRecrutedMessage initGuildInvitationStateRecrutedMessage(uint arg1 = 0)
		{
			this.invitationState = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.invitationState = 0;
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
			this.serializeAs_GuildInvitationStateRecrutedMessage(arg1);
		}
		
		public void serializeAs_GuildInvitationStateRecrutedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.invitationState);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInvitationStateRecrutedMessage(arg1);
		}
		
		public void deserializeAs_GuildInvitationStateRecrutedMessage(BigEndianReader arg1)
		{
			this.invitationState = (uint)arg1.ReadByte();
			if ( this.invitationState < 0 )
			{
				throw new Exception("Forbidden value (" + this.invitationState + ") on element of GuildInvitationStateRecrutedMessage.invitationState.");
			}
		}
		
	}
}
