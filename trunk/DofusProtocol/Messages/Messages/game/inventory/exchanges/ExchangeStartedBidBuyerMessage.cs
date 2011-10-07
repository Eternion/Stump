// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeStartedBidBuyerMessage.xml' the '03/10/2011 12:47:07'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeStartedBidBuyerMessage : Message
	{
		public const uint Id = 5904;
		public override uint MessageId
		{
			get
			{
				return 5904;
			}
		}
		
		public Types.SellerBuyerDescriptor buyerDescriptor;
		
		public ExchangeStartedBidBuyerMessage()
		{
		}
		
		public ExchangeStartedBidBuyerMessage(Types.SellerBuyerDescriptor buyerDescriptor)
		{
			this.buyerDescriptor = buyerDescriptor;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			buyerDescriptor.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			buyerDescriptor = new Types.SellerBuyerDescriptor();
			buyerDescriptor.Deserialize(reader);
		}
	}
}
