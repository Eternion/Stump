using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHousePriceMessage : Message
	{
		public const uint protocolId = 5805;
		internal Boolean _isInitialized = false;
		public uint genId = 0;
		
		public ExchangeBidHousePriceMessage()
		{
		}
		
		public ExchangeBidHousePriceMessage(uint arg1)
			: this()
		{
			initExchangeBidHousePriceMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5805;
		}
		
		public ExchangeBidHousePriceMessage initExchangeBidHousePriceMessage(uint arg1 = 0)
		{
			this.genId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.genId = 0;
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
			this.serializeAs_ExchangeBidHousePriceMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHousePriceMessage(BigEndianWriter arg1)
		{
			if ( this.genId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genId + ") on element genId.");
			}
			arg1.WriteInt((int)this.genId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHousePriceMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHousePriceMessage(BigEndianReader arg1)
		{
			this.genId = (uint)arg1.ReadInt();
			if ( this.genId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genId + ") on element of ExchangeBidHousePriceMessage.genId.");
			}
		}
		
	}
}
