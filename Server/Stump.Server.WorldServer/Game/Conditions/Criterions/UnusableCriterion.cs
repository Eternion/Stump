using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class UnusableCriterion : Criterion
    {
        public const string Identifier = "BI";

        public override bool Eval(Character character)
        {
            return false;
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