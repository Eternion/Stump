using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_SubResistances)]
    [EffectHandler(EffectsEnum.Effect_AddResistances)]
    public class Resistances : SpellEffectHandler
    {
        public Resistances(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (FightActor actor in GetAffectedActors())
            {
                var buff = new ResistancesBuff(actor.PopNextBuffId(), actor, Caster, integerEffect, Spell, 
                    (short) ((Effect.EffectId == EffectsEnum.Effect_SubResistances) ? -integerEffect.Value : integerEffect.Value),
                    false, true);
                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}