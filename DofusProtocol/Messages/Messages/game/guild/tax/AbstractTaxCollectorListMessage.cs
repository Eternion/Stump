

// Generated on 08/04/2015 00:37:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AbstractTaxCollectorListMessage : Message
    {
        public const uint Id = 6568;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.TaxCollectorInformations> informations;
        
        public AbstractTaxCollectorListMessage()
        {
        }
        
        public AbstractTaxCollectorListMessage(IEnumerable<Types.TaxCollectorInformations> informations)
        {
            this.informations = informations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var informations_before = writer.Position;
            var informations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in informations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 informations_count++;
            }
            var informations_after = writer.Position;
            writer.Seek((int)informations_before);
            writer.WriteUShort((ushort)informations_count);
            writer.Seek((int)informations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var informations_ = new Types.TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 informations_[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
                 informations_[i].Deserialize(reader);
            }
            informations = informations_;
        }
        
    }
    
}