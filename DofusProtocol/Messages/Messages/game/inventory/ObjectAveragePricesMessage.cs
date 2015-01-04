

// Generated on 01/04/2015 11:54:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectAveragePricesMessage : Message
    {
        public const uint Id = 6335;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> ids;
        public IEnumerable<int> avgPrices;
        
        public ObjectAveragePricesMessage()
        {
        }
        
        public ObjectAveragePricesMessage(IEnumerable<short> ids, IEnumerable<int> avgPrices)
        {
            this.ids = ids;
            this.avgPrices = avgPrices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var ids_before = writer.Position;
            var ids_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ids)
            {
                 writer.WriteVarShort(entry);
                 ids_count++;
            }
            var ids_after = writer.Position;
            writer.Seek((int)ids_before);
            writer.WriteUShort((ushort)ids_count);
            writer.Seek((int)ids_after);

            var avgPrices_before = writer.Position;
            var avgPrices_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in avgPrices)
            {
                 writer.WriteVarInt(entry);
                 avgPrices_count++;
            }
            var avgPrices_after = writer.Position;
            writer.Seek((int)avgPrices_before);
            writer.WriteUShort((ushort)avgPrices_count);
            writer.Seek((int)avgPrices_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var ids_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids_[i] = reader.ReadVarShort();
            }
            ids = ids_;
            limit = reader.ReadUShort();
            var avgPrices_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 avgPrices_[i] = reader.ReadVarInt();
            }
            avgPrices = avgPrices_;
        }
        
    }
    
}