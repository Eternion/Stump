using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildHouseRemoveMessage : Message
	{
		public const uint protocolId = 6180;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		
		public GuildHouseRemoveMessage()
		{
		}
		
		public GuildHouseRemoveMessage(uint arg1)
			: this()
		{
			initGuildHouseRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6180;
		}
		
		public GuildHouseRemoveMessage initGuildHouseRemoveMessage(uint arg1 = 0)
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
			this.serializeAs_GuildHouseRemoveMessage(arg1);
		}
		
		public void serializeAs_GuildHouseRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildHouseRemoveMessage(arg1);
		}
		
		public void deserializeAs_GuildHouseRemoveMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of GuildHouseRemoveMessage.houseId.");
			}
		}
		
	}
}
