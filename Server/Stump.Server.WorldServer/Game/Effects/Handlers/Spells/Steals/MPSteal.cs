using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealMP_441)]
    public class MPSteal : SpellEffectHandler
    {
        public MPSteal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                AddStatBuff(actor, (short)( -( integerEffect.Value ) ), PlayerFields.MP, true, (short)EffectsEnum.Effect_SubMP);
                actor.TriggerBuffs(BuffTriggerType.LOST_MP);

                if (Effect.Duration > 0)
                {
                    AddStatBuff(Caster, integerEffect.Value, PlayerFields.MP, true, (short)EffectsEnum.Effect_AddMP_128);
                }
                else
                {
                    Caster.RegainMP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}