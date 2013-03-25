
// Generated on 03/25/2013 19:23:58
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)cells.Count());
            foreach (var entry in cells)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            color = reader.ReadInt();
            var limit = reader.ReadUShort();
            cells = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (cells as short[])[i] = reader.ReadShort();
            }
        }
        
    }
    
}