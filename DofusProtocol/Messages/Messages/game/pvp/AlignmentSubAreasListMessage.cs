

// Generated on 03/02/2014 20:42:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AlignmentSubAreasListMessage : Message
    {
        public const uint Id = 6059;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> angelsSubAreas;
        public IEnumerable<short> evilsSubAreas;
        
        public AlignmentSubAreasListMessage()
        {
        }
        
        public AlignmentSubAreasListMessage(IEnumerable<short> angelsSubAreas, IEnumerable<short> evilsSubAreas)
        {
            this.angelsSubAreas = angelsSubAreas;
            this.evilsSubAreas = evilsSubAreas;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var angelsSubAreas_before = writer.Position;
            var angelsSubAreas_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in angelsSubAreas)
            {
                 writer.WriteShort(entry);
                 angelsSubAreas_count++;
            }
            var angelsSubAreas_after = writer.Position;
            writer.Seek((int)angelsSubAreas_before);
            writer.WriteUShort((ushort)angelsSubAreas_count);
            writer.Seek((int)angelsSubAreas_after);

            var evilsSubAreas_before = writer.Position;
            var evilsSubAreas_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in evilsSubAreas)
            {
                 writer.WriteShort(entry);
                 evilsSubAreas_count++;
            }
            var evilsSubAreas_after = writer.Position;
            writer.Seek((int)evilsSubAreas_before);
            writer.WriteUShort((ushort)evilsSubAreas_count);
            writer.Seek((int)evilsSubAreas_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var angelsSubAreas_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 angelsSubAreas_[i] = reader.ReadShort();
            }
            angelsSubAreas = angelsSubAreas_;
            limit = reader.ReadUShort();
            var evilsSubAreas_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 evilsSubAreas_[i] = reader.ReadShort();
            }
            evilsSubAreas = evilsSubAreas_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + angelsSubAreas.Sum(x => sizeof(short)) + sizeof(short) + evilsSubAreas.Sum(x => sizeof(short));
        }
        
    }
    
}