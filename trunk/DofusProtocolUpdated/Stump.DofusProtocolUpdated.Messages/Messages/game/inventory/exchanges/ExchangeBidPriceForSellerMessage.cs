

// Generated on 12/12/2013 16:57:14
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
        
        public ExchangeBidPriceForSellerMessage(int genericId, int averagePrice, bool allIdentical, IEnumerable<int> minimalPrices)
         : base(genericId, averagePrice)
        {
            this.allIdentical = allIdentical;
            this.minimalPrices = minimalPrices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(allIdentical);
            writer.WriteUShort((ushort)minimalPrices.Count());
            foreach (var entry in minimalPrices)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allIdentical = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            minimalPrices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (minimalPrices as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(bool) + sizeof(short) + minimalPrices.Sum(x => sizeof(int));
        }
        
    }
    
}