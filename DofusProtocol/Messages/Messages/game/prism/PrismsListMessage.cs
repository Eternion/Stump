

// Generated on 11/16/2015 14:26:26
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
            var prisms_before = writer.Position;
            var prisms_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 prisms_count++;
            }
            var prisms_after = writer.Position;
            writer.Seek((int)prisms_before);
            writer.WriteUShort((ushort)prisms_count);
            writer.Seek((int)prisms_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var prisms_ = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 prisms_[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 prisms_[i].Deserialize(reader);
            }
            prisms = prisms_;
        }
        
    }
    
}