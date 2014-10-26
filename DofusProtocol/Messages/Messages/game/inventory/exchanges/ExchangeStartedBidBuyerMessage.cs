

// Generated on 10/26/2014 23:29:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartedBidBuyerMessage : Message
    {
        public const uint Id = 5904;
        public override uint MessageId
        {
            get { return Id; }
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
        
        public override int GetSerializationSize()
        {
            return buyerDescriptor.GetSerializationSize();
        }
        
    }
    
}