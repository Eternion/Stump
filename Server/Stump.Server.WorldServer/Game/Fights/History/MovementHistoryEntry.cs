using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class MovementHistoryEntry
    {
        public MovementHistoryEntry(Cell cell, int currentRound)
        {
            Cell = cell;
            Round = currentRound;
        }

        public Cell Cell
        {
            get;
            private set;
        }

        public int Round
        {
            get;
            private set;
        }
    }
}