

// Generated on 10/27/2014 19:57:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectTransfertListWithQuantityToInvMessage : Message
    {
        public const uint Id = 6470;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> ids;
        public IEnumerable<int> qtys;
        
        public ExchangeObjectTransfertListWithQuantityToInvMessage()
        {
        }
        
        public ExchangeObjectTransfertListWithQuantityToInvMessage(IEnumerable<int> ids, IEnumerable<int> qtys)
        {
            this.ids = ids;
            this.qtys = qtys;
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

            var qtys_before = writer.Position;
            var qtys_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in qtys)
            {
                 writer.WriteInt(entry);
                 qtys_count++;
            }
            var qtys_after = writer.Position;
            writer.Seek((int)qtys_before);
            writer.WriteUShort((ushort)qtys_count);
            writer.Seek((int)qtys_after);

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
            limit = reader.ReadUShort();
            var qtys_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 qtys_[i] = reader.ReadInt();
            }
            qtys = qtys_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ids.Sum(x => sizeof(int)) + sizeof(short) + qtys.Sum(x => sizeof(int));
        }
        
    }
    
}