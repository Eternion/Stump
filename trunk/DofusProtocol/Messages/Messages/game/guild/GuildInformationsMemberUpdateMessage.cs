using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInformationsMemberUpdateMessage : Message
	{
		public const uint protocolId = 5597;
		internal Boolean _isInitialized = false;
		public GuildMember member;
		
		public GuildInformationsMemberUpdateMessage()
		{
			this.member = new GuildMember();
		}
		
		public GuildInformationsMemberUpdateMessage(GuildMember arg1)
			: this()
		{
			initGuildInformationsMemberUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5597;
		}
		
		public GuildInformationsMemberUpdateMessage initGuildInformationsMemberUpdateMessage(GuildMember arg1 = null)
		{
			this.member = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.member = new GuildMember();
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
			this.serializeAs_GuildInformationsMemberUpdateMessage(arg1);
		}
		
		public void serializeAs_GuildInformationsMemberUpdateMessage(BigEndianWriter arg1)
		{
			this.member.serializeAs_GuildMember(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformationsMemberUpdateMessage(arg1);
		}
		
		public void deserializeAs_GuildInformationsMemberUpdateMessage(BigEndianReader arg1)
		{
			this.member = new GuildMember();
			this.member.deserialize(arg1);
		}
		
	}
}
