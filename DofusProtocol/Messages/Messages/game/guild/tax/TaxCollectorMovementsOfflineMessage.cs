

// Generated on 02/02/2016 14:14:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorMovementsOfflineMessage : Message
    {
        public const uint Id = 6611;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.TaxCollectorMovement> movements;
        
        public TaxCollectorMovementsOfflineMessage()
        {
        }
        
        public TaxCollectorMovementsOfflineMessage(IEnumerable<Types.TaxCollectorMovement> movements)
        {
            this.movements = movements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var movements_before = writer.Position;
            var movements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in movements)
            {
                 entry.Serialize(writer);
                 movements_count++;
            }
            var movements_after = writer.Position;
            writer.Seek((int)movements_before);
            writer.WriteUShort((ushort)movements_count);
            writer.Seek((int)movements_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var movements_ = new Types.TaxCollectorMovement[limit];
            for (int i = 0; i < limit; i++)
            {
                 movements_[i] = new Types.TaxCollectorMovement();
                 movements_[i].Deserialize(reader);
            }
            movements = movements_;
        }
        
    }
    
}