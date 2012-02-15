using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SpecializationCriterion : Criterion
    {
        public const string Identifier = "Pr";

        public override bool Eval(Character character)
        {
            return true;
        }

        public override void Build()
        {
            // todo
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}