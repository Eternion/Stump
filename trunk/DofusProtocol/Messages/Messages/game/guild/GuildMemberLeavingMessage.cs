using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildMemberLeavingMessage : Message
	{
		public const uint protocolId = 5923;
		internal Boolean _isInitialized = false;
		public Boolean kicked = false;
		public int memberId = 0;
		
		public GuildMemberLeavingMessage()
		{
		}
		
		public GuildMemberLeavingMessage(Boolean arg1, int arg2)
			: this()
		{
			initGuildMemberLeavingMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5923;
		}
		
		public GuildMemberLeavingMessage initGuildMemberLeavingMessage(Boolean arg1 = false, int arg2 = 0)
		{
			this.kicked = arg1;
			this.memberId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.kicked = false;
			this.memberId = 0;
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
			this.serializeAs_GuildMemberLeavingMessage(arg1);
		}
		
		public void serializeAs_GuildMemberLeavingMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.kicked);
			arg1.WriteInt((int)this.memberId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildMemberLeavingMessage(arg1);
		}
		
		public void deserializeAs_GuildMemberLeavingMessage(BigEndianReader arg1)
		{
			this.kicked = (Boolean)arg1.ReadBoolean();
			this.memberId = (int)arg1.ReadInt();
		}
		
	}
}
