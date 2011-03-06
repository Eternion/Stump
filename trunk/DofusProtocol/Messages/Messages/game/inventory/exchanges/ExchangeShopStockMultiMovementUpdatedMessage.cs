using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeShopStockMultiMovementUpdatedMessage : Message
	{
		public const uint protocolId = 6038;
		internal Boolean _isInitialized = false;
		public List<ObjectItemToSell> objectInfoList;
		
		public ExchangeShopStockMultiMovementUpdatedMessage()
		{
			this.@objectInfoList = new List<ObjectItemToSell>();
		}
		
		public ExchangeShopStockMultiMovementUpdatedMessage(List<ObjectItemToSell> arg1)
			: this()
		{
			initExchangeShopStockMultiMovementUpdatedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6038;
		}
		
		public ExchangeShopStockMultiMovementUpdatedMessage initExchangeShopStockMultiMovementUpdatedMessage(List<ObjectItemToSell> arg1)
		{
			this.@objectInfoList = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectInfoList = new List<ObjectItemToSell>();
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
			this.serializeAs_ExchangeShopStockMultiMovementUpdatedMessage(arg1);
		}
		
		public void serializeAs_ExchangeShopStockMultiMovementUpdatedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectInfoList.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectInfoList.Count )
			{
				this.@objectInfoList[loc1].serializeAs_ObjectItemToSell(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeShopStockMultiMovementUpdatedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeShopStockMultiMovementUpdatedMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSell()) as ObjectItemToSell).deserialize(arg1);
				this.@objectInfoList.Add((ObjectItemToSell)loc3);
				++loc2;
			}
		}
		
	}
}
