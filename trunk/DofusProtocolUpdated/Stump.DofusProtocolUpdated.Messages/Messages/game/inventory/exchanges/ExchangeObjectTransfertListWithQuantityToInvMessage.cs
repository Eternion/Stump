

// Generated on 12/12/2013 16:57:16
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
            writer.WriteUShort((ushort)ids.Count());
            foreach (var entry in ids)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)qtys.Count());
            foreach (var entry in qtys)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ids as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            qtys = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (qtys as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + ids.Sum(x => sizeof(int)) + sizeof(short) + qtys.Sum(x => sizeof(int));
        }
        
    }
    
}