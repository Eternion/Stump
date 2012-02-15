using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SubAreaCriterion : Criterion
    {
        public const string Identifier = "PB";

        public int SubArea
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.SubArea.Id, SubArea);
        }

        public override void Build()
        {
            int subarea;

            if (!int.TryParse(Literal, out subarea))
                throw new Exception(string.Format("Cannot build SubAreaCriterion, {0} is not a valid subarea id", Literal));

            SubArea = subarea;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}