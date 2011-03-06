using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildCreationValidMessage : Message
	{
		public const uint protocolId = 5546;
		internal Boolean _isInitialized = false;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		
		public GuildCreationValidMessage()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public GuildCreationValidMessage(String arg1, GuildEmblem arg2)
			: this()
		{
			initGuildCreationValidMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5546;
		}
		
		public GuildCreationValidMessage initGuildCreationValidMessage(String arg1 = "", GuildEmblem arg2 = null)
		{
			this.guildName = arg1;
			this.guildEmblem = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.guildName = "";
			this.guildEmblem = new GuildEmblem();
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
			this.serializeAs_GuildCreationValidMessage(arg1);
		}
		
		public void serializeAs_GuildCreationValidMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildCreationValidMessage(arg1);
		}
		
		public void deserializeAs_GuildCreationValidMessage(BigEndianReader arg1)
		{
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
