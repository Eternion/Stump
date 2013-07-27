

// Generated on 07/26/2013 22:51:08
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)angelsSubAreas.Count());
            foreach (var entry in angelsSubAreas)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)evilsSubAreas.Count());
            foreach (var entry in evilsSubAreas)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            angelsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (angelsSubAreas as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            evilsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (evilsSubAreas as short[])[i] = reader.ReadShort();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + angelsSubAreas.Sum(x => sizeof(short)) + sizeof(short) + evilsSubAreas.Sum(x => sizeof(short));
        }
        
    }
    
}