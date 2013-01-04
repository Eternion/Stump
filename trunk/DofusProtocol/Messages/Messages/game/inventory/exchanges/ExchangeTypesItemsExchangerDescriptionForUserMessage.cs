
// Generated on 01/04/2013 14:35:58
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeTypesItemsExchangerDescriptionForUserMessage : Message
    {
        public const uint Id = 5752;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.BidExchangerObjectInfo> itemTypeDescriptions;
        
        public ExchangeTypesItemsExchangerDescriptionForUserMessage()
        {
        }
        
        public ExchangeTypesItemsExchangerDescriptionForUserMessage(IEnumerable<Types.BidExchangerObjectInfo> itemTypeDescriptions)
        {
            this.itemTypeDescriptions = itemTypeDescriptions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)itemTypeDescriptions.Count());
            foreach (var entry in itemTypeDescriptions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            itemTypeDescriptions = new Types.BidExchangerObjectInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemTypeDescriptions as Types.BidExchangerObjectInfo[])[i] = new Types.BidExchangerObjectInfo();
                 (itemTypeDescriptions as Types.BidExchangerObjectInfo[])[i].Deserialize(reader);
            }
        }
        
    }
    
}