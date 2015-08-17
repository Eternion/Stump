using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class MarkShape
    {
        private readonly Zone m_zone;
        private readonly Cell[] m_cells;

        public MarkShape(IFight fight, Cell cell, GameActionMarkCellsTypeEnum shape, byte size, Color color)
        {
            Fight = fight;
            Cell = cell;
            Shape = shape;
            Size = size;
            Color = color;

            switch(Shape)
            {
                case GameActionMarkCellsTypeEnum.CELLS_CROSS:
                    m_zone = new Zone(SpellShapeEnum.Q, size);
                    break;
                case GameActionMarkCellsTypeEnum.CELLS_SQUARE:
                    m_zone = new Zone(SpellShapeEnum.G, size);
                    break;
                default:
                    m_zone = new Zone(SpellShapeEnum.C, size);
                    break;
            }

            m_cells = m_zone.GetCells(Cell, fight.Map);
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
            var markedCells = new List<GameActionMarkedCell>();

            foreach (var cell in m_cells)
                markedCells.Add(new GameActionMarkedCell(cell.Id, 0, Color.ToArgb() & 0xFFFFFF, (sbyte)Shape));

            return markedCells.ToArray();
        }

        public GameActionMarkedCell GetGameActionMarkedCell()
        {
            return new GameActionMarkedCell(Cell.Id, (sbyte) Size, Color.ToArgb() & 0xFFFFFF, (sbyte)Shape);
        }
    }
}