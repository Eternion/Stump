using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class MarkShape
    {
        private readonly Zone m_zone;
        private bool m_customForm;
        private Cell[] m_cells;

        public MarkShape(IFight fight, Cell cell, SpellShapeEnum spellShape, GameActionMarkCellsTypeEnum shape, byte size, Color color)
        {
            Fight = fight;
            Cell = cell;
            Shape = shape;
            Size = size;
            Color = color;

            m_zone = new Zone(spellShape, size);
            CheckCells(m_zone.GetCells(Cell, fight.Map));
        }

        public MarkShape(IFight fight, Cell cell, GameActionMarkCellsTypeEnum shape, byte size, Color color)
            : this(fight, cell, SpellShapeEnum.C, shape, size, color)
        {
        }

        public IFight Fight
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

        public GameActionMarkedCell[] GetGameActionMarkedCells()
        {
            if (!m_customForm && m_cells.Length > 1 && Shape == GameActionMarkCellsTypeEnum.CELLS_CIRCLE || Shape == GameActionMarkCellsTypeEnum.CELLS_CROSS)
                return new[] {new GameActionMarkedCell(Cell.Id, (sbyte)Size, Color.ToArgb() & 0xFFFFFF, (sbyte) Shape)};
            
            return m_cells.Select(x => new GameActionMarkedCell(x.Id, 0, Color.ToArgb() & 0xFFFFFF, (sbyte)Shape)).ToArray();
        }

        private void CheckCells(Cell[] cells)
        {
            var validCells = new List<Cell>();
            foreach (var cell in cells)
            {
                if (cell.Walkable)
                    validCells.Add(cell);
                else
                    m_customForm = true;
            }

            m_cells = validCells.ToArray();
        }
    }
}