using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SoulStoneCriterion : Criterion
    {
        public const string Identifier = "PA";

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