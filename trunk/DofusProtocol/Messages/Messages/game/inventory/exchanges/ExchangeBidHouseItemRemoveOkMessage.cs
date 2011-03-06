using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseItemRemoveOkMessage : Message
	{
		public const uint protocolId = 5946;
		internal Boolean _isInitialized = false;
		public int sellerId = 0;
		
		public ExchangeBidHouseItemRemoveOkMessage()
		{
		}
		
		public ExchangeBidHouseItemRemoveOkMessage(int arg1)
			: this()
		{
			initExchangeBidHouseItemRemoveOkMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5946;
		}
		
		public ExchangeBidHouseItemRemoveOkMessage initExchangeBidHouseItemRemoveOkMessage(int arg1 = 0)
		{
			this.sellerId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sellerId = 0;
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
			this.serializeAs_ExchangeBidHouseItemRemoveOkMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseItemRemoveOkMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.sellerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseItemRemoveOkMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseItemRemoveOkMessage(BigEndianReader arg1)
		{
			this.sellerId = (int)arg1.ReadInt();
		}
		
	}
}
