using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeItemObjectAddAsPaymentMessage : Message
	{
		public const uint protocolId = 5766;
		internal Boolean _isInitialized = false;
		public int paymentType = 0;
		public Boolean bAdd = false;
		public uint objectToMoveId = 0;
		public uint quantity = 0;
		
		public ExchangeItemObjectAddAsPaymentMessage()
		{
		}
		
		public ExchangeItemObjectAddAsPaymentMessage(int arg1, Boolean arg2, uint arg3, uint arg4)
			: this()
		{
			initExchangeItemObjectAddAsPaymentMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5766;
		}
		
		public ExchangeItemObjectAddAsPaymentMessage initExchangeItemObjectAddAsPaymentMessage(int arg1 = 0, Boolean arg2 = false, uint arg3 = 0, uint arg4 = 0)
		{
			this.paymentType = arg1;
			this.bAdd = arg2;
			this.@objectToMoveId = arg3;
			this.quantity = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.paymentType = 0;
			this.bAdd = false;
			this.@objectToMoveId = 0;
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
			this.serializeAs_ExchangeItemObjectAddAsPaymentMessage(arg1);
		}
		
		public void serializeAs_ExchangeItemObjectAddAsPaymentMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.paymentType);
			arg1.WriteBoolean(this.bAdd);
			if ( this.@objectToMoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToMoveId + ") on element objectToMoveId.");
			}
			arg1.WriteInt((int)this.@objectToMoveId);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeItemObjectAddAsPaymentMessage(arg1);
		}
		
		public void deserializeAs_ExchangeItemObjectAddAsPaymentMessage(BigEndianReader arg1)
		{
			this.paymentType = (int)arg1.ReadByte();
			this.bAdd = (Boolean)arg1.ReadBoolean();
			this.@objectToMoveId = (uint)arg1.ReadInt();
			if ( this.@objectToMoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToMoveId + ") on element of ExchangeItemObjectAddAsPaymentMessage.objectToMoveId.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeItemObjectAddAsPaymentMessage.quantity.");
			}
		}
		
	}
}
