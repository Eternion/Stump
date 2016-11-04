using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class UnknownCriterion : Criterion
    {
        public const string Identifier = "Pc";
        public const string Identifier2 = "Mw";

        public override bool Eval(Character character)
        {
            return true;
        }

        public override void Build()
        {
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
