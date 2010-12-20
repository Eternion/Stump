using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeItemGoldAddAsPaymentMessage : Message
	{
		public const uint protocolId = 5770;
		internal Boolean _isInitialized = false;
		public int paymentType = 0;
		public uint quantity = 0;
		
		public ExchangeItemGoldAddAsPaymentMessage()
		{
		}
		
		public ExchangeItemGoldAddAsPaymentMessage(int arg1, uint arg2)
			: this()
		{
			initExchangeItemGoldAddAsPaymentMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5770;
		}
		
		public ExchangeItemGoldAddAsPaymentMessage initExchangeItemGoldAddAsPaymentMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.paymentType = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.paymentType = 0;
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
			this.serializeAs_ExchangeItemGoldAddAsPaymentMessage(arg1);
		}
		
		public void serializeAs_ExchangeItemGoldAddAsPaymentMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.paymentType);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeItemGoldAddAsPaymentMessage(arg1);
		}
		
		public void deserializeAs_ExchangeItemGoldAddAsPaymentMessage(BigEndianReader arg1)
		{
			this.paymentType = (int)arg1.ReadByte();
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeItemGoldAddAsPaymentMessage.quantity.");
			}
		}
		
	}
}
