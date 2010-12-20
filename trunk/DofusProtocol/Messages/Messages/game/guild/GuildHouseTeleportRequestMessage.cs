using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildHouseTeleportRequestMessage : Message
	{
		public const uint protocolId = 5712;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		
		public GuildHouseTeleportRequestMessage()
		{
		}
		
		public GuildHouseTeleportRequestMessage(uint arg1)
			: this()
		{
			initGuildHouseTeleportRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5712;
		}
		
		public GuildHouseTeleportRequestMessage initGuildHouseTeleportRequestMessage(uint arg1 = 0)
		{
			this.houseId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.houseId = 0;
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
			this.serializeAs_GuildHouseTeleportRequestMessage(arg1);
		}
		
		public void serializeAs_GuildHouseTeleportRequestMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildHouseTeleportRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildHouseTeleportRequestMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of GuildHouseTeleportRequestMessage.houseId.");
			}
		}
		
	}
}
