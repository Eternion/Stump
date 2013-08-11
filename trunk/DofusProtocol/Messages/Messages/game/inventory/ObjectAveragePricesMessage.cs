

// Generated on 08/11/2013 11:28:50
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
            writer.WriteUShort((ushort)ids.Count());
            foreach (var entry in ids)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)avgPrices.Count());
            foreach (var entry in avgPrices)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ids as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            avgPrices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (avgPrices as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ids.Sum(x => sizeof(short)) + sizeof(short) + avgPrices.Sum(x => sizeof(int));
        }
        
    }
    
}