using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_RemoveAP)]
    [EffectHandler(EffectsEnum.Effect_LosingAP)]
    [EffectHandler(EffectsEnum.Effect_SubAP_1079)]
    public class APDebuffNonFix : SpellEffectHandler
    {
        public APDebuffNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var target = actor;
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    target = Caster;

                    if (buff.Duration <= 0)
                        actor.RemoveAndDispellBuff(buff);
                }

                short value;

                // Effect_RemoveAP ignore resistance
                // if (Effect.EffectId != EffectsEnum.Effect_RemoveAP) // note : was i wrong ?
                {
                    value = 0;

                    for (var i = 0; i < integerEffect.Value && value < actor.AP; i++)
                    {
                        if (actor.RollAPLose(Caster, value))
                        {
                            value++;
                        }
                    }

                    var dodged = (short)(integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, target, dodged);
                    }
                }

                if (value <= 0)
                    return false;

                if (Effect.Duration > 1)
                {
                    AddStatBuff(target, (short)(-value), PlayerFields.AP, true);
                }
                else
                {
                    target.LostAP(value, Caster);
                }

                target.TriggerBuffs(BuffTriggerType.LOST_AP);
            }

            return true;
        }

        private void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }
    }
}