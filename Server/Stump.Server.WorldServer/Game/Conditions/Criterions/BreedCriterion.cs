using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class BreedCriterion : Criterion
    {
        public const string Identifier = "PG";

        public int Breed
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            return Compare(Breed, character.Breed.Id);
        }

        public override void Build()
        {
            int breed;

            if (!int.TryParse(Literal, out breed))
                throw new Exception(string.Format("Cannot build BreedCriterion, {0} is not a valid breed id", Literal));

            Breed = breed;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}