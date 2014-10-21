using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class HasOrnament : Criterion
    {
        public const string Identifier = "HO";

        public short Ornament
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return character.HasOrnament(Ornament);
        }

        public override void Build()
        {
            short ornament;

            if (!short.TryParse(Literal, out ornament))
                throw new Exception(string.Format("Cannot build LevelCriterion, {0} is not a valid ornament", Literal));

            Ornament = ornament;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}
