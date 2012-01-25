using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public delegate int StatsFormulasHandler(IStatsOwner target, int @base, int equiped, int given, int context);

    public class StatsFields
    {
        #region Formulas

        private static readonly StatsFormulasHandler FormulasInitiative =
            (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
                {
                    return owner.Stats.Health.Total <= 0
                               ? 0
                               : (valueBase + valueEquiped + valueContext +
                                  owner.Stats[PlayerFields.Chance] +
                                  owner.Stats[PlayerFields.Intelligence] +
                                  owner.Stats[PlayerFields.Agility] +
                                  owner.Stats[PlayerFields.Strength])*
                                 (owner.Stats.Health.Total/
                                  owner.Stats.Health.TotalMax);
                };

        private static readonly StatsFormulasHandler FormulasChanceDependant =
            (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
            valueBase + valueEquiped + valueContext + (int) (owner.Stats[PlayerFields.Chance]/10d);

        private static readonly StatsFormulasHandler FormulasWisdomDependant =
             (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
                 valueBase + valueEquiped + valueContext + (int)( owner.Stats[PlayerFields.Wisdom] / 10d );


        private static readonly StatsFormulasHandler FormulasAgilityDependant =
             (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
                 valueBase + valueEquiped + valueContext + (int)( owner.Stats[PlayerFields.Agility] / 10d );

        #endregion

        public StatsFields(IStatsOwner owner, CharacterRecord record)
        {
            Owner = owner;

            Initialize(record);
        }

        public StatsFields(IStatsOwner owner, MonsterGrade record)
        {
            Owner = owner;

            Initialize(record);
        }

        public Dictionary<PlayerFields, StatsData> Fields
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
            get { return this[PlayerFields.Health] as StatsHealth; }
        }

        public StatsAP AP
        {
            get { return this[PlayerFields.AP] as StatsAP; }
        }

        public StatsMP MP
        {
            get { return this[PlayerFields.MP] as StatsMP; }
        }

        public StatsData Strength
        {
            get { return this[PlayerFields.Strength]; }
        }

        public StatsData Wisdom
        {
            get { return this[PlayerFields.Wisdom]; }
        }

        public StatsData Chance
        {
            get { return this[PlayerFields.Chance]; }
        }

        public StatsData Agility
        {
            get { return this[PlayerFields.Agility]; }
        }

        public StatsData Intelligence
        {
            get { return this[PlayerFields.Intelligence]; }
        }

        public StatsData this[PlayerFields name]
        {
            get
            {
                StatsData value;
                return Fields.TryGetValue(name, out value) ? value : null;
            }
        }

        public void Initialize(CharacterRecord record)
        {
            Fields = new Dictionary<PlayerFields, StatsData>
                         {
                             {PlayerFields.Health, new StatsHealth(Owner, (short) record.BaseHealth, (short) record.DamageTaken)},
                             {PlayerFields.Initiative, new StatsData(Owner, PlayerFields.Initiative, 0, FormulasInitiative)},
                             {PlayerFields.Prospecting, new StatsData(Owner, PlayerFields.Prospecting, (short) record.Prospection, FormulasChanceDependant)},
                             {PlayerFields.AP, new StatsAP(Owner, (short) record.AP)},
                             {PlayerFields.MP, new StatsMP(Owner, (short) record.MP)},
                             {PlayerFields.Strength, new StatsData(Owner, PlayerFields.Strength, record.Strength)},
                             {PlayerFields.Vitality, new StatsData(Owner, PlayerFields.Vitality, record.Vitality)},
                             {PlayerFields.Wisdom, new StatsData(Owner, PlayerFields.Wisdom, record.Wisdom)},
                             {PlayerFields.Chance, new StatsData(Owner, PlayerFields.Chance, record.Chance)},
                             {PlayerFields.Agility, new StatsData(Owner, PlayerFields.Agility, record.Agility)},
                             {PlayerFields.Intelligence, new StatsData(Owner, PlayerFields.Intelligence, record.Intelligence)},
                             {PlayerFields.Range, new StatsData(Owner, PlayerFields.Range, 0)},
                             {PlayerFields.SummonLimit, new StatsData(Owner, PlayerFields.SummonLimit, 1)},
                             {PlayerFields.DamageReflection, new StatsData(Owner, PlayerFields.DamageReflection, 0)},
                             {PlayerFields.CriticalHit, new StatsData(Owner, PlayerFields.CriticalHit, 0)},
                             {PlayerFields.CriticalMiss, new StatsData(Owner, PlayerFields.CriticalMiss, 0)},
                             {PlayerFields.HealBonus, new StatsData(Owner, PlayerFields.HealBonus, 0)},
                             {PlayerFields.DamageBonus, new StatsData(Owner, PlayerFields.DamageBonus, 0)},
                             {PlayerFields.WeaponDamageBonus, new StatsData(Owner, PlayerFields.WeaponDamageBonus, 0)},
                             {PlayerFields.DamageBonusPercent, new StatsData(Owner, PlayerFields.DamageBonusPercent, 0)},
                             {PlayerFields.TrapBonus, new StatsData(Owner, PlayerFields.TrapBonus, 0)},
                             {PlayerFields.TrapBonusPercent, new StatsData(Owner, PlayerFields.TrapBonusPercent, 0)},
                             {PlayerFields.PermanentDamagePercent, new StatsData(Owner, PlayerFields.PermanentDamagePercent, 0)},
                             {PlayerFields.TackleBlock, new StatsData(Owner, PlayerFields.TackleBlock, 0, FormulasAgilityDependant)},
                             {PlayerFields.TackleEvade, new StatsData(Owner, PlayerFields.TackleEvade, 0, FormulasAgilityDependant)},
                             {PlayerFields.APAttack, new StatsData(Owner, PlayerFields.APAttack, 0, FormulasWisdomDependant)},
                             {PlayerFields.MPAttack, new StatsData(Owner, PlayerFields.MPAttack, 0, FormulasWisdomDependant)},
                             {PlayerFields.PushDamageBonus, new StatsData(Owner, PlayerFields.PushDamageBonus, 0)},
                             {PlayerFields.CriticalDamageBonus, new StatsData(Owner, PlayerFields.CriticalDamageBonus, 0)},
                             {PlayerFields.NeutralDamageBonus, new StatsData(Owner, PlayerFields.NeutralDamageBonus, 0)},
                             {PlayerFields.EarthDamageBonus, new StatsData(Owner, PlayerFields.EarthDamageBonus, 0)},
                             {PlayerFields.WaterDamageBonus, new StatsData(Owner, PlayerFields.WaterDamageBonus, 0)},
                             {PlayerFields.AirDamageBonus, new StatsData(Owner, PlayerFields.AirDamageBonus, 0)},
                             {PlayerFields.FireDamageBonus, new StatsData(Owner, PlayerFields.FireDamageBonus, 0)},
                             {PlayerFields.DodgeAPProbability, new StatsData(Owner, PlayerFields.DodgeAPProbability, 0, FormulasWisdomDependant)},
                             {PlayerFields.DodgeMPProbability, new StatsData(Owner, PlayerFields.DodgeMPProbability, 0, FormulasWisdomDependant)},
                             {PlayerFields.NeutralResistPercent, new StatsData(Owner, PlayerFields.NeutralResistPercent, 0)},
                             {PlayerFields.EarthResistPercent, new StatsData(Owner, PlayerFields.EarthResistPercent, 0)},
                             {PlayerFields.WaterResistPercent, new StatsData(Owner, PlayerFields.WaterResistPercent, 0)},
                             {PlayerFields.AirResistPercent, new StatsData(Owner, PlayerFields.AirResistPercent, 0)},
                             {PlayerFields.FireResistPercent, new StatsData(Owner, PlayerFields.FireResistPercent, 0)},
                             {PlayerFields.NeutralElementReduction, new StatsData(Owner, PlayerFields.NeutralElementReduction, 0)},
                             {PlayerFields.EarthElementReduction, new StatsData(Owner, PlayerFields.EarthElementReduction, 0)},
                             {PlayerFields.WaterElementReduction, new StatsData(Owner, PlayerFields.WaterElementReduction, 0)},
                             {PlayerFields.AirElementReduction, new StatsData(Owner, PlayerFields.AirElementReduction, 0)},
                             {PlayerFields.FireElementReduction, new StatsData(Owner, PlayerFields.FireElementReduction, 0)},
                             {PlayerFields.PushDamageReduction, new StatsData(Owner, PlayerFields.PushDamageReduction, 0)},
                             {PlayerFields.CriticalDamageReduction, new StatsData(Owner, PlayerFields.CriticalDamageReduction, 0)},
                             {PlayerFields.PvpNeutralResistPercent, new StatsData(Owner, PlayerFields.PvpNeutralResistPercent, 0)},
                             {PlayerFields.PvpEarthResistPercent, new StatsData(Owner, PlayerFields.PvpEarthResistPercent, 0)},
                             {PlayerFields.PvpWaterResistPercent, new StatsData(Owner, PlayerFields.PvpWaterResistPercent, 0)},
                             {PlayerFields.PvpAirResistPercent, new StatsData(Owner, PlayerFields.PvpAirResistPercent, 0)},
                             {PlayerFields.PvpFireResistPercent, new StatsData(Owner, PlayerFields.PvpFireResistPercent, 0)},
                             {PlayerFields.PvpNeutralElementReduction, new StatsData(Owner, PlayerFields.PvpNeutralElementReduction, 0)},
                             {PlayerFields.PvpEarthElementReduction, new StatsData(Owner, PlayerFields.PvpEarthElementReduction, 0)},
                             {PlayerFields.PvpWaterElementReduction, new StatsData(Owner, PlayerFields.PvpWaterElementReduction, 0)},
                             {PlayerFields.PvpAirElementReduction, new StatsData(Owner, PlayerFields.PvpAirElementReduction, 0)},
                             {PlayerFields.PvpFireElementReduction, new StatsData(Owner, PlayerFields.PvpFireElementReduction, 0)},
                             {PlayerFields.GlobalDamageReduction, new StatsData(Owner, PlayerFields.GlobalDamageReduction, 0)},
                             {PlayerFields.DamageMultiplicator, new StatsData(Owner, PlayerFields.DamageMultiplicator, 0)},
                             {PlayerFields.PhysicalDamage, new StatsData(Owner, PlayerFields.PhysicalDamage, 0)},
                             {PlayerFields.MagicDamage, new StatsData(Owner, PlayerFields.MagicDamage, 0)},
                             {PlayerFields.PhysicalDamageReduction, new StatsData(Owner, PlayerFields.PhysicalDamageReduction, 0)},
                             {PlayerFields.MagicDamageReduction, new StatsData(Owner, PlayerFields.MagicDamageReduction, 0)},
                             // custom fields

                             {PlayerFields.WaterDamageArmor, new StatsData(Owner, PlayerFields.WaterDamageArmor, 0)},
                             {PlayerFields.EarthDamageArmor, new StatsData(Owner, PlayerFields.EarthDamageArmor, 0)},
                             {PlayerFields.NeutralDamageArmor, new StatsData(Owner, PlayerFields.NeutralDamageArmor, 0)},
                             {PlayerFields.AirDamageArmor, new StatsData(Owner, PlayerFields.AirDamageArmor, 0)},
                             {PlayerFields.FireDamageArmor, new StatsData(Owner, PlayerFields.FireDamageArmor, 0)},
                         };
        }

        public void Initialize(MonsterGrade record)
        {
            Fields = new Dictionary<PlayerFields, StatsData>
                         {
                             {PlayerFields.Health, new StatsHealth(Owner, (short) record.LifePoints, 0)},
                             {PlayerFields.Initiative, new StatsData(Owner, PlayerFields.Initiative, 0, FormulasInitiative)},
                             {PlayerFields.Prospecting, new StatsData(Owner, PlayerFields.Prospecting, 100, FormulasChanceDependant)},
                             {PlayerFields.AP, new StatsAP(Owner, (short) record.ActionPoints)},
                             {PlayerFields.MP, new StatsMP(Owner, (short) record.MovementPoints)},
                             {PlayerFields.Strength, new StatsData(Owner, PlayerFields.Strength, record.Strength)},
                             {PlayerFields.Vitality, new StatsData(Owner, PlayerFields.Vitality, record.Vitality)},
                             {PlayerFields.Wisdom, new StatsData(Owner, PlayerFields.Wisdom, record.Wisdom)},
                             {PlayerFields.Chance, new StatsData(Owner, PlayerFields.Chance, record.Chance)},
                             {PlayerFields.Agility, new StatsData(Owner, PlayerFields.Agility, record.Agility)},
                             {PlayerFields.Intelligence, new StatsData(Owner, PlayerFields.Intelligence, record.Intelligence)},
                             {PlayerFields.Range, new StatsData(Owner, PlayerFields.Range, 0)},
                             {PlayerFields.SummonLimit, new StatsData(Owner, PlayerFields.SummonLimit, 1)},
                             {PlayerFields.DamageReflection, new StatsData(Owner, PlayerFields.DamageReflection, 0)},
                             {PlayerFields.CriticalHit, new StatsData(Owner, PlayerFields.CriticalHit, 0)},
                             {PlayerFields.CriticalMiss, new StatsData(Owner, PlayerFields.CriticalMiss, 0)},
                             {PlayerFields.HealBonus, new StatsData(Owner, PlayerFields.HealBonus, 0)},
                             {PlayerFields.DamageBonus, new StatsData(Owner, PlayerFields.DamageBonus, 0)},
                             {PlayerFields.WeaponDamageBonus, new StatsData(Owner, PlayerFields.WeaponDamageBonus, 0)},
                             {PlayerFields.DamageBonusPercent, new StatsData(Owner, PlayerFields.DamageBonusPercent, 0)},
                             {PlayerFields.TrapBonus, new StatsData(Owner, PlayerFields.TrapBonus, 0)},
                             {PlayerFields.TrapBonusPercent, new StatsData(Owner, PlayerFields.TrapBonusPercent, 0)},
                             {PlayerFields.PermanentDamagePercent, new StatsData(Owner, PlayerFields.PermanentDamagePercent, 0)},
                             {PlayerFields.TackleBlock, new StatsData(Owner, PlayerFields.TackleBlock, record.TackleBlock, FormulasAgilityDependant)},
                             {PlayerFields.TackleEvade, new StatsData(Owner, PlayerFields.TackleEvade, record.TackleEvade, FormulasAgilityDependant)},
                             {PlayerFields.APAttack, new StatsData(Owner, PlayerFields.APAttack, 0)},
                             {PlayerFields.MPAttack, new StatsData(Owner, PlayerFields.MPAttack, 0)},
                             {PlayerFields.PushDamageBonus, new StatsData(Owner, PlayerFields.PushDamageBonus, 0)},
                             {PlayerFields.CriticalDamageBonus, new StatsData(Owner, PlayerFields.CriticalDamageBonus, 0)},
                             {PlayerFields.NeutralDamageBonus, new StatsData(Owner, PlayerFields.NeutralDamageBonus, 0)},
                             {PlayerFields.EarthDamageBonus, new StatsData(Owner, PlayerFields.EarthDamageBonus, 0)},
                             {PlayerFields.WaterDamageBonus, new StatsData(Owner, PlayerFields.WaterDamageBonus, 0)},
                             {PlayerFields.AirDamageBonus, new StatsData(Owner, PlayerFields.AirDamageBonus, 0)},
                             {PlayerFields.FireDamageBonus, new StatsData(Owner, PlayerFields.FireDamageBonus, 0)},
                             {PlayerFields.DodgeAPProbability, new StatsData(Owner, PlayerFields.DodgeAPProbability, (short) record.PaDodge, FormulasWisdomDependant)},
                             {PlayerFields.DodgeMPProbability, new StatsData(Owner, PlayerFields.DodgeMPProbability, (short) record.PmDodge, FormulasWisdomDependant)},
                             {PlayerFields.NeutralResistPercent, new StatsData(Owner, PlayerFields.NeutralResistPercent, (short) record.NeutralResistance)},
                             {PlayerFields.EarthResistPercent, new StatsData(Owner, PlayerFields.EarthResistPercent, (short) record.EarthResistance)},
                             {PlayerFields.WaterResistPercent, new StatsData(Owner, PlayerFields.WaterResistPercent, (short) record.WaterResistance)},
                             {PlayerFields.AirResistPercent, new StatsData(Owner, PlayerFields.AirResistPercent, (short) record.AirResistance)},
                             {PlayerFields.FireResistPercent, new StatsData(Owner, PlayerFields.FireResistPercent, (short) record.FireResistance)},
                             {PlayerFields.NeutralElementReduction, new StatsData(Owner, PlayerFields.NeutralElementReduction, 0)},
                             {PlayerFields.EarthElementReduction, new StatsData(Owner, PlayerFields.EarthElementReduction, 0)},
                             {PlayerFields.WaterElementReduction, new StatsData(Owner, PlayerFields.WaterElementReduction, 0)},
                             {PlayerFields.AirElementReduction, new StatsData(Owner, PlayerFields.AirElementReduction, 0)},
                             {PlayerFields.FireElementReduction, new StatsData(Owner, PlayerFields.FireElementReduction, 0)},
                             {PlayerFields.PushDamageReduction, new StatsData(Owner, PlayerFields.PushDamageReduction, 0)},
                             {PlayerFields.CriticalDamageReduction, new StatsData(Owner, PlayerFields.CriticalDamageReduction, 0)},
                             {PlayerFields.PvpNeutralResistPercent, new StatsData(Owner, PlayerFields.PvpNeutralResistPercent, 0)},
                             {PlayerFields.PvpEarthResistPercent, new StatsData(Owner, PlayerFields.PvpEarthResistPercent, 0)},
                             {PlayerFields.PvpWaterResistPercent, new StatsData(Owner, PlayerFields.PvpWaterResistPercent, 0)},
                             {PlayerFields.PvpAirResistPercent, new StatsData(Owner, PlayerFields.PvpAirResistPercent, 0)},
                             {PlayerFields.PvpFireResistPercent, new StatsData(Owner, PlayerFields.PvpFireResistPercent, 0)},
                             {PlayerFields.PvpNeutralElementReduction, new StatsData(Owner, PlayerFields.PvpNeutralElementReduction, 0)},
                             {PlayerFields.PvpEarthElementReduction, new StatsData(Owner, PlayerFields.PvpEarthElementReduction, 0)},
                             {PlayerFields.PvpWaterElementReduction, new StatsData(Owner, PlayerFields.PvpWaterElementReduction, 0)},
                             {PlayerFields.PvpAirElementReduction, new StatsData(Owner, PlayerFields.PvpAirElementReduction, 0)},
                             {PlayerFields.PvpFireElementReduction, new StatsData(Owner, PlayerFields.PvpFireElementReduction, 0)},
                             {PlayerFields.GlobalDamageReduction, new StatsData(Owner, PlayerFields.GlobalDamageReduction, 0)},
                             {PlayerFields.DamageMultiplicator, new StatsData(Owner, PlayerFields.DamageMultiplicator, 0)},
                             {PlayerFields.PhysicalDamage, new StatsData(Owner, PlayerFields.PhysicalDamage, 0)},
                             {PlayerFields.MagicDamage, new StatsData(Owner, PlayerFields.MagicDamage, 0)},
                             {PlayerFields.PhysicalDamageReduction, new StatsData(Owner, PlayerFields.PhysicalDamageReduction, 0)},
                             {PlayerFields.MagicDamageReduction, new StatsData(Owner, PlayerFields.MagicDamageReduction, 0)},
                             // custom fields

                             {PlayerFields.WaterDamageArmor, new StatsData(Owner, PlayerFields.WaterDamageArmor, 0)},
                             {PlayerFields.EarthDamageArmor, new StatsData(Owner, PlayerFields.EarthDamageArmor, 0)},
                             {PlayerFields.NeutralDamageArmor, new StatsData(Owner, PlayerFields.NeutralDamageArmor, 0)},
                             {PlayerFields.AirDamageArmor, new StatsData(Owner, PlayerFields.AirDamageArmor, 0)},
                             {PlayerFields.FireDamageArmor, new StatsData(Owner, PlayerFields.FireDamageArmor, 0)},
                         };
        }
    }
}