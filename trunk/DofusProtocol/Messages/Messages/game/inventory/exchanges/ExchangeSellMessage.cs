using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeSellMessage : Message
	{
		public const uint protocolId = 5778;
		internal Boolean _isInitialized = false;
		public uint objectToSellId = 0;
		public uint quantity = 0;
		
		public ExchangeSellMessage()
		{
		}
		
		public ExchangeSellMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeSellMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5778;
		}
		
		public ExchangeSellMessage initExchangeSellMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectToSellId = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectToSellId = 0;
			this.quantity = 0;
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
			this.serializeAs_ExchangeSellMessage(arg1);
		}
		
		public void serializeAs_ExchangeSellMessage(BigEndianWriter arg1)
		{
			if ( this.@objectToSellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToSellId + ") on element objectToSellId.");
			}
			arg1.WriteInt((int)this.@objectToSellId);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeSellMessage(arg1);
		}
		
		public void deserializeAs_ExchangeSellMessage(BigEndianReader arg1)
		{
			this.@objectToSellId = (uint)arg1.ReadInt();
			if ( this.@objectToSellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToSellId + ") on element of ExchangeSellMessage.objectToSellId.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeSellMessage.quantity.");
			}
		}
		
	}
}
