

// Generated on 02/02/2016 14:14:48
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
        
        public double markAuthorId;
        public sbyte markTeamId;
        public int markSpellId;
        public sbyte markSpellLevel;
        public short markId;
        public sbyte markType;
        public short markimpactCell;
        public IEnumerable<Types.GameActionMarkedCell> cells;
        public bool active;
        
        public GameActionMark()
        {
        }
        
        public GameActionMark(double markAuthorId, sbyte markTeamId, int markSpellId, sbyte markSpellLevel, short markId, sbyte markType, short markimpactCell, IEnumerable<Types.GameActionMarkedCell> cells, bool active)
        {
            this.markAuthorId = markAuthorId;
            this.markTeamId = markTeamId;
            this.markSpellId = markSpellId;
            this.markSpellLevel = markSpellLevel;
            this.markId = markId;
            this.markType = markType;
            this.markimpactCell = markimpactCell;
            this.cells = cells;
            this.active = active;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(markAuthorId);
            writer.WriteSByte(markTeamId);
            writer.WriteInt(markSpellId);
            writer.WriteSByte(markSpellLevel);
            writer.WriteShort(markId);
            writer.WriteSByte(markType);
            writer.WriteShort(markimpactCell);
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

            writer.WriteBoolean(active);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            markAuthorId = reader.ReadDouble();
            if (markAuthorId < -9.007199254740992E15 || markAuthorId > 9.007199254740992E15)
                throw new Exception("Forbidden value on markAuthorId = " + markAuthorId + ", it doesn't respect the following condition : markAuthorId < -9.007199254740992E15 || markAuthorId > 9.007199254740992E15");
            markTeamId = reader.ReadSByte();
            if (markTeamId < 0)
                throw new Exception("Forbidden value on markTeamId = " + markTeamId + ", it doesn't respect the following condition : markTeamId < 0");
            markSpellId = reader.ReadInt();
            if (markSpellId < 0)
                throw new Exception("Forbidden value on markSpellId = " + markSpellId + ", it doesn't respect the following condition : markSpellId < 0");
            markSpellLevel = reader.ReadSByte();
            if (markSpellLevel < 1 || markSpellLevel > 6)
                throw new Exception("Forbidden value on markSpellLevel = " + markSpellLevel + ", it doesn't respect the following condition : markSpellLevel < 1 || markSpellLevel > 6");
            markId = reader.ReadShort();
            markType = reader.ReadSByte();
            markimpactCell = reader.ReadShort();
            if (markimpactCell < -1 || markimpactCell > 559)
                throw new Exception("Forbidden value on markimpactCell = " + markimpactCell + ", it doesn't respect the following condition : markimpactCell < -1 || markimpactCell > 559");
            var limit = reader.ReadUShort();
            var cells_ = new Types.GameActionMarkedCell[limit];
            for (int i = 0; i < limit; i++)
            {
                 cells_[i] = new Types.GameActionMarkedCell();
                 cells_[i].Deserialize(reader);
            }
            cells = cells_;
            active = reader.ReadBoolean();
        }
        
        
    }
    
}