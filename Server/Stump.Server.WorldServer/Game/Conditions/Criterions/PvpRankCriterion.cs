using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class PvpRankCriterion : Criterion
    {
        public const string Identifier = "PP";
        public const string Identifier2 = "Pp";

        public sbyte Rank
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.AlignmentGrade, Rank) && character.PvPEnabled;
        }

        public override void Build()
        {
            sbyte rank;

            if (!sbyte.TryParse(Literal, out rank))
                throw new Exception(string.Format("Cannot build PvpRankCriterion, {0} is not a valid rank", Literal));

            Rank = rank;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}