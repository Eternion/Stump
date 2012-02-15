using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class MonthCriterion : Criterion
    {
        public const string Identifier = "SG";

        public int Month
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(DateTime.Now.Month, Month);
        }

        public override void Build()
        {
            int month;

            if (!int.TryParse(Literal, out month) || month < 1 || month > 12)
                throw new Exception(string.Format("Cannot build MonthCriterion, {0} is not a valid month", Literal));

            Month = month;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}