

// Generated on 08/04/2015 13:24:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DebugHighlightCellsMessage : Message
    {
        public const uint Id = 2001;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int color;
        public IEnumerable<short> cells;
        
        public DebugHighlightCellsMessage()
        {
        }
        
        public DebugHighlightCellsMessage(int color, IEnumerable<short> cells)
        {
            this.color = color;
            this.cells = cells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(color);
            var cells_before = writer.Position;
            var cells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in cells)
            {
                 writer.WriteVarShort(entry);
                 cells_count++;
            }
            var cells_after = writer.Position;
            writer.Seek((int)cells_before);
            writer.WriteUShort((ushort)cells_count);
            writer.Seek((int)cells_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            color = reader.ReadInt();
            var limit = reader.ReadUShort();
            var cells_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells_[i] = reader.ReadVarShort();
            }
            cells = cells_;
        }
        
    }
    
}