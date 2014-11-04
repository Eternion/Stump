

// Generated on 10/28/2014 16:37:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameActionMark
    {
        public const short Id = 351;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int markAuthorId;
        public int markSpellId;
        public short markId;
        public sbyte markType;
        public IEnumerable<Types.GameActionMarkedCell> cells;
        
        public GameActionMark()
        {
        }
        
        public GameActionMark(int markAuthorId, int markSpellId, short markId, sbyte markType, IEnumerable<Types.GameActionMarkedCell> cells)
        {
            this.markAuthorId = markAuthorId;
            this.markSpellId = markSpellId;
            this.markId = markId;
            this.markType = markType;
            this.cells = cells;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(markAuthorId);
            writer.WriteInt(markSpellId);
            writer.WriteShort(markId);
            writer.WriteSByte(markType);
            var cells_before = writer.Position;
            var cells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in cells)
            {
                 entry.Serialize(writer);
                 cells_count++;
            }
            var cells_after = writer.Position;
            writer.Seek((int)cells_before);
            writer.WriteUShort((ushort)cells_count);
            writer.Seek((int)cells_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            markAuthorId = reader.ReadInt();
            markSpellId = reader.ReadInt();
            if (markSpellId < 0)
                throw new Exception("Forbidden value on markSpellId = " + markSpellId + ", it doesn't respect the following condition : markSpellId < 0");
            markId = reader.ReadShort();
            markType = reader.ReadSByte();
            var limit = reader.ReadUShort();
            var cells_ = new Types.GameActionMarkedCell[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells_[i] = new Types.GameActionMarkedCell();
                 cells_[i].Deserialize(reader);
            }
            cells = cells_;
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + sizeof(sbyte) + sizeof(short) + cells.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}