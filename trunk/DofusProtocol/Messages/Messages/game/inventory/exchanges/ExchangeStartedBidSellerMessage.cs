using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartedBidSellerMessage : Message
	{
		public const uint protocolId = 5905;
		internal Boolean _isInitialized = false;
		public SellerBuyerDescriptor sellerDescriptor;
		public List<ObjectItemToSellInBid> objectsInfos;
		
		public ExchangeStartedBidSellerMessage()
		{
			this.sellerDescriptor = new SellerBuyerDescriptor();
			this.@objectsInfos = new List<ObjectItemToSellInBid>();
		}
		
		public ExchangeStartedBidSellerMessage(SellerBuyerDescriptor arg1, List<ObjectItemToSellInBid> arg2)
			: this()
		{
			initExchangeStartedBidSellerMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5905;
		}
		
		public ExchangeStartedBidSellerMessage initExchangeStartedBidSellerMessage(SellerBuyerDescriptor arg1 = null, List<ObjectItemToSellInBid> arg2 = null)
		{
			this.sellerDescriptor = arg1;
			this.@objectsInfos = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sellerDescriptor = new SellerBuyerDescriptor();
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
			this.serializeAs_ExchangeStartedBidSellerMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedBidSellerMessage(BigEndianWriter arg1)
		{
			this.sellerDescriptor.serializeAs_SellerBuyerDescriptor(arg1);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemToSellInBid(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedBidSellerMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedBidSellerMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.sellerDescriptor = new SellerBuyerDescriptor();
			this.sellerDescriptor.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSellInBid()) as ObjectItemToSellInBid).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemToSellInBid)loc3);
				++loc2;
			}
		}
		
	}
}
