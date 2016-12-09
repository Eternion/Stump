using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_SubResistances)]
    [EffectHandler(EffectsEnum.Effect_AddResistances)]
    public class Resistances : SpellEffectHandler
    {
        public Resistances(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                var buff = new ResistancesBuff(actor.PopNextBuffId(), actor, Caster, this, Spell, 
                    (short) ((Effect.EffectId == EffectsEnum.Effect_SubResistances) ? -integerEffect.Value : integerEffect.Value),
                    false, FightDispellableEnum.DISPELLABLE);

                actor.AddBuff(buff);
            }

            return true;
        }
    }
}