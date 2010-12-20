using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildJoinedMessage : Message
	{
		public const uint protocolId = 5564;
		internal Boolean _isInitialized = false;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		public uint memberRights = 0;
		
		public GuildJoinedMessage()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public GuildJoinedMessage(String arg1, GuildEmblem arg2, uint arg3)
			: this()
		{
			initGuildJoinedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5564;
		}
		
		public GuildJoinedMessage initGuildJoinedMessage(String arg1 = "", GuildEmblem arg2 = null, uint arg3 = 0)
		{
			this.guildName = arg1;
			this.guildEmblem = arg2;
			this.memberRights = arg3;
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
			this.serializeAs_GuildJoinedMessage(arg1);
		}
		
		public void serializeAs_GuildJoinedMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
			if ( this.memberRights < 0 || this.memberRights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.memberRights + ") on element memberRights.");
			}
			arg1.WriteUInt((uint)this.memberRights);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildJoinedMessage(arg1);
		}
		
		public void deserializeAs_GuildJoinedMessage(BigEndianReader arg1)
		{
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
			this.memberRights = (uint)arg1.ReadUInt();
			if ( this.memberRights < 0 || this.memberRights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.memberRights + ") on element of GuildJoinedMessage.memberRights.");
			}
		}
		
	}
}
