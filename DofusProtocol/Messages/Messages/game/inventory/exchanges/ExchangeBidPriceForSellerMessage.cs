

// Generated on 11/16/2015 14:26:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidPriceForSellerMessage : ExchangeBidPriceMessage
    {
        public const uint Id = 6464;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool allIdentical;
        public IEnumerable<int> minimalPrices;
        
        public ExchangeBidPriceForSellerMessage()
        {
        }
        
        public ExchangeBidPriceForSellerMessage(short genericId, int averagePrice, bool allIdentical, IEnumerable<int> minimalPrices)
         : base(genericId, averagePrice)
        {
            this.allIdentical = allIdentical;
            this.minimalPrices = minimalPrices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(allIdentical);
            var minimalPrices_before = writer.Position;
            var minimalPrices_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in minimalPrices)
            {
                 writer.WriteVarInt(entry);
                 minimalPrices_count++;
            }
            var minimalPrices_after = writer.Position;
            writer.Seek((int)minimalPrices_before);
            writer.WriteUShort((ushort)minimalPrices_count);
            writer.Seek((int)minimalPrices_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allIdentical = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var minimalPrices_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 minimalPrices_[i] = reader.ReadVarInt();
            }
            minimalPrices = minimalPrices_;
        }
        
    }
    
}