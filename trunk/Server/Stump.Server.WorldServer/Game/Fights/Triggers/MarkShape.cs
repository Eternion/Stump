using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class MarkShape
    {
        private readonly Zone m_zone;
        private readonly Cell[] m_cells;

        public MarkShape(Fight fight, Cell cell, GameActionMarkCellsTypeEnum shape, byte size, Color color)
        {
            Fight = fight;
            Cell = cell;
            Shape = shape;
            Size = size;
            Color = color;

            m_zone = Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ?
                new Zone(SpellShapeEnum.Q, (byte)size) : new Zone(SpellShapeEnum.C, size);
            m_cells = m_zone.GetCells(Cell, fight.Map);
        }

        public Fight Fight
        {
            get;
            private set;
        }

        public Cell Cell
        {
            get;
            private set;
        }

        public GameActionMarkCellsTypeEnum Shape
        {
            get;
            private set;
        }

        public byte Size
        {
            get;
            private set;
        }

        public Color Color
        {
            get;
            private set;
        }

        public Cell[] GetCells()
        {
            return m_cells;
        }

        public GameActionMarkedCell GetGameActionMarkedCell()
        {
            return new GameActionMarkedCell(Cell.Id, (sbyte) Size, Color.ToArgb() & 0xFFFFFF, (sbyte)Shape);
        }
    }
}