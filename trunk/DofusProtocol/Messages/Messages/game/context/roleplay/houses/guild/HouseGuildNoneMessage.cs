using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseGuildNoneMessage : Message
	{
		public const uint protocolId = 5701;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		
		public HouseGuildNoneMessage()
		{
		}
		
		public HouseGuildNoneMessage(uint arg1)
			: this()
		{
			initHouseGuildNoneMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5701;
		}
		
		public HouseGuildNoneMessage initHouseGuildNoneMessage(uint arg1 = 0)
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
			this.serializeAs_HouseGuildNoneMessage(arg1);
		}
		
		public void serializeAs_HouseGuildNoneMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteShort((short)this.houseId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseGuildNoneMessage(arg1);
		}
		
		public void deserializeAs_HouseGuildNoneMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadShort();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseGuildNoneMessage.houseId.");
			}
		}
		
	}
}
