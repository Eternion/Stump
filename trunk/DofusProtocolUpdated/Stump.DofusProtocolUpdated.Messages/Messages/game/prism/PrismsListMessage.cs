

// Generated on 12/12/2013 16:57:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismsListMessage : Message
    {
        public const uint Id = 6440;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.PrismSubareaEmptyInfo> prisms;
        
        public PrismsListMessage()
        {
        }
        
        public PrismsListMessage(IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
        {
            this.prisms = prisms;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)prisms.Count());
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prisms as Types.PrismSubareaEmptyInfo[])[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 (prisms as Types.PrismSubareaEmptyInfo[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + prisms.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}