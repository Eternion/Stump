using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_Punishment_Damage)]
    public class PunishmentDamage : SpellEffectHandler
    {
        public PunishmentDamage(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                var damages = (short) (( -Math.Abs((double)actor.LifePoints / actor.MaxLifePoints - 0.5) + 1 ) * ( integerEffect.Value / 100d * actor.LifePoints ));

               // spell reflected
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel)
                {
                    NotifySpellReflected(actor);
                    Caster.InflictDamage(damages, EffectSchoolEnum.Neutral, Caster, Caster is CharacterFighter, Spell);

                    actor.RemoveAndDispellBuff(buff);
                }
                else
                {
                    short inflictedDamage = actor.InflictDamage(damages, EffectSchoolEnum.Neutral, Caster, actor is CharacterFighter, Spell);
                }
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, source, Caster);
        }
    }
}