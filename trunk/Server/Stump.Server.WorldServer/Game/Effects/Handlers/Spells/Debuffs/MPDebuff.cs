using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubMP)]
    [EffectHandler(EffectsEnum.Effect_LosingMP)]
    [EffectHandler(EffectsEnum.Effect_LostMP)]
    public class MPDebuff : SpellEffectHandler
    {
        public MPDebuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var value = integerEffect.Value;

                // Effect_LosingMP ignore resistance
                if (Effect.EffectId != EffectsEnum.Effect_LosingMP)
                {
                    value = 0;

                    for (int i = 0; i < integerEffect.Value && value < actor.MP; i++)
                    {
                        if (actor.RollMPLose(Caster))
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
                    AddStatBuff(actor, (short)( -value ), PlayerFields.MP, true);
                }
                else
                {
                    actor.LostMP(value);
                }
            }

            return true;
        }
    }
}