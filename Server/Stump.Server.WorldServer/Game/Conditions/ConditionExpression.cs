using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions
{
    public abstract class ConditionExpression
    {
        public static ConditionExpression Parse(string str)
        {
            var parser = new ConditionParser(str);
            return parser.Parse();
        }

        public abstract bool Eval(Character character);
    }
}