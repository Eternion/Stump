using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AlignmentCriterion : Criterion
    {
        public const string Identifier = "Ps";


        public override bool Eval(Character character)
        {
            // todo
            return true; 
        }

        public override void Build()
        {
            int id;

            if (!int.TryParse(Literal, out id))
                throw new Exception(string.Format("Cannot build AlignmentCriterion, {0} is not a valid alignement id", Literal));
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}