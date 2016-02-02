

// Generated on 02/02/2016 14:14:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseUnsoldItemsMessage : Message
    {
        public const uint Id = 6612;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItemGenericQuantity> items;
        
        public ExchangeBidHouseUnsoldItemsMessage()
        {
        }
        
        public ExchangeBidHouseUnsoldItemsMessage(IEnumerable<Types.ObjectItemGenericQuantity> items)
        {
            this.items = items;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var items_before = writer.Position;
            var items_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in items)
            {
                 entry.Serialize(writer);
                 items_count++;
            }
            var items_after = writer.Position;
            writer.Seek((int)items_before);
            writer.WriteUShort((ushort)items_count);
            writer.Seek((int)items_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var items_ = new Types.ObjectItemGenericQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 items_[i] = new Types.ObjectItemGenericQuantity();
                 items_[i].Deserialize(reader);
            }
            items = items_;
        }
        
    }
    
}