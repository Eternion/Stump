
// Generated on 03/25/2013 19:24:20
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectTransfertListToInvMessage : Message
    {
        public const uint Id = 6039;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> ids;
        
        public ExchangeObjectTransfertListToInvMessage()
        {
        }
        
        public ExchangeObjectTransfertListToInvMessage(IEnumerable<int> ids)
        {
            this.ids = ids;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)ids.Count());
            foreach (var entry in ids)
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
        }
        
    }
    
}