using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights.Triggers;

namespace Stump.Server.WorldServer.Game.Maps.Pathfinding
{
    public class CellInformation
    {
        public CellInformation(Cell cell, bool walkable)
        {
            Cell = cell;
            Walkable = walkable;
        }

        public CellInformation(Cell cell, bool walkable, bool fighting)
        {
            Cell = cell;
            Walkable = walkable;
            Fighting = fighting;
        }

        public CellInformation(Cell cell, bool walkable, bool fighting, bool useAI, int efficience, Trap trap, Glyph glyph)
        {
            Cell = cell;
            Walkable = walkable;
            Fighting = fighting;
            UseAI = useAI;
            Efficience = efficience;
            Trap = trap;
            Glyph = glyph;
        }

        public Cell Cell
        {
            get;
            set;
        }

        public bool Walkable
        {
            get;
            set;
        }

        public bool Fighting
        {
            get;
            set;
        }

        public bool UseAI
        {
            get;
            set;
        }

        public int Efficience
        {
            get;
            set;
        }

        public Trap Trap
        {
            get;
            set;
        }

        public Glyph Glyph
        {
            get;
            set;
        }
    }
}