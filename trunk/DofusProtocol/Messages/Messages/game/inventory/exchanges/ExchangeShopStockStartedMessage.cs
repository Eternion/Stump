using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeShopStockStartedMessage : Message
	{
		public const uint protocolId = 5910;
		internal Boolean _isInitialized = false;
		public List<ObjectItemToSell> objectsInfos;
		
		public ExchangeShopStockStartedMessage()
		{
			this.@objectsInfos = new List<ObjectItemToSell>();
		}
		
		public ExchangeShopStockStartedMessage(List<ObjectItemToSell> arg1)
			: this()
		{
			initExchangeShopStockStartedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5910;
		}
		
		public ExchangeShopStockStartedMessage initExchangeShopStockStartedMessage(List<ObjectItemToSell> arg1)
		{
			this.@objectsInfos = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectsInfos = new List<ObjectItemToSell>();
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
			this.serializeAs_ExchangeShopStockStartedMessage(arg1);
		}
		
		public void serializeAs_ExchangeShopStockStartedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemToSell(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeShopStockStartedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeShopStockStartedMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSell()) as ObjectItemToSell).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemToSell)loc3);
				++loc2;
			}
		}
		
	}
}
