using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions
{
    public class PriorityOperator : ConditionExpression
    {
        public ConditionExpression Expression
        {
            get;
            set;
        }
            
        public override bool Eval(Character character)
        {
            return Expression.Eval(character);
        }

        public override string ToString()
        {
            return string.Format("({0})", Expression);
        }
    }
}