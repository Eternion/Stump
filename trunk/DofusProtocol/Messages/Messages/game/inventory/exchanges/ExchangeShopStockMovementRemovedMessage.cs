using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeShopStockMovementRemovedMessage : Message
	{
		public const uint protocolId = 5907;
		internal Boolean _isInitialized = false;
		public uint objectId = 0;
		
		public ExchangeShopStockMovementRemovedMessage()
		{
		}
		
		public ExchangeShopStockMovementRemovedMessage(uint arg1)
			: this()
		{
			initExchangeShopStockMovementRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5907;
		}
		
		public ExchangeShopStockMovementRemovedMessage initExchangeShopStockMovementRemovedMessage(uint arg1 = 0)
		{
			this.@objectId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectId = 0;
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
			this.serializeAs_ExchangeShopStockMovementRemovedMessage(arg1);
		}
		
		public void serializeAs_ExchangeShopStockMovementRemovedMessage(BigEndianWriter arg1)
		{
			if ( this.@objectId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectId + ") on element objectId.");
			}
			arg1.WriteInt((int)this.@objectId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeShopStockMovementRemovedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeShopStockMovementRemovedMessage(BigEndianReader arg1)
		{
			this.@objectId = (uint)arg1.ReadInt();
			if ( this.@objectId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectId + ") on element of ExchangeShopStockMovementRemovedMessage.objectId.");
			}
		}
		
	}
}
