using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class MariedCriterion : Criterion
    {
        public const string Identifier = "PR";

        public bool Married
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
            int married;

            if (!int.TryParse(Literal, out married) || (married != 1 && married != 2))
                throw new Exception(string.Format("Cannot build MariedCriterion, {0} is not a valid married condition (1 or 2)", Literal));

            Married = married == 1;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}