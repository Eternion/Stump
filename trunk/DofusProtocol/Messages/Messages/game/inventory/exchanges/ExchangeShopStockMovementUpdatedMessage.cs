using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeShopStockMovementUpdatedMessage : Message
	{
		public const uint protocolId = 5909;
		internal Boolean _isInitialized = false;
		public ObjectItemToSell objectInfo;
		
		public ExchangeShopStockMovementUpdatedMessage()
		{
			this.@objectInfo = new ObjectItemToSell();
		}
		
		public ExchangeShopStockMovementUpdatedMessage(ObjectItemToSell arg1)
			: this()
		{
			initExchangeShopStockMovementUpdatedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5909;
		}
		
		public ExchangeShopStockMovementUpdatedMessage initExchangeShopStockMovementUpdatedMessage(ObjectItemToSell arg1 = null)
		{
			this.@objectInfo = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectInfo = new ObjectItemToSell();
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
			this.serializeAs_ExchangeShopStockMovementUpdatedMessage(arg1);
		}
		
		public void serializeAs_ExchangeShopStockMovementUpdatedMessage(BigEndianWriter arg1)
		{
			this.@objectInfo.serializeAs_ObjectItemToSell(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeShopStockMovementUpdatedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeShopStockMovementUpdatedMessage(BigEndianReader arg1)
		{
			this.@objectInfo = new ObjectItemToSell();
			this.@objectInfo.deserialize(arg1);
		}
		
	}
}
