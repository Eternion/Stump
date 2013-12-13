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
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var damageRate = 0d;
                var life = (double)Caster.LifePoints / Caster.MaxLifePoints;

                if (life <= 0.5)
                    damageRate = 2 * life;
                else if (life > 0.5)
                    damageRate = 1 + (life - 0.5) * -2;

                var damages = (short)(Caster.LifePoints * damageRate * integerEffect.Value / 100d);

               // spell reflected
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    Caster.InflictDamage(damages, EffectSchoolEnum.Neutral, Caster, actor is CharacterFighter, Spell, false);

                    actor.RemoveAndDispellBuff(buff);
                }
                else
                {
                    actor.InflictDirectDamage(damages, Caster);
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