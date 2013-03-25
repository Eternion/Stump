
// Generated on 03/25/2013 19:24:20
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartedBidSellerMessage : Message
    {
        public const uint Id = 5905;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.SellerBuyerDescriptor sellerDescriptor;
        public IEnumerable<Types.ObjectItemToSellInBid> objectsInfos;
        
        public ExchangeStartedBidSellerMessage()
        {
        }
        
        public ExchangeStartedBidSellerMessage(Types.SellerBuyerDescriptor sellerDescriptor, IEnumerable<Types.ObjectItemToSellInBid> objectsInfos)
        {
            this.sellerDescriptor = sellerDescriptor;
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            sellerDescriptor.Serialize(writer);
            writer.WriteUShort((ushort)objectsInfos.Count());
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sellerDescriptor = new Types.SellerBuyerDescriptor();
            sellerDescriptor.Deserialize(reader);
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItemToSellInBid[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectsInfos as Types.ObjectItemToSellInBid[])[i] = new Types.ObjectItemToSellInBid();
                 (objectsInfos as Types.ObjectItemToSellInBid[])[i].Deserialize(reader);
            }
        }
        
    }
    
}