using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class LevelCriterion : Criterion
    {
        public const string Identifier = "PL";

        public int Level
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.Level, Level);
        }

        public override void Build()
        {    
            int level;

            if (!int.TryParse(Literal, out level))
                throw new Exception(string.Format("Cannot build LevelCriterion, {0} is not a valid level", Literal));

            Level = level;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}