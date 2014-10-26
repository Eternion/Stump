

// Generated on 10/26/2014 23:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectTransfertListFromInvMessage : Message
    {
        public const uint Id = 6183;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> ids;
        
        public ExchangeObjectTransfertListFromInvMessage()
        {
        }
        
        public ExchangeObjectTransfertListFromInvMessage(IEnumerable<int> ids)
        {
            this.ids = ids;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var ids_before = writer.Position;
            var ids_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
                 ids_count++;
            }
            var ids_after = writer.Position;
            writer.Seek((int)ids_before);
            writer.WriteUShort((ushort)ids_count);
            writer.Seek((int)ids_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var ids_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids_[i] = reader.ReadInt();
            }
            ids = ids_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ids.Sum(x => sizeof(int));
        }
        
    }
    
}