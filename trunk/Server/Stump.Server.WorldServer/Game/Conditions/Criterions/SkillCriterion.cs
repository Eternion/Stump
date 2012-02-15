using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SkillCriterion : Criterion
    {
        public const string Identifier = "Pi";
        public const string Identifier2 = "PI";

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