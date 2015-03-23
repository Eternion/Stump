using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_LosingMP)]
    [EffectHandler(EffectsEnum.Effect_LostMP)]
    [EffectHandler(EffectsEnum.Effect_SubMP_1080)]
    public class MPDebuffNonFix : SpellEffectHandler
    {
        public MPDebuffNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                short value;

                // Effect_LosingMP ignore resistance
                //if (Effect.EffectId != EffectsEnum.Effect_LosingMP)
                {
                    value = 0;

                    for (var i = 0; i < integerEffect.Value && value < actor.MP; i++)
                    {
                        if (actor.RollMPLose(Caster, value))
                        {
                            value++;
                        }
                    }

                    var dodged = (short)( integerEffect.Value - value );

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, Caster, actor, dodged);
                    }
                }

                if (value <= 0)
                    return false;

                if (Effect.Duration > 1)
                {
                    AddStatBuff(actor, (short)(-value), PlayerFields.MP, true);
                    ActionsHandler.SendGameActionFightPointsVariationMessage(Fight.Clients, ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE, Caster, actor, (short)(-value));
                }
                else
                {
                    actor.LostMP(value);
                }

                actor.TriggerBuffs(BuffTriggerType.LOST_MP);
            }

            return true;
        }
    }
}