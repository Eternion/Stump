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
            foreach (var actor in GetAffectedActors())
            {
                var damages = new Fights.Damage(Dice);
                damages.MarkTrigger = MarkTrigger;
                damages.GenerateDamages();

                var damageRate = 0d;
                var life = (double)Caster.LifePoints / Caster.MaxLifePoints;

                if (life <= 0.5)
                    damageRate = 2 * life;
                else if (life > 0.5)
                    damageRate = 1 + (life - 0.5) * -2;

                damages.Amount = (int)(Caster.LifePoints * damageRate * damages.Amount / 100d);

               // spell reflected
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    damages.Source = actor;
                    damages.ReflectedDamages = true;
                    damages.IgnoreDamageBoost = true;
                    damages.IsCritical = Critical;
                    Caster.InflictDamage(damages);

                    actor.RemoveAndDispellBuff(buff);
                }
                else
                {
                    actor.InflictDamage(damages);
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