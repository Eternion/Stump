using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInvitationMessage : Message
	{
		public const uint protocolId = 5551;
		internal Boolean _isInitialized = false;
		public uint targetId = 0;
		
		public GuildInvitationMessage()
		{
		}
		
		public GuildInvitationMessage(uint arg1)
			: this()
		{
			initGuildInvitationMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5551;
		}
		
		public GuildInvitationMessage initGuildInvitationMessage(uint arg1 = 0)
		{
			this.targetId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.targetId = 0;
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
			this.serializeAs_GuildInvitationMessage(arg1);
		}
		
		public void serializeAs_GuildInvitationMessage(BigEndianWriter arg1)
		{
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element targetId.");
			}
			arg1.WriteInt((int)this.targetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInvitationMessage(arg1);
		}
		
		public void deserializeAs_GuildInvitationMessage(BigEndianReader arg1)
		{
			this.targetId = (uint)arg1.ReadInt();
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element of GuildInvitationMessage.targetId.");
			}
		}
		
	}
}
