using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealMP_77)]
    public class MPStealNonFix: SpellEffectHandler
    {
        public MPStealNonFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var value = 0;

                for (int i = 0; i < integerEffect.Value && value < actor.MP; i++)
                {
                    if (actor.RollMPLose(Caster))
                    {
                        value++;
                    }
                }

                var dodged = (short)(integerEffect.Value - value);

                if (dodged > 0)
                {
                    ActionsHandler.SendGameActionFightDodgePointLossMessage(Fight.Clients,
                        ActionsEnum.ACTION_FIGHT_SPELL_DODGED_PM, Caster, actor, dodged);
                }

                if (value <= 0)
                    return false;

                AddStatBuff(actor, (short)( -value ), PlayerFields.MP, true, (short)EffectsEnum.Effect_SubMP);
                if (Effect.Duration > 0)
                {
                    AddStatBuff(Caster, (short)(value), PlayerFields.MP, true, (short)EffectsEnum.Effect_AddMP_128);
                }
                else
                {
                    Caster.RegainMP((short)(value));
                }
            }

            return true;
        }
    }
}