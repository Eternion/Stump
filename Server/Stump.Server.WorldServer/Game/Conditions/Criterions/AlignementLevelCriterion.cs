using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class AlignementLevelCriterion : Criterion
    {
        public const string Identifier = "Pa";

        public int Level
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
            int level;

            if (!int.TryParse(Literal, out level))
                throw new Exception(string.Format("Cannot build AlignementLevelCriterion, {0} is not a valid alignement level", Literal));
        
            Level = level;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}