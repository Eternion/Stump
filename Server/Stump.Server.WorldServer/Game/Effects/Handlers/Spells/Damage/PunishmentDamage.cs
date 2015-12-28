using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

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
            foreach (var actor in GetAffectedActors().ToArray())
            {
                var damages = new Fights.Damage(Dice) {MarkTrigger = MarkTrigger};
                damages.GenerateDamages();

                var damagesPercent = Critical ? 0.30d : 0.25d;
                damages.Amount = (int)((damagesPercent * Math.Pow(Math.Cos(2 * Math.PI * ((Caster.LifePoints / (double)Caster.MaxLifePoints) - 0.5)) + 1, 2)) / 4 * Caster.MaxLifePoints);

               // spell reflected
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    damages.Source = Caster;
                    damages.ReflectedDamages = true;
                    damages.IgnoreDamageBoost = true;
                    damages.IsCritical = Critical;
                    Caster.InflictDamage(damages);

                    if (buff.Duration <= 0)
                        actor.RemoveBuff(buff);
                }
                else
                {
                    damages.Source = Caster;
                    damages.IgnoreDamageBoost = true;
                    actor.InflictDamage(damages);
                }
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }
    }
}