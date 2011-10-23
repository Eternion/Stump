using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes;

namespace Stump.Server.WorldServer.Worlds.Fights.Triggers
{
    public class MarkShape
    {
        private readonly Zone m_zone;
        private readonly Cell[] m_cells;

        public MarkShape(Fight fight, Cell cell, GameActionMarkCellsTypeEnum shape, sbyte size, Color color)
        {
            Fight = fight;
            Cell = cell;
            Shape = shape;
            Size = size;
            Color = color;

            m_zone = Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS ?
                new Zone(SpellShapeEnum.Q, (uint)size) : new Zone(SpellShapeEnum.C, (uint)size);
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

        public sbyte Size
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
            return new GameActionMarkedCell(Cell.Id, Size, Color.ToArgb() & 0xFFFFFF, (sbyte)Shape);
        }
    }
}