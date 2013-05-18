#region License GNU GPL
// RestoreHpPercent.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_RestoreHPPercent)]
    public class RestoreHpPercent : SpellEffectHandler
    {
        public RestoreHpPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, OnBuffTriggered);
                }
                else
                    HealHpPercent(actor, integerEffect.Value);
            }

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {                
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            HealHpPercent(buff.Target, integerEffect.Value);
        }

        private void HealHpPercent(FightActor actor, int percent)
        {
            int healAmount = (int) (actor.MaxLifePoints*(percent/100d));

            actor.Heal(healAmount, Caster, false);
        }
    }
}