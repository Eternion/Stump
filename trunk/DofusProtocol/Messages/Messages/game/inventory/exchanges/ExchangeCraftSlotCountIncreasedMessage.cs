using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeCraftSlotCountIncreasedMessage : Message
	{
		public const uint protocolId = 6125;
		internal Boolean _isInitialized = false;
		public uint newMaxSlot = 0;
		
		public ExchangeCraftSlotCountIncreasedMessage()
		{
		}
		
		public ExchangeCraftSlotCountIncreasedMessage(uint arg1)
			: this()
		{
			initExchangeCraftSlotCountIncreasedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6125;
		}
		
		public ExchangeCraftSlotCountIncreasedMessage initExchangeCraftSlotCountIncreasedMessage(uint arg1 = 0)
		{
			this.newMaxSlot = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.newMaxSlot = 0;
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
			this.serializeAs_ExchangeCraftSlotCountIncreasedMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftSlotCountIncreasedMessage(BigEndianWriter arg1)
		{
			if ( this.newMaxSlot < 0 )
			{
				throw new Exception("Forbidden value (" + this.newMaxSlot + ") on element newMaxSlot.");
			}
			arg1.WriteByte((byte)this.newMaxSlot);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftSlotCountIncreasedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftSlotCountIncreasedMessage(BigEndianReader arg1)
		{
			this.newMaxSlot = (uint)arg1.ReadByte();
			if ( this.newMaxSlot < 0 )
			{
				throw new Exception("Forbidden value (" + this.newMaxSlot + ") on element of ExchangeCraftSlotCountIncreasedMessage.newMaxSlot.");
			}
		}
		
	}
}
