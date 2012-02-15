using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class SexCriterion : Criterion
    {
        public const string Identifier = "PS";

        public int Sex
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare((int)character.Sex, Sex);
        }

        public override void Build()
        {
            int sex;

            if (!int.TryParse(Literal, out sex))
                throw new Exception(string.Format("Cannot build SexCriterion, {0} is not a valid sex", Literal));

            Sex = sex;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}