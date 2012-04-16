using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubResistances)]
    public class Resistances : SpellEffectHandler
    {
        public Resistances(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return false;

            foreach (FightActor actor in GetAffectedActors())
            {
                if (actor == Caster) // just an ugly hack
                    continue;

                var buff = new ResistancesDebuff(actor.PopNextBuffId(), actor, Caster, integerEffect, Spell, integerEffect.Value, false, true);
                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}