using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class WeightCriterion : Criterion
    {
        public const string Identifier = "PW";

        public int Weight
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(character.Inventory.Weight, Weight);
        }

        public override void Build()
        {
            int weight;

            if (!int.TryParse(Literal, out weight))
                throw new Exception(string.Format("Cannot build WeightCriterion, {0} is not a valid weight", Literal));

            Weight = weight;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}