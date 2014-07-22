using System;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaRankFormulas
    {
        public const int KFactor = 32;

        public static double GetWinningChances(int currentRank, int opposantRank)
        {
            return 1/(1 + Math.Pow(10d,(opposantRank - currentRank)/400d));
        }

        public static int AdjustRank(int currentRank, int opposantRank, bool won)
        {
            var rank = (int)(currentRank + KFactor*((won ? 1 : 0) - GetWinningChances(currentRank, opposantRank)));

            return rank > 0 ? rank : 0;
        }
    }
}