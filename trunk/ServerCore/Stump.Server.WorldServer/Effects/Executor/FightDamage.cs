// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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
                                                FightGroupMember caster, CellData cell, bool critical)
        {
            if (!(generatedEffect is EffectInteger))
                return;

            var damage = unchecked((ushort) (generatedEffect as EffectInteger).Value);
            FightGroupMember target = fight.GetOneFighter(cell);

            if (target != null)
            {
                /* Recalculate damage */

                InflictDamage(fight, caster, target, damage);
            }
        }

        private static void InflictDamage(Fight fight, FightGroupMember caster, FightGroupMember target, ushort damage)
        {
            damage = target.ReceiveDamage(damage);

            var delta = (short) -damage;

            fight.CallOnAllCharacters(entry =>
                                      ActionsHandler.SendGameActionFightPointsVariationMessage(entry.Client, ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST,
                                                                                                 caster.Entity,
                                                                                                 target.Entity, delta));
        }
    }
}