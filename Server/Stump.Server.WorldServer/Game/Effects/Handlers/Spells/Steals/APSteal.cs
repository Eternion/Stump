using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealAP_440)]
    public class APSteal : SpellEffectHandler
    {
        public APSteal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                AddStatBuff(actor, (short)( -( integerEffect.Value ) ), PlayerFields.AP, true, (short)EffectsEnum.Effect_SubAP);
                actor.TriggerBuffs(BuffTriggerType.LOST_AP);

                if (Effect.Duration > 0)
                {
                    AddStatBuff(Caster, integerEffect.Value, PlayerFields.AP, true, (short)EffectsEnum.Effect_AddAP_111);
                }
                else
                {
                    Caster.RegainAP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}