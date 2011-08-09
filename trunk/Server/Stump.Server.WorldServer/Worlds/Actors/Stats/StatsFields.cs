using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Breeds;

namespace Stump.Server.WorldServer.Worlds.Actors.Stats
{
    public class StatsFields
    {
        #region Formulas

        private static readonly Func<IStatsOwner, int, int, int, int, int> FormuleInitiative =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus) =>
            {
                return owner.Stats["Health"].Total <= 0
                           ? 0
                           : ( valueBase + valueEquiped + valueBonus +
                              owner.Stats["Chance"] +
                              owner.Stats["Intelligence"] +
                              owner.Stats["Agility"] +
                              owner.Stats["Strengh"] ) *
                             ( owner.Stats["Health"].Total /
                             ( (StatsHealth)owner.Stats["Health"] ).TotalMax );
            };

        private static readonly Func<IStatsOwner, int, int, int, int, int> FormuleProspecting =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus) =>
            valueBase + valueEquiped + valueBonus + (int)( owner.Stats["Chance"] / 10d );

        #endregion

        public StatsFields(IStatsOwner owner, CharacterRecord record)
        {
            Owner = owner;

            Initialize(record);
        }

        public Dictionary<string, StatsData> Fields
        {
            get;
            private set;
        }

        public IStatsOwner Owner
        {
            get;
            private set;
        }

        public StatsHealth Health
        {
            get
            {
                return this["Health"] as StatsHealth;
            }
        }

        public StatsData this[string name]
        {
            get
            {
                StatsData value;
                return Fields.TryGetValue(name, out value) ? value : null;
            }
        }

        public void Initialize(CharacterRecord record)
        {
            Fields = new Dictionary<string, StatsData>
                {
                    {"Health", new StatsHealth(Owner, record.BaseHealth, record.DamageTaken)},
                    {"Initiative", new StatsData(Owner, "Initiative", 0, FormuleInitiative)},
                    {"Prospecting", new StatsData(Owner, "Prospecting", (short) record.Prospection, FormuleProspecting)},
                    {"AP", new StatsData(Owner, "AP", (short) record.AP)},
                    {"MP", new StatsData(Owner, "MP", (short) record.MP)},
                    {"Strength", new StatsData(Owner, "Strength", record.Strength)},
                    {"Vitality", new StatsData(Owner, "Vitality", record.Vitality)},
                    {"Wisdom", new StatsData(Owner, "Wisdom", record.Wisdom)},
                    {"Chance", new StatsData(Owner, "Chance", record.Chance)},
                    {"Agility", new StatsData(Owner, "Agility", record.Agility)},
                    {"Intelligence", new StatsData(Owner, "Intelligence", record.Intelligence)},
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