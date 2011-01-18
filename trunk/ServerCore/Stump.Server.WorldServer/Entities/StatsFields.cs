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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core;
using Stump.BaseCore.Framework.Extensions;
using Stump.Server.WorldServer.Effects;

namespace Stump.Server.WorldServer.Entities
{
    public class StatsFields
    {
        private readonly List<Tuple<EffectBase, EffectContext>> m_appliedeffects =
            new List<Tuple<EffectBase, EffectContext>>();

        private readonly object m_sync = new object();

        #region Formules

        private static readonly Func<LivingEntity, int, int, int, int, int> FormuleInitiative =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus) =>
            {
                return owner.DamageTaken <= 0
                           ? 0
                           : (valueBase + valueEquiped + valueBonus +
                              owner.Stats["Chance"] +
                              owner.Stats["Intelligence"] +
                              owner.Stats["Agility"] +
                              owner.Stats["Strengh"])*
                             (owner.Stats["Health"].Total/
                              ((StatsHealth) owner.Stats["Health"]).
                                  TotalMax);
            };

        private static readonly Func<LivingEntity, int, int, int, int, int> FormuleProspecting =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus) =>
            valueBase + valueEquiped + valueBonus + (int) (owner.Stats["Chance"]/10d);

        #endregion

        public StatsFields(LivingEntity owner)
        {
            Owner = owner;

            if (Owner is Character)
                InitializeStatsAsCharacter();
            else if (Owner is Monster)
            {
                /*...*/
            }
            else
                throw new Exception("Cannot use StatsFields with other Entity than Character or Monster");
        }

        public Dictionary<string, StatsData> Fields
        {
            get;
            private set;
        }

        public LivingEntity Owner
        {
            get;
            private set;
        }

        public StatsHealth Health
        {
            get { return this["Health"] as StatsHealth; }
        }

        public StatsData this[string name]
        {
            get
            {
                StatsData value;
                return Fields.TryGetValue(name, out value) ? value : null;
            }
        }

        public void ClearAllEffect()
        {
            ClearAllEquipedEffect();
            ClearAllFightEffect();
        }

        public void ClearAllEquipedEffect()
        {
            lock (m_sync)
            {
                Fields.ForEach(entry => entry.Value.Equiped = 0);
                m_appliedeffects.RemoveAll(entry => entry.Item2 == EffectContext.Inventory);
            }
        }

        public void ClearAllFightEffect()
        {
            lock (m_sync)
            {
                Fields.ForEach(entry => entry.Value.Bonus = 0);
                m_appliedeffects.RemoveAll(entry => entry.Item2 == EffectContext.Fight);
            }
        }

        public bool TryAddEffect(EffectBase effect, EffectContext context)
        {
            string stat;
            Action action;
            EffectBase generatedeffect = effect.GenerateEffect(EffectGenerationContext.Item);
            if (generatedeffect is EffectInteger &&
                GetEffectCharacteristicBonus(generatedeffect, out stat))
            {
                int value = ( generatedeffect as EffectInteger ).Value;

                if (context == EffectContext.Fight)
                    this[stat].Bonus += value;
                else if (context == EffectContext.Inventory)
                    this[stat].Equiped += value;

                m_appliedeffects.Add(new Tuple<EffectBase, EffectContext>(effect, context));

                return true;
            }
            else if (GetEffectBindedAction(effect, out action))
            {
                action.DynamicInvoke(null); // TODO : args = ?

                return true;
            }

            return false;
        }

        public bool TryDeleteEffect(EffectBase effect, EffectContext context)
        {
            IEnumerable<Tuple<EffectBase, EffectContext>> effectstodelete =
                m_appliedeffects.Where(entry => entry.Item1 == effect && entry.Item2 == context);

            if (effectstodelete.Count() > 0)
            {
                m_appliedeffects.Remove(effectstodelete.First());

                return true;
            }

            return false;
        }

        private static bool GetEffectCharacteristicBonus(EffectBase effect, out string stat)
        {
            try
            {
                string name = effect.EffectId.ToString().Split('_').GetValue(1).ToString();

                if (name.StartsWith("Add") || name.StartsWith("Sub"))
                {
                    stat = name.Substring(3);
                    return true;
                }
            }
            catch
            {
                stat = null;
                return false;
            }

            stat = null;
            return false;
        }

        private static bool GetEffectBindedAction(EffectBase effect, out Action action)
        {
            try
            {
                string name = effect.EffectId.ToString().Split('_').GetValue(1).ToString();

                foreach (MethodInfo method in typeof (StatsFields).GetMethods())
                {
                    IEnumerable<object> attributes =
                        method.GetCustomAttributes(true).Where(
                            entry =>
                            entry is EffectHandleAttribute &&
                            (entry as EffectHandleAttribute).Effects.Contains(effect.EffectId));

                    if (attributes.Count() > 0)
                    {
                        action = (Action) method.ToDelegate(null);
                        return true;
                    }
                }
            }
            catch
            {
                action = null;
                return false;
            }

            action = null;
            return false;
        }

        public void InitializeStatsAsCharacter()
        {
            Fields = new Dictionary<string, StatsData>
                {
                    {"Health", new StatsHealth(Owner, ((Character) Owner).Record.BaseHealth, ((Character) Owner).Record.DamageTaken)},
                    {"Initiative", new StatsData(Owner, "Initiative", 0, FormuleInitiative)},
                    {"Prospecting", new StatsData(Owner, "Prospecting", 100, FormuleProspecting)},
                    {"AP", new StatsData(Owner, "AP", ((Character) Owner).Record.Level >= 100 ? 7 : 6)},
                    {"MP", new StatsData(Owner, "MP", 3)},
                    {"Strength", new StatsData(Owner, "Strength", 0)},
                    {"Vitality", new StatsData(Owner, "Vitality", 0)},
                    {"Wisdom", new StatsData(Owner, "Wisdom", 0)},
                    {"Chance", new StatsData(Owner, "Chance", 0)},
                    {"Agility", new StatsData(Owner, "Agility", 0)},
                    {"Intelligence", new StatsData(Owner, "Intelligence", 0)},
                    {"Range", new StatsData(Owner, "Range", 0)},
                    {"SummonLimit", new StatsData(Owner, "SummonLimit", 1)},
                    {"DamageReflection", new StatsData(Owner, "DamageReflection", 0)},
                    {"CriticalHit", new StatsData(Owner, "CriticalHit", 0)},
                    {"CriticalMiss", new StatsData(Owner, "CriticalMiss", 0)},
                    {"HealBonus", new StatsData(Owner, "HealBonus", 0)},
                    {"DamageBonus", new StatsData(Owner, "DamageBonus", 0)},
                    {"WeaponDamageBonus", new StatsData(Owner, "WeaponDamageBonus", 0)},
                    {"DamageBonusPercent", new StatsData(Owner, "DamageBonusPercent", 0)},
                    {"TrapBonus", new StatsData(Owner, "TrapBonus", 0)},
                    {"TrapBonusPercent", new StatsData(Owner, "TrapBonusPercent", 0)},
                    {"PermanentDamagePercent", new StatsData(Owner, "PermanentDamagePercent", 0)},
                    {"TackleBlock", new StatsData(Owner, "TackleBlock", 0)},
                    {"TackleEvade", new StatsData(Owner, "TackleEvade", 0)},
                    {"APAttack", new StatsData(Owner, "APAttack", 0)},
                    {"MPAttack", new StatsData(Owner, "MPAttack", 0)},
                    {"PushDamageBonus", new StatsData(Owner, "PushDamageBonus", 0)},
                    {"CriticalDamageBonus", new StatsData(Owner, "CriticalDamageBonus", 0)},
                    {"NeutralDamageBonus", new StatsData(Owner, "NeutralDamageBonus", 0)},
                    {"EarthDamageBonus", new StatsData(Owner, "EarthDamageBonus", 0)},
                    {"WaterDamageBonus", new StatsData(Owner, "WaterDamageBonus", 0)},
                    {"AirDamageBonus", new StatsData(Owner, "AirDamageBonus", 0)},
                    {"FireDamageBonus", new StatsData(Owner, "FireDamageBonus", 0)},
                    {"DodgeAPProbability", new StatsData(Owner, "DodgeAPProbability", 0)},
                    {"DodgeMPProbability", new StatsData(Owner, "DodgeMPProbability", 0)},
                    {"NeutralResistPercent", new StatsData(Owner, "NeutralResistPercent", 0)},
                    {"EarthResistPercent", new StatsData(Owner, "EarthResistPercent", 0)},
                    {"WaterResistPercent", new StatsData(Owner, "WaterResistPercent", 0)},
                    {"AirResistPercent", new StatsData(Owner, "AirResistPercent", 0)},
                    {"FireResistPercent", new StatsData(Owner, "FireResistPercent", 0)},
                    {"NeutralElementReduction", new StatsData(Owner, "NeutralElementReduction", 0)},
                    {"EarthElementReduction", new StatsData(Owner, "EarthElementReduction", 0)},
                    {"WaterElementReduction", new StatsData(Owner, "WaterElementReduction", 0)},
                    {"AirElementReduction", new StatsData(Owner, "AirElementReduction", 0)},
                    {"FireElementReduction", new StatsData(Owner, "FireElementReduction", 0)},
                    {"PushDamageReduction", new StatsData(Owner, "PushDamageReduction", 0)},
                    {"CriticalDamageReduction", new StatsData(Owner, "CriticalDamageReduction", 0)},
                    {"PvpNeutralResistPercent", new StatsData(Owner, "PvpNeutralResistPercent", 0)},
                    {"PvpEarthResistPercent", new StatsData(Owner, "PvpEarthResistPercent", 0)},
                    {"PvpWaterResistPercent", new StatsData(Owner, "PvpWaterResistPercent", 0)},
                    {"PvpAirResistPercent", new StatsData(Owner, "PvpAirResistPercent", 0)},
                    {"PvpFireResistPercent", new StatsData(Owner, "PvpFireResistPercent", 0)},
                    {"PvpNeutralElementReduction", new StatsData(Owner, "PvpNeutralElementReduction", 0)},
                    {"PvpEarthElementReduction", new StatsData(Owner, "PvpEarthElementReduction", 0)},
                    {"PvpWaterElementReduction", new StatsData(Owner, "PvpWaterElementReduction", 0)},
                    {"PvpAirElementReduction", new StatsData(Owner, "PvpAirElementReduction", 0)},
                    {"PvpFireElementReduction", new StatsData(Owner, "PvpFireElementReduction", 0)},
                    {"GlobalDamageReduction", new StatsData(Owner, "GlobalDamageReduction", 0)},
                    {"DamageMultiplicator", new StatsData(Owner, "DamageMultiplicator", 0)},
                    {"PhysicalDamage", new StatsData(Owner, "PhysicalDamage", 0)},
                    {"MagicDamage", new StatsData(Owner, "MagicDamage", 0)},
                    {"PhysicalDamageReduction", new StatsData(Owner, "PhysicalDamageReduction", 0)},
                    {"MagicDamageReduction", new StatsData(Owner, "MagicDamageReduction", 0)}
                };

            // custom fields
        }
    }
}