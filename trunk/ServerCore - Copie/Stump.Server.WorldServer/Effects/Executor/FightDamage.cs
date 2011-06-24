
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Effects.Executor
{
    public partial class FightEffectExecutor
    {
        [FightEffectAttribute(EffectsEnum.Effect_DamageNeutral)]
        [FightEffectAttribute(EffectsEnum.Effect_DamageEarth)]
        [FightEffectAttribute(EffectsEnum.Effect_DamageFire)]
        [FightEffectAttribute(EffectsEnum.Effect_DamageWater)]
        [FightEffectAttribute(EffectsEnum.Effect_DamageAir)]
        [FightEffectAttribute(EffectsEnum.Effect_DamageFix)]
        public static void ExecuteEffect_Damage(SpellLevel spellLevel, EffectBase generatedEffect, Fight fight,
                                                FightGroupMember caster, CellLinked cell, bool critical)
        {
            if (!(generatedEffect is EffectInteger))
                return;

            var damage = unchecked((ushort) (generatedEffect as EffectInteger).Value);
            FightGroupMember target = fight.GetOneFighter(cell);

            if (target != null)
            {
                /* Recalculate damage */

                target.ReceiveDamage(damage, caster);
            }
        }
    }
}