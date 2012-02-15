using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class PvpRankCriterion : Criterion
    {
        public const string Identifier = "Pp";
        public const string Identifier2 = "PP";

        public int Rank
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return true;
        }

        public override void Build()
        {
            int rank;

            if (!int.TryParse(Literal, out rank))
                throw new Exception(string.Format("Cannot build PvpRankCriterion, {0} is not a valid rank", Literal));

            Rank = rank;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}