using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
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
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var value = 0;

                for (var i = 0; i < integerEffect.Value && value < actor.MP; i++)
                {
                    if (actor.RollMPLose(Caster, value))
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

                //AddStatBuff(actor, (short)( -value ), PlayerFields.MP, true, (short)EffectsEnum.Effect_SubMP);
                actor.LostMP((short)value);
                actor.TriggerBuffs(BuffTriggerType.LOST_MP);

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