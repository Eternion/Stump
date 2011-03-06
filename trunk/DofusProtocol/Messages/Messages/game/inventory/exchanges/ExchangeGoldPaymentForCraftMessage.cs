using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeGoldPaymentForCraftMessage : Message
	{
		public const uint protocolId = 5833;
		internal Boolean _isInitialized = false;
		public Boolean onlySuccess = false;
		public uint goldSum = 0;
		
		public ExchangeGoldPaymentForCraftMessage()
		{
		}
		
		public ExchangeGoldPaymentForCraftMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeGoldPaymentForCraftMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5833;
		}
		
		public ExchangeGoldPaymentForCraftMessage initExchangeGoldPaymentForCraftMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.onlySuccess = arg1;
			this.goldSum = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.onlySuccess = false;
			this.goldSum = 0;
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
			this.serializeAs_ExchangeGoldPaymentForCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeGoldPaymentForCraftMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.onlySuccess);
			if ( this.goldSum < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldSum + ") on element goldSum.");
			}
			arg1.WriteInt((int)this.goldSum);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeGoldPaymentForCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeGoldPaymentForCraftMessage(BigEndianReader arg1)
		{
			this.onlySuccess = (Boolean)arg1.ReadBoolean();
			this.goldSum = (uint)arg1.ReadInt();
			if ( this.goldSum < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldSum + ") on element of ExchangeGoldPaymentForCraftMessage.goldSum.");
			}
		}
		
	}
}
