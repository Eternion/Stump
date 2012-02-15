using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class KamaCriterion : Criterion
    {
        public const string Identifier = "PK";

        public int Kamas
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.Kamas, Kamas);
        }

        public override void Build()
        {
            int kamas;

            if (!int.TryParse(Literal, out kamas))
                throw new Exception(string.Format("Cannot build KamaCriterion, {0} is not a valid kamas amount", Literal));

            Kamas = kamas;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}