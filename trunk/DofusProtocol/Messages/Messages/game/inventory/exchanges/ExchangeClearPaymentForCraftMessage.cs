using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeClearPaymentForCraftMessage : Message
	{
		public const uint protocolId = 6145;
		internal Boolean _isInitialized = false;
		public int paymentType = 0;
		
		public ExchangeClearPaymentForCraftMessage()
		{
		}
		
		public ExchangeClearPaymentForCraftMessage(int arg1)
			: this()
		{
			initExchangeClearPaymentForCraftMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6145;
		}
		
		public ExchangeClearPaymentForCraftMessage initExchangeClearPaymentForCraftMessage(int arg1 = 0)
		{
			this.paymentType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.paymentType = 0;
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
			this.serializeAs_ExchangeClearPaymentForCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeClearPaymentForCraftMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.paymentType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeClearPaymentForCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeClearPaymentForCraftMessage(BigEndianReader arg1)
		{
			this.paymentType = (int)arg1.ReadByte();
		}
		
	}
}
