using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class RideCriterion : Criterion
    {
        public const string Identifier = "Pf";

        public bool Mounted
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
            int mounted;

            if (!int.TryParse(Literal, out mounted))
                throw new Exception(string.Format("Cannot build RideCriterion, {0} is not a valid mount state", Literal));

            Mounted = mounted != 0;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}