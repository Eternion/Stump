using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class HasTitleCriterion : Criterion
    {
        public const string Identifier = "HT";
        public const string Identifier2 = "Ot";

        public short Title
        {
            get;
            set;
        }

        public override bool Eval(Character character)
            => Operator == ComparaisonOperatorEnum.EQUALS ? character.HasTitle(Title) : !character.HasTitle(Title);

        public override void Build()
        {
            short title;

            if (!short.TryParse(Literal, out title))
                throw new Exception(string.Format("Cannot build LevelCriterion, {0} is not a valid title", Literal));

            Title = title;
        }

        public override string ToString() => FormatToString(Identifier);
    }
}
