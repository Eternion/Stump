

// Generated on 10/27/2014 19:57:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkHumanVendorMessage : Message
    {
        public const uint Id = 5767;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int sellerId;
        public IEnumerable<Types.ObjectItemToSellInHumanVendorShop> objectsInfos;
        
        public ExchangeStartOkHumanVendorMessage()
        {
        }
        
        public ExchangeStartOkHumanVendorMessage(int sellerId, IEnumerable<Types.ObjectItemToSellInHumanVendorShop> objectsInfos)
        {
            this.sellerId = sellerId;
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(sellerId);
            var objectsInfos_before = writer.Position;
            var objectsInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
                 objectsInfos_count++;
            }
            var objectsInfos_after = writer.Position;
            writer.Seek((int)objectsInfos_before);
            writer.WriteUShort((ushort)objectsInfos_count);
            writer.Seek((int)objectsInfos_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            sellerId = reader.ReadInt();
            if (sellerId < 0)
                throw new Exception("Forbidden value on sellerId = " + sellerId + ", it doesn't respect the following condition : sellerId < 0");
            var limit = reader.ReadUShort();
            var objectsInfos_ = new Types.ObjectItemToSellInHumanVendorShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos_[i] = new Types.ObjectItemToSellInHumanVendorShop();
                 objectsInfos_[i].Deserialize(reader);
            }
            objectsInfos = objectsInfos_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + objectsInfos.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}