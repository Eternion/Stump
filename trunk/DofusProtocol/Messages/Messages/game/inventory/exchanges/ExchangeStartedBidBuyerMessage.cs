using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartedBidBuyerMessage : Message
	{
		public const uint protocolId = 5904;
		internal Boolean _isInitialized = false;
		public SellerBuyerDescriptor buyerDescriptor;
		
		public ExchangeStartedBidBuyerMessage()
		{
			this.buyerDescriptor = new SellerBuyerDescriptor();
		}
		
		public ExchangeStartedBidBuyerMessage(SellerBuyerDescriptor arg1)
			: this()
		{
			initExchangeStartedBidBuyerMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5904;
		}
		
		public ExchangeStartedBidBuyerMessage initExchangeStartedBidBuyerMessage(SellerBuyerDescriptor arg1 = null)
		{
			this.buyerDescriptor = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.buyerDescriptor = new SellerBuyerDescriptor();
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
			this.serializeAs_ExchangeStartedBidBuyerMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedBidBuyerMessage(BigEndianWriter arg1)
		{
			this.buyerDescriptor.serializeAs_SellerBuyerDescriptor(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedBidBuyerMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedBidBuyerMessage(BigEndianReader arg1)
		{
			this.buyerDescriptor = new SellerBuyerDescriptor();
			this.buyerDescriptor.deserialize(arg1);
		}
		
	}
}
