using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseGuildRightsMessage : Message
	{
		public const uint protocolId = 5703;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		public uint rights = 0;
		
		public HouseGuildRightsMessage()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public HouseGuildRightsMessage(uint arg1, String arg2, GuildEmblem arg3, uint arg4)
			: this()
		{
			initHouseGuildRightsMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5703;
		}
		
		public HouseGuildRightsMessage initHouseGuildRightsMessage(uint arg1 = 0, String arg2 = "", GuildEmblem arg3 = null, uint arg4 = 0)
		{
			this.houseId = arg1;
			this.guildName = arg2;
			this.guildEmblem = arg3;
			this.rights = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.houseId = 0;
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
			this.serializeAs_HouseGuildRightsMessage(arg1);
		}
		
		public void serializeAs_HouseGuildRightsMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteShort((short)this.houseId);
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element rights.");
			}
			arg1.WriteUInt((uint)this.rights);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseGuildRightsMessage(arg1);
		}
		
		public void deserializeAs_HouseGuildRightsMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadShort();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseGuildRightsMessage.houseId.");
			}
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
			this.rights = (uint)arg1.ReadUInt();
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element of HouseGuildRightsMessage.rights.");
			}
		}
		
	}
}
