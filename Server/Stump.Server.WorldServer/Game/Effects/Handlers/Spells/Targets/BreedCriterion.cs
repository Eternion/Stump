using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets
{
    public class BreedCriterion : TargetCriterion
    {
        public BreedCriterion(int breed, bool caster, bool required)
        {
            Breed = breed;
            Caster = caster;
            Required = required;
        }

        public int Breed
        {
            get;
            set;
        }

        public bool Caster
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public override bool IsDisjonction => false;

        public override bool IsTargetValid(FightActor actor, SpellEffectHandler handler)
        {
            if (Caster)
                actor = handler.Caster;

            if (actor is CharacterFighter)
                return Required ? (int)((CharacterFighter)actor).Character.BreedId == Breed : (int)((CharacterFighter)actor).Character.BreedId != Breed;
            else
                return Required ? (int)BreedEnum.MONSTER == Breed : (int)BreedEnum.MONSTER != Breed;
        }
    }
}
