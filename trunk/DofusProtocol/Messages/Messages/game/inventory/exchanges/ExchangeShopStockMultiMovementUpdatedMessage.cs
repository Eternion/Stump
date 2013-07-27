

// Generated on 07/26/2013 22:51:05
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeShopStockMultiMovementUpdatedMessage : Message
    {
        public const uint Id = 6038;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItemToSell> objectInfoList;
        
        public ExchangeShopStockMultiMovementUpdatedMessage()
        {
        }
        
        public ExchangeShopStockMultiMovementUpdatedMessage(IEnumerable<Types.ObjectItemToSell> objectInfoList)
        {
            this.objectInfoList = objectInfoList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objectInfoList.Count());
            foreach (var entry in objectInfoList)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectInfoList = new Types.ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectInfoList as Types.ObjectItemToSell[])[i] = new Types.ObjectItemToSell();
                 (objectInfoList as Types.ObjectItemToSell[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + objectInfoList.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}