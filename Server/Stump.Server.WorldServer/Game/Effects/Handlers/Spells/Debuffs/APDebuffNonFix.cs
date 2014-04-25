using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_RemoveAP)]
    [EffectHandler(EffectsEnum.Effect_LosingAP)]
    public class APDebuffNonFix : SpellEffectHandler
    {
        public APDebuffNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var value = integerEffect.Value;

                // Effect_RemoveAP ignore resistance
                // if (Effect.EffectId != EffectsEnum.Effect_RemoveAP) // note : was i wrong ?
                {
                    value = 0;

                    for (int i = 0; i < integerEffect.Value && value < actor.AP; i++)
                    {
                        if (actor.RollAPLose(Caster))
                        {
                            value++;
                        }
                    }

                    var dodged = (short) (integerEffect.Value - value);

                    if (dodged > 0)
                    {
                        ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients, 
                            ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PA, Caster, actor, dodged);
                    }
                }

                if (value <= 0)
                    return false;

                if (Effect.Duration > 1)
                {
                    AddStatBuff(actor, (short)( -value ), PlayerFields.AP, true);
                }
                else
                {
                    actor.LostAP(value);
                }
            }

            return true;
        }
    }
}