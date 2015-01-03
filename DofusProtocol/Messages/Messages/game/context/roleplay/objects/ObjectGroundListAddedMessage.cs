

// Generated on 12/29/2014 21:12:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectGroundListAddedMessage : Message
    {
        public const uint Id = 5925;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> cells;
        public IEnumerable<short> referenceIds;
        
        public ObjectGroundListAddedMessage()
        {
        }
        
        public ObjectGroundListAddedMessage(IEnumerable<short> cells, IEnumerable<short> referenceIds)
        {
            this.cells = cells;
            this.referenceIds = referenceIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var cells_before = writer.Position;
            var cells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in cells)
            {
                 writer.WriteShort(entry);
                 cells_count++;
            }
            var cells_after = writer.Position;
            writer.Seek((int)cells_before);
            writer.WriteUShort((ushort)cells_count);
            writer.Seek((int)cells_after);

            var referenceIds_before = writer.Position;
            var referenceIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in referenceIds)
            {
                 writer.WriteShort(entry);
                 referenceIds_count++;
            }
            var referenceIds_after = writer.Position;
            writer.Seek((int)referenceIds_before);
            writer.WriteUShort((ushort)referenceIds_count);
            writer.Seek((int)referenceIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var cells_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells_[i] = reader.ReadShort();
            }
            cells = cells_;
            limit = reader.ReadUShort();
            var referenceIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 referenceIds_[i] = reader.ReadShort();
            }
            referenceIds = referenceIds_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + cells.Sum(x => sizeof(short)) + sizeof(short) + referenceIds.Sum(x => sizeof(short));
        }
        
    }
    
}