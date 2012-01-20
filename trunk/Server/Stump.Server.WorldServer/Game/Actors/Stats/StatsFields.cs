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

        private static readonly StatsFormulasHandler FormuleInitiative =
            (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
                {
                    return owner.Stats.Health.Total <= 0
                               ? 0
                               : (valueBase + valueEquiped + valueContext +
                                  owner.Stats[CaracteristicsEnum.Chance] +
                                  owner.Stats[CaracteristicsEnum.Intelligence] +
                                  owner.Stats[CaracteristicsEnum.Agility] +
                                  owner.Stats[CaracteristicsEnum.Strength])*
                                 (owner.Stats.Health.Total/
                                  owner.Stats.Health.TotalMax);
                };

        private static readonly StatsFormulasHandler FormuleProspecting =
            (owner, valueBase, valueEquiped, valueGiven, valueContext) =>
            valueBase + valueEquiped + valueContext + (int) (owner.Stats[CaracteristicsEnum.Chance]/10d);

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

        public Dictionary<CaracteristicsEnum, StatsData> Fields
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
            get { return this[CaracteristicsEnum.Health] as StatsHealth; }
        }

        public StatsAP AP
        {
            get { return this[CaracteristicsEnum.AP] as StatsAP; }
        }

        public StatsMP MP
        {
            get
            {
                return this[CaracteristicsEnum.MP] as StatsMP;
            }
        }

        public StatsData this[CaracteristicsEnum name]
        {
            get
            {
                StatsData value;
                return Fields.TryGetValue(name, out value) ? value : null;
            }
        }

        public void Initialize(CharacterRecord record)
        {
            Fields = new Dictionary<CaracteristicsEnum, StatsData>
                         {
                             {CaracteristicsEnum.Health, new StatsHealth(Owner, (short) record.BaseHealth, (short) record.DamageTaken)},
                             {CaracteristicsEnum.Initiative, new StatsData(Owner, CaracteristicsEnum.Initiative, 0, FormuleInitiative)},
                             {CaracteristicsEnum.Prospecting, new StatsData(Owner, CaracteristicsEnum.Prospecting, (short) record.Prospection, FormuleProspecting)},
                             {CaracteristicsEnum.AP, new StatsAP(Owner, (short) record.AP)},
                             {CaracteristicsEnum.MP, new StatsMP(Owner, (short) record.MP)},
                             {CaracteristicsEnum.Strength, new StatsData(Owner, CaracteristicsEnum.Strength, record.Strength)},
                             {CaracteristicsEnum.Vitality, new StatsData(Owner, CaracteristicsEnum.Vitality, record.Vitality)},
                             {CaracteristicsEnum.Wisdom, new StatsData(Owner, CaracteristicsEnum.Wisdom, record.Wisdom)},
                             {CaracteristicsEnum.Chance, new StatsData(Owner, CaracteristicsEnum.Chance, record.Chance)},
                             {CaracteristicsEnum.Agility, new StatsData(Owner, CaracteristicsEnum.Agility, record.Agility)},
                             {CaracteristicsEnum.Intelligence, new StatsData(Owner, CaracteristicsEnum.Intelligence, record.Intelligence)},
                             {CaracteristicsEnum.Range, new StatsData(Owner, CaracteristicsEnum.Range, 0)},
                             {CaracteristicsEnum.SummonLimit, new StatsData(Owner, CaracteristicsEnum.SummonLimit, 1)},
                             {CaracteristicsEnum.DamageReflection, new StatsData(Owner, CaracteristicsEnum.DamageReflection, 0)},
                             {CaracteristicsEnum.CriticalHit, new StatsData(Owner, CaracteristicsEnum.CriticalHit, 0)},
                             {CaracteristicsEnum.CriticalMiss, new StatsData(Owner, CaracteristicsEnum.CriticalMiss, 0)},
                             {CaracteristicsEnum.HealBonus, new StatsData(Owner, CaracteristicsEnum.HealBonus, 0)},
                             {CaracteristicsEnum.DamageBonus, new StatsData(Owner, CaracteristicsEnum.DamageBonus, 0)},
                             {CaracteristicsEnum.WeaponDamageBonus, new StatsData(Owner, CaracteristicsEnum.WeaponDamageBonus, 0)},
                             {CaracteristicsEnum.DamageBonusPercent, new StatsData(Owner, CaracteristicsEnum.DamageBonusPercent, 0)},
                             {CaracteristicsEnum.TrapBonus, new StatsData(Owner, CaracteristicsEnum.TrapBonus, 0)},
                             {CaracteristicsEnum.TrapBonusPercent, new StatsData(Owner, CaracteristicsEnum.TrapBonusPercent, 0)},
                             {CaracteristicsEnum.PermanentDamagePercent, new StatsData(Owner, CaracteristicsEnum.PermanentDamagePercent, 0)},
                             {CaracteristicsEnum.TackleBlock, new StatsData(Owner, CaracteristicsEnum.TackleBlock, 0)},
                             {CaracteristicsEnum.TackleEvade, new StatsData(Owner, CaracteristicsEnum.TackleEvade, 0)},
                             {CaracteristicsEnum.APAttack, new StatsData(Owner, CaracteristicsEnum.APAttack, 0)},
                             {CaracteristicsEnum.MPAttack, new StatsData(Owner, CaracteristicsEnum.MPAttack, 0)},
                             {CaracteristicsEnum.PushDamageBonus, new StatsData(Owner, CaracteristicsEnum.PushDamageBonus, 0)},
                             {CaracteristicsEnum.CriticalDamageBonus, new StatsData(Owner, CaracteristicsEnum.CriticalDamageBonus, 0)},
                             {CaracteristicsEnum.NeutralDamageBonus, new StatsData(Owner, CaracteristicsEnum.NeutralDamageBonus, 0)},
                             {CaracteristicsEnum.EarthDamageBonus, new StatsData(Owner, CaracteristicsEnum.EarthDamageBonus, 0)},
                             {CaracteristicsEnum.WaterDamageBonus, new StatsData(Owner, CaracteristicsEnum.WaterDamageBonus, 0)},
                             {CaracteristicsEnum.AirDamageBonus, new StatsData(Owner, CaracteristicsEnum.AirDamageBonus, 0)},
                             {CaracteristicsEnum.FireDamageBonus, new StatsData(Owner, CaracteristicsEnum.FireDamageBonus, 0)},
                             {CaracteristicsEnum.DodgeAPProbability, new StatsData(Owner, CaracteristicsEnum.DodgeAPProbability, 0)},
                             {CaracteristicsEnum.DodgeMPProbability, new StatsData(Owner, CaracteristicsEnum.DodgeMPProbability, 0)},
                             {CaracteristicsEnum.NeutralResistPercent, new StatsData(Owner, CaracteristicsEnum.NeutralResistPercent, 0)},
                             {CaracteristicsEnum.EarthResistPercent, new StatsData(Owner, CaracteristicsEnum.EarthResistPercent, 0)},
                             {CaracteristicsEnum.WaterResistPercent, new StatsData(Owner, CaracteristicsEnum.WaterResistPercent, 0)},
                             {CaracteristicsEnum.AirResistPercent, new StatsData(Owner, CaracteristicsEnum.AirResistPercent, 0)},
                             {CaracteristicsEnum.FireResistPercent, new StatsData(Owner, CaracteristicsEnum.FireResistPercent, 0)},
                             {CaracteristicsEnum.NeutralElementReduction, new StatsData(Owner, CaracteristicsEnum.NeutralElementReduction, 0)},
                             {CaracteristicsEnum.EarthElementReduction, new StatsData(Owner, CaracteristicsEnum.EarthElementReduction, 0)},
                             {CaracteristicsEnum.WaterElementReduction, new StatsData(Owner, CaracteristicsEnum.WaterElementReduction, 0)},
                             {CaracteristicsEnum.AirElementReduction, new StatsData(Owner, CaracteristicsEnum.AirElementReduction, 0)},
                             {CaracteristicsEnum.FireElementReduction, new StatsData(Owner, CaracteristicsEnum.FireElementReduction, 0)},
                             {CaracteristicsEnum.PushDamageReduction, new StatsData(Owner, CaracteristicsEnum.PushDamageReduction, 0)},
                             {CaracteristicsEnum.CriticalDamageReduction, new StatsData(Owner, CaracteristicsEnum.CriticalDamageReduction, 0)},
                             {CaracteristicsEnum.PvpNeutralResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpNeutralResistPercent, 0)},
                             {CaracteristicsEnum.PvpEarthResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpEarthResistPercent, 0)},
                             {CaracteristicsEnum.PvpWaterResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpWaterResistPercent, 0)},
                             {CaracteristicsEnum.PvpAirResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpAirResistPercent, 0)},
                             {CaracteristicsEnum.PvpFireResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpFireResistPercent, 0)},
                             {CaracteristicsEnum.PvpNeutralElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpNeutralElementReduction, 0)},
                             {CaracteristicsEnum.PvpEarthElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpEarthElementReduction, 0)},
                             {CaracteristicsEnum.PvpWaterElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpWaterElementReduction, 0)},
                             {CaracteristicsEnum.PvpAirElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpAirElementReduction, 0)},
                             {CaracteristicsEnum.PvpFireElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpFireElementReduction, 0)},
                             {CaracteristicsEnum.GlobalDamageReduction, new StatsData(Owner, CaracteristicsEnum.GlobalDamageReduction, 0)},
                             {CaracteristicsEnum.DamageMultiplicator, new StatsData(Owner, CaracteristicsEnum.DamageMultiplicator, 0)},
                             {CaracteristicsEnum.PhysicalDamage, new StatsData(Owner, CaracteristicsEnum.PhysicalDamage, 0)},
                             {CaracteristicsEnum.MagicDamage, new StatsData(Owner, CaracteristicsEnum.MagicDamage, 0)},
                             {CaracteristicsEnum.PhysicalDamageReduction, new StatsData(Owner, CaracteristicsEnum.PhysicalDamageReduction, 0)},
                             {CaracteristicsEnum.MagicDamageReduction, new StatsData(Owner, CaracteristicsEnum.MagicDamageReduction, 0)},
                             // custom fields

                             {CaracteristicsEnum.WaterDamageArmor, new StatsData(Owner, CaracteristicsEnum.WaterDamageArmor, 0)},
                             {CaracteristicsEnum.EarthDamageArmor, new StatsData(Owner, CaracteristicsEnum.EarthDamageArmor, 0)},
                             {CaracteristicsEnum.NeutralDamageArmor, new StatsData(Owner, CaracteristicsEnum.NeutralDamageArmor, 0)},
                             {CaracteristicsEnum.AirDamageArmor, new StatsData(Owner, CaracteristicsEnum.AirDamageArmor, 0)},
                             {CaracteristicsEnum.FireDamageArmor, new StatsData(Owner, CaracteristicsEnum.FireDamageArmor, 0)},
                         };
        }

        public void Initialize(MonsterGrade record)
        {
            Fields = new Dictionary<CaracteristicsEnum, StatsData>
                         {
                             {CaracteristicsEnum.Health, new StatsHealth(Owner, (short) record.LifePoints, 0)},
                             {CaracteristicsEnum.Initiative, new StatsData(Owner, CaracteristicsEnum.Initiative, 0, FormuleInitiative)},
                             {CaracteristicsEnum.Prospecting, new StatsData(Owner, CaracteristicsEnum.Prospecting, 100, FormuleProspecting)},
                             {CaracteristicsEnum.AP, new StatsAP(Owner, (short) record.ActionPoints)},
                             {CaracteristicsEnum.MP, new StatsMP(Owner, (short) record.MovementPoints)},
                             {CaracteristicsEnum.Strength, new StatsData(Owner, CaracteristicsEnum.Strength, record.Strength)},
                             {CaracteristicsEnum.Vitality, new StatsData(Owner, CaracteristicsEnum.Vitality, record.Vitality)},
                             {CaracteristicsEnum.Wisdom, new StatsData(Owner, CaracteristicsEnum.Wisdom, record.Wisdom)},
                             {CaracteristicsEnum.Chance, new StatsData(Owner, CaracteristicsEnum.Chance, record.Chance)},
                             {CaracteristicsEnum.Agility, new StatsData(Owner, CaracteristicsEnum.Agility, record.Agility)},
                             {CaracteristicsEnum.Intelligence, new StatsData(Owner, CaracteristicsEnum.Intelligence, record.Intelligence)},
                             {CaracteristicsEnum.Range, new StatsData(Owner, CaracteristicsEnum.Range, 0)},
                             {CaracteristicsEnum.SummonLimit, new StatsData(Owner, CaracteristicsEnum.SummonLimit, 1)},
                             {CaracteristicsEnum.DamageReflection, new StatsData(Owner, CaracteristicsEnum.DamageReflection, 0)},
                             {CaracteristicsEnum.CriticalHit, new StatsData(Owner, CaracteristicsEnum.CriticalHit, 0)},
                             {CaracteristicsEnum.CriticalMiss, new StatsData(Owner, CaracteristicsEnum.CriticalMiss, 0)},
                             {CaracteristicsEnum.HealBonus, new StatsData(Owner, CaracteristicsEnum.HealBonus, 0)},
                             {CaracteristicsEnum.DamageBonus, new StatsData(Owner, CaracteristicsEnum.DamageBonus, 0)},
                             {CaracteristicsEnum.WeaponDamageBonus, new StatsData(Owner, CaracteristicsEnum.WeaponDamageBonus, 0)},
                             {CaracteristicsEnum.DamageBonusPercent, new StatsData(Owner, CaracteristicsEnum.DamageBonusPercent, 0)},
                             {CaracteristicsEnum.TrapBonus, new StatsData(Owner, CaracteristicsEnum.TrapBonus, 0)},
                             {CaracteristicsEnum.TrapBonusPercent, new StatsData(Owner, CaracteristicsEnum.TrapBonusPercent, 0)},
                             {CaracteristicsEnum.PermanentDamagePercent, new StatsData(Owner, CaracteristicsEnum.PermanentDamagePercent, 0)},
                             {CaracteristicsEnum.TackleBlock, new StatsData(Owner, CaracteristicsEnum.TackleBlock, 0)},
                             {CaracteristicsEnum.TackleEvade, new StatsData(Owner, CaracteristicsEnum.TackleEvade, 0)},
                             {CaracteristicsEnum.APAttack, new StatsData(Owner, CaracteristicsEnum.APAttack, 0)},
                             {CaracteristicsEnum.MPAttack, new StatsData(Owner, CaracteristicsEnum.MPAttack, 0)},
                             {CaracteristicsEnum.PushDamageBonus, new StatsData(Owner, CaracteristicsEnum.PushDamageBonus, 0)},
                             {CaracteristicsEnum.CriticalDamageBonus, new StatsData(Owner, CaracteristicsEnum.CriticalDamageBonus, 0)},
                             {CaracteristicsEnum.NeutralDamageBonus, new StatsData(Owner, CaracteristicsEnum.NeutralDamageBonus, 0)},
                             {CaracteristicsEnum.EarthDamageBonus, new StatsData(Owner, CaracteristicsEnum.EarthDamageBonus, 0)},
                             {CaracteristicsEnum.WaterDamageBonus, new StatsData(Owner, CaracteristicsEnum.WaterDamageBonus, 0)},
                             {CaracteristicsEnum.AirDamageBonus, new StatsData(Owner, CaracteristicsEnum.AirDamageBonus, 0)},
                             {CaracteristicsEnum.FireDamageBonus, new StatsData(Owner, CaracteristicsEnum.FireDamageBonus, 0)},
                             {CaracteristicsEnum.DodgeAPProbability, new StatsData(Owner, CaracteristicsEnum.DodgeAPProbability, (short) record.PaDodge)},
                             {CaracteristicsEnum.DodgeMPProbability, new StatsData(Owner, CaracteristicsEnum.DodgeMPProbability, (short) record.PmDodge)},
                             {CaracteristicsEnum.NeutralResistPercent, new StatsData(Owner, CaracteristicsEnum.NeutralResistPercent, (short) record.NeutralResistance)},
                             {CaracteristicsEnum.EarthResistPercent, new StatsData(Owner, CaracteristicsEnum.EarthResistPercent, (short) record.EarthResistance)},
                             {CaracteristicsEnum.WaterResistPercent, new StatsData(Owner, CaracteristicsEnum.WaterResistPercent, (short) record.WaterResistance)},
                             {CaracteristicsEnum.AirResistPercent, new StatsData(Owner, CaracteristicsEnum.AirResistPercent, (short) record.AirResistance)},
                             {CaracteristicsEnum.FireResistPercent, new StatsData(Owner, CaracteristicsEnum.FireResistPercent, (short) record.FireResistance)},
                             {CaracteristicsEnum.NeutralElementReduction, new StatsData(Owner, CaracteristicsEnum.NeutralElementReduction, 0)},
                             {CaracteristicsEnum.EarthElementReduction, new StatsData(Owner, CaracteristicsEnum.EarthElementReduction, 0)},
                             {CaracteristicsEnum.WaterElementReduction, new StatsData(Owner, CaracteristicsEnum.WaterElementReduction, 0)},
                             {CaracteristicsEnum.AirElementReduction, new StatsData(Owner, CaracteristicsEnum.AirElementReduction, 0)},
                             {CaracteristicsEnum.FireElementReduction, new StatsData(Owner, CaracteristicsEnum.FireElementReduction, 0)},
                             {CaracteristicsEnum.PushDamageReduction, new StatsData(Owner, CaracteristicsEnum.PushDamageReduction, 0)},
                             {CaracteristicsEnum.CriticalDamageReduction, new StatsData(Owner, CaracteristicsEnum.CriticalDamageReduction, 0)},
                             {CaracteristicsEnum.PvpNeutralResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpNeutralResistPercent, 0)},
                             {CaracteristicsEnum.PvpEarthResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpEarthResistPercent, 0)},
                             {CaracteristicsEnum.PvpWaterResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpWaterResistPercent, 0)},
                             {CaracteristicsEnum.PvpAirResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpAirResistPercent, 0)},
                             {CaracteristicsEnum.PvpFireResistPercent, new StatsData(Owner, CaracteristicsEnum.PvpFireResistPercent, 0)},
                             {CaracteristicsEnum.PvpNeutralElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpNeutralElementReduction, 0)},
                             {CaracteristicsEnum.PvpEarthElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpEarthElementReduction, 0)},
                             {CaracteristicsEnum.PvpWaterElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpWaterElementReduction, 0)},
                             {CaracteristicsEnum.PvpAirElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpAirElementReduction, 0)},
                             {CaracteristicsEnum.PvpFireElementReduction, new StatsData(Owner, CaracteristicsEnum.PvpFireElementReduction, 0)},
                             {CaracteristicsEnum.GlobalDamageReduction, new StatsData(Owner, CaracteristicsEnum.GlobalDamageReduction, 0)},
                             {CaracteristicsEnum.DamageMultiplicator, new StatsData(Owner, CaracteristicsEnum.DamageMultiplicator, 0)},
                             {CaracteristicsEnum.PhysicalDamage, new StatsData(Owner, CaracteristicsEnum.PhysicalDamage, 0)},
                             {CaracteristicsEnum.MagicDamage, new StatsData(Owner, CaracteristicsEnum.MagicDamage, 0)},
                             {CaracteristicsEnum.PhysicalDamageReduction, new StatsData(Owner, CaracteristicsEnum.PhysicalDamageReduction, 0)},
                             {CaracteristicsEnum.MagicDamageReduction, new StatsData(Owner, CaracteristicsEnum.MagicDamageReduction, 0)},
                             // custom fields

                             {CaracteristicsEnum.WaterDamageArmor, new StatsData(Owner, CaracteristicsEnum.WaterDamageArmor, 0)},
                             {CaracteristicsEnum.EarthDamageArmor, new StatsData(Owner, CaracteristicsEnum.EarthDamageArmor, 0)},
                             {CaracteristicsEnum.NeutralDamageArmor, new StatsData(Owner, CaracteristicsEnum.NeutralDamageArmor, 0)},
                             {CaracteristicsEnum.AirDamageArmor, new StatsData(Owner, CaracteristicsEnum.AirDamageArmor, 0)},
                             {CaracteristicsEnum.FireDamageArmor, new StatsData(Owner, CaracteristicsEnum.FireDamageArmor, 0)},
                         };
        }
    }
}