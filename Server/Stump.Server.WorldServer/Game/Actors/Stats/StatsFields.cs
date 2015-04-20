using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;

namespace Stump.Server.WorldServer.Game.Actors.Stats
{
    public delegate short StatsFormulasHandler(IStatsOwner target);

    public class StatsFields
    {
        [Variable]
        public static int MPLimit = 6;

        [Variable]
        public static int APLimit = 12;
        
        [Variable]
        public static int ResistanceLimit = 50;

        [Variable]
        public static int RangeLimit = 6;

        #region Formulas

        private static readonly StatsFormulasHandler FormulasChanceDependant =
            owner =>
            (short) (owner.Stats[PlayerFields.Chance]/10d);

        private static readonly StatsFormulasHandler FormulasWisdomDependant =
             owner =>
                 (short) ( owner.Stats[PlayerFields.Wisdom] / 10d );


        private static readonly StatsFormulasHandler FormulasAgilityDependant =
             owner =>
                 (short) ( owner.Stats[PlayerFields.Agility] / 10d );

        #endregion

        public StatsFields(IStatsOwner owner)
        {
            Owner = owner;
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

        public StatsData Shield
        {
            get { return this[PlayerFields.Shield]; }
        }

        public StatsAP AP
        {
            get { return this[PlayerFields.AP] as StatsAP; }
        }

        public StatsMP MP
        {
            get { return this[PlayerFields.MP] as StatsMP; }
        }

        public StatsData Vitality
        {
            get { return this[PlayerFields.Vitality]; }
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

        public int GetTotal(PlayerFields name)
        {
            var field = this[name];

            return field == null ? 0 : field.Total;
        }

        public void Initialize(CharacterRecord record)
        {
            // note : keep this order !
            Fields = new Dictionary<PlayerFields, StatsData>();

            Fields.Add(PlayerFields.Initiative, new StatsInitiative(Owner, 0));
            Fields.Add(PlayerFields.Prospecting, new StatsData(Owner, PlayerFields.Prospecting, (short)record.Prospection, FormulasChanceDependant));
            Fields.Add(PlayerFields.AP, new StatsAP(Owner, (short)record.AP, APLimit));
            Fields.Add(PlayerFields.MP, new StatsMP(Owner, (short)record.MP, MPLimit));
            Fields.Add(PlayerFields.Strength, new StatsData(Owner, PlayerFields.Strength, record.Strength));
            Fields.Add(PlayerFields.Vitality, new StatsData(Owner, PlayerFields.Vitality, record.Vitality));
            Fields.Add(PlayerFields.Health, new StatsHealth(Owner, (short)record.BaseHealth, (short)record.DamageTaken));
            Fields.Add(PlayerFields.Wisdom, new StatsData(Owner, PlayerFields.Wisdom, record.Wisdom));
            Fields.Add(PlayerFields.Chance, new StatsData(Owner, PlayerFields.Chance, record.Chance));
            Fields.Add(PlayerFields.Agility, new StatsData(Owner, PlayerFields.Agility, record.Agility));
            Fields.Add(PlayerFields.Intelligence, new StatsData(Owner, PlayerFields.Intelligence, record.Intelligence));
            Fields.Add(PlayerFields.Range, new StatsData(Owner, PlayerFields.Range, 0, RangeLimit, true));
            Fields.Add(PlayerFields.SummonLimit, new StatsData(Owner, PlayerFields.SummonLimit, 1));
            Fields.Add(PlayerFields.DamageReflection, new StatsData(Owner, PlayerFields.DamageReflection, 0));
            Fields.Add(PlayerFields.CriticalHit, new StatsData(Owner, PlayerFields.CriticalHit, 0));
            Fields.Add(PlayerFields.CriticalMiss, new StatsData(Owner, PlayerFields.CriticalMiss, 0));
            Fields.Add(PlayerFields.HealBonus, new StatsData(Owner, PlayerFields.HealBonus, 0));
            Fields.Add(PlayerFields.DamageBonus, new StatsData(Owner, PlayerFields.DamageBonus, 0));
            Fields.Add(PlayerFields.WeaponDamageBonus, new StatsData(Owner, PlayerFields.WeaponDamageBonus, 0));
            Fields.Add(PlayerFields.DamageBonusPercent, new StatsData(Owner, PlayerFields.DamageBonusPercent, 0));
            Fields.Add(PlayerFields.TrapBonus, new StatsData(Owner, PlayerFields.TrapBonus, 0));
            Fields.Add(PlayerFields.TrapBonusPercent, new StatsData(Owner, PlayerFields.TrapBonusPercent, 0));
            Fields.Add(PlayerFields.PermanentDamagePercent, new StatsData(Owner, PlayerFields.PermanentDamagePercent, 0));
            Fields.Add(PlayerFields.TackleBlock, new StatsData(Owner, PlayerFields.TackleBlock, 0, FormulasAgilityDependant));
            Fields.Add(PlayerFields.TackleEvade, new StatsData(Owner, PlayerFields.TackleEvade, 0, FormulasAgilityDependant));
            Fields.Add(PlayerFields.APAttack, new StatsData(Owner, PlayerFields.APAttack, 0, FormulasWisdomDependant));
            Fields.Add(PlayerFields.MPAttack, new StatsData(Owner, PlayerFields.MPAttack, 0, FormulasWisdomDependant));
            Fields.Add(PlayerFields.PushDamageBonus, new StatsData(Owner, PlayerFields.PushDamageBonus, 0));
            Fields.Add(PlayerFields.CriticalDamageBonus, new StatsData(Owner, PlayerFields.CriticalDamageBonus, 0));
            Fields.Add(PlayerFields.NeutralDamageBonus, new StatsData(Owner, PlayerFields.NeutralDamageBonus, 0));
            Fields.Add(PlayerFields.EarthDamageBonus, new StatsData(Owner, PlayerFields.EarthDamageBonus, 0));
            Fields.Add(PlayerFields.WaterDamageBonus, new StatsData(Owner, PlayerFields.WaterDamageBonus, 0));
            Fields.Add(PlayerFields.AirDamageBonus, new StatsData(Owner, PlayerFields.AirDamageBonus, 0));
            Fields.Add(PlayerFields.FireDamageBonus, new StatsData(Owner, PlayerFields.FireDamageBonus, 0));
            Fields.Add(PlayerFields.DodgeAPProbability, new StatsData(Owner, PlayerFields.DodgeAPProbability, 0, FormulasWisdomDependant));
            Fields.Add(PlayerFields.DodgeMPProbability, new StatsData(Owner, PlayerFields.DodgeMPProbability, 0, FormulasWisdomDependant));
            Fields.Add(PlayerFields.NeutralResistPercent, new StatsData(Owner, PlayerFields.NeutralResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.EarthResistPercent, new StatsData(Owner, PlayerFields.EarthResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.WaterResistPercent, new StatsData(Owner, PlayerFields.WaterResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.AirResistPercent, new StatsData(Owner, PlayerFields.AirResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.FireResistPercent, new StatsData(Owner, PlayerFields.FireResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.NeutralElementReduction, new StatsData(Owner, PlayerFields.NeutralElementReduction, 0));
            Fields.Add(PlayerFields.EarthElementReduction, new StatsData(Owner, PlayerFields.EarthElementReduction, 0));
            Fields.Add(PlayerFields.WaterElementReduction, new StatsData(Owner, PlayerFields.WaterElementReduction, 0));
            Fields.Add(PlayerFields.AirElementReduction, new StatsData(Owner, PlayerFields.AirElementReduction, 0));
            Fields.Add(PlayerFields.FireElementReduction, new StatsData(Owner, PlayerFields.FireElementReduction, 0));
            Fields.Add(PlayerFields.PushDamageReduction, new StatsData(Owner, PlayerFields.PushDamageReduction, 0));
            Fields.Add(PlayerFields.CriticalDamageReduction, new StatsData(Owner, PlayerFields.CriticalDamageReduction, 0));
            Fields.Add(PlayerFields.PvpNeutralResistPercent, new StatsData(Owner, PlayerFields.PvpNeutralResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.PvpEarthResistPercent, new StatsData(Owner, PlayerFields.PvpEarthResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.PvpWaterResistPercent, new StatsData(Owner, PlayerFields.PvpWaterResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.PvpAirResistPercent, new StatsData(Owner, PlayerFields.PvpAirResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.PvpFireResistPercent, new StatsData(Owner, PlayerFields.PvpFireResistPercent, 0, ResistanceLimit));
            Fields.Add(PlayerFields.PvpNeutralElementReduction, new StatsData(Owner, PlayerFields.PvpNeutralElementReduction, 0));
            Fields.Add(PlayerFields.PvpEarthElementReduction, new StatsData(Owner, PlayerFields.PvpEarthElementReduction, 0));
            Fields.Add(PlayerFields.PvpWaterElementReduction, new StatsData(Owner, PlayerFields.PvpWaterElementReduction, 0));
            Fields.Add(PlayerFields.PvpAirElementReduction, new StatsData(Owner, PlayerFields.PvpAirElementReduction, 0));
            Fields.Add(PlayerFields.PvpFireElementReduction, new StatsData(Owner, PlayerFields.PvpFireElementReduction, 0));
            Fields.Add(PlayerFields.GlobalDamageReduction, new StatsData(Owner, PlayerFields.GlobalDamageReduction, 0));
            Fields.Add(PlayerFields.DamageMultiplicator, new StatsData(Owner, PlayerFields.DamageMultiplicator, 0));
            Fields.Add(PlayerFields.PhysicalDamage, new StatsData(Owner, PlayerFields.PhysicalDamage, 0));
            Fields.Add(PlayerFields.MagicDamage, new StatsData(Owner, PlayerFields.MagicDamage, 0));
            Fields.Add(PlayerFields.PhysicalDamageReduction, new StatsData(Owner, PlayerFields.PhysicalDamageReduction, 0));
            Fields.Add(PlayerFields.MagicDamageReduction, new StatsData(Owner, PlayerFields.MagicDamageReduction, 0));
            Fields.Add(PlayerFields.Weight, new StatsData(Owner, PlayerFields.Weight, 0));

            // custom fields
            Fields.Add(PlayerFields.WaterDamageArmor, new StatsData(Owner, PlayerFields.WaterDamageArmor, 0));
            Fields.Add(PlayerFields.EarthDamageArmor, new StatsData(Owner, PlayerFields.EarthDamageArmor, 0));
            Fields.Add(PlayerFields.NeutralDamageArmor, new StatsData(Owner, PlayerFields.NeutralDamageArmor, 0));
            Fields.Add(PlayerFields.AirDamageArmor, new StatsData(Owner, PlayerFields.AirDamageArmor, 0));
            Fields.Add(PlayerFields.FireDamageArmor, new StatsData(Owner, PlayerFields.FireDamageArmor, 0));
            Fields.Add(PlayerFields.Erosion, new StatsData(Owner, PlayerFields.Erosion, 10));
            Fields.Add(PlayerFields.ComboBonus, new StatsData(Owner, PlayerFields.ComboBonus, 0));
            Fields.Add(PlayerFields.Shield, new StatsData(Owner, PlayerFields.Shield, 0));
        }

        public void Initialize(MonsterGrade record)
        {
            // note : keep this order !

            Fields = new Dictionary<PlayerFields, StatsData>();

            Fields.Add(PlayerFields.Initiative, new StatsInitiative(Owner, 0));
            Fields.Add(PlayerFields.Prospecting, new StatsData(Owner, PlayerFields.Prospecting, 100, FormulasChanceDependant));
            Fields.Add(PlayerFields.AP, new StatsAP(Owner, (short) record.ActionPoints));
            Fields.Add(PlayerFields.MP, new StatsMP(Owner, (short) record.MovementPoints));
            Fields.Add(PlayerFields.Strength, new StatsData(Owner, PlayerFields.Strength, record.Strength));
            Fields.Add(PlayerFields.Vitality, new StatsData(Owner, PlayerFields.Vitality, record.Vitality));                             
            Fields.Add(PlayerFields.Health, new StatsHealth(Owner, (short) record.LifePoints, 0));
            Fields.Add(PlayerFields.Wisdom, new StatsData(Owner, PlayerFields.Wisdom, record.Wisdom));
            Fields.Add(PlayerFields.Chance, new StatsData(Owner, PlayerFields.Chance, record.Chance));
            Fields.Add(PlayerFields.Agility, new StatsData(Owner, PlayerFields.Agility, record.Agility));
            Fields.Add(PlayerFields.Intelligence, new StatsData(Owner, PlayerFields.Intelligence, record.Intelligence));
            Fields.Add(PlayerFields.Range, new StatsData(Owner, PlayerFields.Range, 0));
            Fields.Add(PlayerFields.SummonLimit, new StatsData(Owner, PlayerFields.SummonLimit, 1));
            Fields.Add(PlayerFields.DamageReflection, new StatsData(Owner, PlayerFields.DamageReflection, 0));
            Fields.Add(PlayerFields.CriticalHit, new StatsData(Owner, PlayerFields.CriticalHit, 0));
            Fields.Add(PlayerFields.CriticalMiss, new StatsData(Owner, PlayerFields.CriticalMiss, 0));
            Fields.Add(PlayerFields.HealBonus, new StatsData(Owner, PlayerFields.HealBonus, 0));
            Fields.Add(PlayerFields.DamageBonus, new StatsData(Owner, PlayerFields.DamageBonus, 0));
            Fields.Add(PlayerFields.WeaponDamageBonus, new StatsData(Owner, PlayerFields.WeaponDamageBonus, 0));
            Fields.Add(PlayerFields.DamageBonusPercent, new StatsData(Owner, PlayerFields.DamageBonusPercent, 0));
            Fields.Add(PlayerFields.TrapBonus, new StatsData(Owner, PlayerFields.TrapBonus, 0));
            Fields.Add(PlayerFields.TrapBonusPercent, new StatsData(Owner, PlayerFields.TrapBonusPercent, 0));
            Fields.Add(PlayerFields.PermanentDamagePercent, new StatsData(Owner, PlayerFields.PermanentDamagePercent, 0));
            Fields.Add(PlayerFields.TackleBlock, new StatsData(Owner, PlayerFields.TackleBlock, record.TackleBlock, FormulasAgilityDependant));
            Fields.Add(PlayerFields.TackleEvade, new StatsData(Owner, PlayerFields.TackleEvade, record.TackleEvade, FormulasAgilityDependant));
            Fields.Add(PlayerFields.APAttack, new StatsData(Owner, PlayerFields.APAttack, 0));
            Fields.Add(PlayerFields.MPAttack, new StatsData(Owner, PlayerFields.MPAttack, 0));
            Fields.Add(PlayerFields.PushDamageBonus, new StatsData(Owner, PlayerFields.PushDamageBonus, 0));
            Fields.Add(PlayerFields.CriticalDamageBonus, new StatsData(Owner, PlayerFields.CriticalDamageBonus, 0));
            Fields.Add(PlayerFields.NeutralDamageBonus, new StatsData(Owner, PlayerFields.NeutralDamageBonus, 0));
            Fields.Add(PlayerFields.EarthDamageBonus, new StatsData(Owner, PlayerFields.EarthDamageBonus, 0));
            Fields.Add(PlayerFields.WaterDamageBonus, new StatsData(Owner, PlayerFields.WaterDamageBonus, 0));
            Fields.Add(PlayerFields.AirDamageBonus, new StatsData(Owner, PlayerFields.AirDamageBonus, 0));
            Fields.Add(PlayerFields.FireDamageBonus, new StatsData(Owner, PlayerFields.FireDamageBonus, 0));
            Fields.Add(PlayerFields.DodgeAPProbability, new StatsData(Owner, PlayerFields.DodgeAPProbability, (short) record.PaDodge, FormulasWisdomDependant));
            Fields.Add(PlayerFields.DodgeMPProbability, new StatsData(Owner, PlayerFields.DodgeMPProbability, (short) record.PmDodge, FormulasWisdomDependant));
            Fields.Add(PlayerFields.NeutralResistPercent, new StatsData(Owner, PlayerFields.NeutralResistPercent, (short) record.NeutralResistance));
            Fields.Add(PlayerFields.EarthResistPercent, new StatsData(Owner, PlayerFields.EarthResistPercent, (short) record.EarthResistance));
            Fields.Add(PlayerFields.WaterResistPercent, new StatsData(Owner, PlayerFields.WaterResistPercent, (short) record.WaterResistance));
            Fields.Add(PlayerFields.AirResistPercent, new StatsData(Owner, PlayerFields.AirResistPercent, (short) record.AirResistance));
            Fields.Add(PlayerFields.FireResistPercent, new StatsData(Owner, PlayerFields.FireResistPercent, (short) record.FireResistance));
            Fields.Add(PlayerFields.NeutralElementReduction, new StatsData(Owner, PlayerFields.NeutralElementReduction, 0));
            Fields.Add(PlayerFields.EarthElementReduction, new StatsData(Owner, PlayerFields.EarthElementReduction, 0));
            Fields.Add(PlayerFields.WaterElementReduction, new StatsData(Owner, PlayerFields.WaterElementReduction, 0));
            Fields.Add(PlayerFields.AirElementReduction, new StatsData(Owner, PlayerFields.AirElementReduction, 0));
            Fields.Add(PlayerFields.FireElementReduction, new StatsData(Owner, PlayerFields.FireElementReduction, 0));
            Fields.Add(PlayerFields.PushDamageReduction, new StatsData(Owner, PlayerFields.PushDamageReduction, 0));
            Fields.Add(PlayerFields.CriticalDamageReduction, new StatsData(Owner, PlayerFields.CriticalDamageReduction, 0));
            Fields.Add(PlayerFields.PvpNeutralResistPercent, new StatsData(Owner, PlayerFields.PvpNeutralResistPercent, 0));
            Fields.Add(PlayerFields.PvpEarthResistPercent, new StatsData(Owner, PlayerFields.PvpEarthResistPercent, 0));
            Fields.Add(PlayerFields.PvpWaterResistPercent, new StatsData(Owner, PlayerFields.PvpWaterResistPercent, 0));
            Fields.Add(PlayerFields.PvpAirResistPercent, new StatsData(Owner, PlayerFields.PvpAirResistPercent, 0));
            Fields.Add(PlayerFields.PvpFireResistPercent, new StatsData(Owner, PlayerFields.PvpFireResistPercent, 0));
            Fields.Add(PlayerFields.PvpNeutralElementReduction, new StatsData(Owner, PlayerFields.PvpNeutralElementReduction, 0));
            Fields.Add(PlayerFields.PvpEarthElementReduction, new StatsData(Owner, PlayerFields.PvpEarthElementReduction, 0));
            Fields.Add(PlayerFields.PvpWaterElementReduction, new StatsData(Owner, PlayerFields.PvpWaterElementReduction, 0));
            Fields.Add(PlayerFields.PvpAirElementReduction, new StatsData(Owner, PlayerFields.PvpAirElementReduction, 0));
            Fields.Add(PlayerFields.PvpFireElementReduction, new StatsData(Owner, PlayerFields.PvpFireElementReduction, 0));
            Fields.Add(PlayerFields.GlobalDamageReduction, new StatsData(Owner, PlayerFields.GlobalDamageReduction, 0));
            Fields.Add(PlayerFields.DamageMultiplicator, new StatsData(Owner, PlayerFields.DamageMultiplicator, 0));
            Fields.Add(PlayerFields.PhysicalDamage, new StatsData(Owner, PlayerFields.PhysicalDamage, 0));
            Fields.Add(PlayerFields.MagicDamage, new StatsData(Owner, PlayerFields.MagicDamage, 0));
            Fields.Add(PlayerFields.PhysicalDamageReduction, new StatsData(Owner, PlayerFields.PhysicalDamageReduction, 0));
            Fields.Add(PlayerFields.MagicDamageReduction, new StatsData(Owner, PlayerFields.MagicDamageReduction, 0));
            // custom fields

            Fields.Add(PlayerFields.WaterDamageArmor, new StatsData(Owner, PlayerFields.WaterDamageArmor, 0));
            Fields.Add(PlayerFields.EarthDamageArmor, new StatsData(Owner, PlayerFields.EarthDamageArmor, 0));
            Fields.Add(PlayerFields.NeutralDamageArmor, new StatsData(Owner, PlayerFields.NeutralDamageArmor, 0));
            Fields.Add(PlayerFields.AirDamageArmor, new StatsData(Owner, PlayerFields.AirDamageArmor, 0));
            Fields.Add(PlayerFields.FireDamageArmor, new StatsData(Owner, PlayerFields.FireDamageArmor, 0));
            Fields.Add(PlayerFields.Erosion, new StatsData(Owner, PlayerFields.Erosion, 10));
            Fields.Add(PlayerFields.ComboBonus, new StatsData(Owner, PlayerFields.ComboBonus, 0));
            Fields.Add(PlayerFields.Shield, new StatsData(Owner, PlayerFields.Shield, 0));

            foreach (var pair in record.Stats)
            {
                Fields[pair.Key].Base = pair.Value;
            }
        }

        public void Initialize(TaxCollectorNpc taxCollector)
        {
            // note : keep this order !

            Fields = new Dictionary<PlayerFields, StatsData>();

            Fields.Add(PlayerFields.Initiative, new StatsInitiative(Owner, 0));
            Fields.Add(PlayerFields.Prospecting, new StatsData(Owner, PlayerFields.Prospecting, taxCollector.Guild.TaxCollectorProspecting, FormulasChanceDependant));
            Fields.Add(PlayerFields.AP, new StatsAP(Owner, TaxCollectorNpc.BaseAP));
            Fields.Add(PlayerFields.MP, new StatsMP(Owner, TaxCollectorNpc.BaseMP));
            Fields.Add(PlayerFields.Strength, new StatsData(Owner, PlayerFields.Strength, 0));
            Fields.Add(PlayerFields.Vitality, new StatsData(Owner, PlayerFields.Vitality, 0));                             
            Fields.Add(PlayerFields.Health, new StatsHealth(Owner, taxCollector.Guild.TaxCollectorHealth, 0));
            Fields.Add(PlayerFields.Wisdom, new StatsData(Owner, PlayerFields.Wisdom, taxCollector.Guild.TaxCollectorWisdom));
            Fields.Add(PlayerFields.Chance, new StatsData(Owner, PlayerFields.Chance, 0));
            Fields.Add(PlayerFields.Agility, new StatsData(Owner, PlayerFields.Agility, 0));
            Fields.Add(PlayerFields.Intelligence, new StatsData(Owner, PlayerFields.Intelligence, 0));
            Fields.Add(PlayerFields.Range, new StatsData(Owner, PlayerFields.Range, 0));
            Fields.Add(PlayerFields.SummonLimit, new StatsData(Owner, PlayerFields.SummonLimit, 1));
            Fields.Add(PlayerFields.DamageReflection, new StatsData(Owner, PlayerFields.DamageReflection, 0));
            Fields.Add(PlayerFields.CriticalHit, new StatsData(Owner, PlayerFields.CriticalHit, 0));
            Fields.Add(PlayerFields.CriticalMiss, new StatsData(Owner, PlayerFields.CriticalMiss, 0));
            Fields.Add(PlayerFields.HealBonus, new StatsData(Owner, PlayerFields.HealBonus, 0));
            Fields.Add(PlayerFields.DamageBonus, new StatsData(Owner, PlayerFields.DamageBonus, taxCollector.Guild.TaxCollectorDamageBonuses));
            Fields.Add(PlayerFields.WeaponDamageBonus, new StatsData(Owner, PlayerFields.WeaponDamageBonus, 0));
            Fields.Add(PlayerFields.DamageBonusPercent, new StatsData(Owner, PlayerFields.DamageBonusPercent, 0));
            Fields.Add(PlayerFields.TrapBonus, new StatsData(Owner, PlayerFields.TrapBonus, 0));
            Fields.Add(PlayerFields.TrapBonusPercent, new StatsData(Owner, PlayerFields.TrapBonusPercent, 0));
            Fields.Add(PlayerFields.PermanentDamagePercent, new StatsData(Owner, PlayerFields.PermanentDamagePercent, 0));
            Fields.Add(PlayerFields.TackleBlock, new StatsData(Owner, PlayerFields.TackleBlock, 50, FormulasAgilityDependant));
            Fields.Add(PlayerFields.TackleEvade, new StatsData(Owner, PlayerFields.TackleEvade, 50, FormulasAgilityDependant));
            Fields.Add(PlayerFields.APAttack, new StatsData(Owner, PlayerFields.APAttack, 50));
            Fields.Add(PlayerFields.MPAttack, new StatsData(Owner, PlayerFields.MPAttack, 50));
            Fields.Add(PlayerFields.PushDamageBonus, new StatsData(Owner, PlayerFields.PushDamageBonus, 0));
            Fields.Add(PlayerFields.CriticalDamageBonus, new StatsData(Owner, PlayerFields.CriticalDamageBonus, 0));
            Fields.Add(PlayerFields.NeutralDamageBonus, new StatsData(Owner, PlayerFields.NeutralDamageBonus, 0));
            Fields.Add(PlayerFields.EarthDamageBonus, new StatsData(Owner, PlayerFields.EarthDamageBonus, 0));
            Fields.Add(PlayerFields.WaterDamageBonus, new StatsData(Owner, PlayerFields.WaterDamageBonus, 0));
            Fields.Add(PlayerFields.AirDamageBonus, new StatsData(Owner, PlayerFields.AirDamageBonus, 0));
            Fields.Add(PlayerFields.FireDamageBonus, new StatsData(Owner, PlayerFields.FireDamageBonus, 0));
            Fields.Add(PlayerFields.DodgeAPProbability, new StatsData(Owner, PlayerFields.DodgeAPProbability, taxCollector.Guild.TaxCollectorResistance, FormulasWisdomDependant));
            Fields.Add(PlayerFields.DodgeMPProbability, new StatsData(Owner, PlayerFields.DodgeMPProbability, taxCollector.Guild.TaxCollectorResistance, FormulasWisdomDependant));
            Fields.Add(PlayerFields.NeutralResistPercent, new StatsData(Owner, PlayerFields.NeutralResistPercent, taxCollector.Guild.TaxCollectorResistance));
            Fields.Add(PlayerFields.EarthResistPercent, new StatsData(Owner, PlayerFields.EarthResistPercent, taxCollector.Guild.TaxCollectorResistance));
            Fields.Add(PlayerFields.WaterResistPercent, new StatsData(Owner, PlayerFields.WaterResistPercent, taxCollector.Guild.TaxCollectorResistance));
            Fields.Add(PlayerFields.AirResistPercent, new StatsData(Owner, PlayerFields.AirResistPercent, taxCollector.Guild.TaxCollectorResistance));
            Fields.Add(PlayerFields.FireResistPercent, new StatsData(Owner, PlayerFields.FireResistPercent, taxCollector.Guild.TaxCollectorResistance));
            Fields.Add(PlayerFields.NeutralElementReduction, new StatsData(Owner, PlayerFields.NeutralElementReduction, 0));
            Fields.Add(PlayerFields.EarthElementReduction, new StatsData(Owner, PlayerFields.EarthElementReduction, 0));
            Fields.Add(PlayerFields.WaterElementReduction, new StatsData(Owner, PlayerFields.WaterElementReduction, 0));
            Fields.Add(PlayerFields.AirElementReduction, new StatsData(Owner, PlayerFields.AirElementReduction, 0));
            Fields.Add(PlayerFields.FireElementReduction, new StatsData(Owner, PlayerFields.FireElementReduction, 0));
            Fields.Add(PlayerFields.PushDamageReduction, new StatsData(Owner, PlayerFields.PushDamageReduction, 0));
            Fields.Add(PlayerFields.CriticalDamageReduction, new StatsData(Owner, PlayerFields.CriticalDamageReduction, 0));
            Fields.Add(PlayerFields.PvpNeutralResistPercent, new StatsData(Owner, PlayerFields.PvpNeutralResistPercent, 0));
            Fields.Add(PlayerFields.PvpEarthResistPercent, new StatsData(Owner, PlayerFields.PvpEarthResistPercent, 0));
            Fields.Add(PlayerFields.PvpWaterResistPercent, new StatsData(Owner, PlayerFields.PvpWaterResistPercent, 0));
            Fields.Add(PlayerFields.PvpAirResistPercent, new StatsData(Owner, PlayerFields.PvpAirResistPercent, 0));
            Fields.Add(PlayerFields.PvpFireResistPercent, new StatsData(Owner, PlayerFields.PvpFireResistPercent, 0));
            Fields.Add(PlayerFields.PvpNeutralElementReduction, new StatsData(Owner, PlayerFields.PvpNeutralElementReduction, 0));
            Fields.Add(PlayerFields.PvpEarthElementReduction, new StatsData(Owner, PlayerFields.PvpEarthElementReduction, 0));
            Fields.Add(PlayerFields.PvpWaterElementReduction, new StatsData(Owner, PlayerFields.PvpWaterElementReduction, 0));
            Fields.Add(PlayerFields.PvpAirElementReduction, new StatsData(Owner, PlayerFields.PvpAirElementReduction, 0));
            Fields.Add(PlayerFields.PvpFireElementReduction, new StatsData(Owner, PlayerFields.PvpFireElementReduction, 0));
            Fields.Add(PlayerFields.GlobalDamageReduction, new StatsData(Owner, PlayerFields.GlobalDamageReduction, 0));
            Fields.Add(PlayerFields.DamageMultiplicator, new StatsData(Owner, PlayerFields.DamageMultiplicator, 0));
            Fields.Add(PlayerFields.PhysicalDamage, new StatsData(Owner, PlayerFields.PhysicalDamage, 0));
            Fields.Add(PlayerFields.MagicDamage, new StatsData(Owner, PlayerFields.MagicDamage, 0));
            Fields.Add(PlayerFields.PhysicalDamageReduction, new StatsData(Owner, PlayerFields.PhysicalDamageReduction, 0));
            Fields.Add(PlayerFields.MagicDamageReduction, new StatsData(Owner, PlayerFields.MagicDamageReduction, 0));
            // custom fields

            Fields.Add(PlayerFields.WaterDamageArmor, new StatsData(Owner, PlayerFields.WaterDamageArmor, 0));
            Fields.Add(PlayerFields.EarthDamageArmor, new StatsData(Owner, PlayerFields.EarthDamageArmor, 0));
            Fields.Add(PlayerFields.NeutralDamageArmor, new StatsData(Owner, PlayerFields.NeutralDamageArmor, 0));
            Fields.Add(PlayerFields.AirDamageArmor, new StatsData(Owner, PlayerFields.AirDamageArmor, 0));
            Fields.Add(PlayerFields.FireDamageArmor, new StatsData(Owner, PlayerFields.FireDamageArmor, 0));
            Fields.Add(PlayerFields.Erosion, new StatsData(Owner, PlayerFields.Erosion, 10));
            Fields.Add(PlayerFields.ComboBonus, new StatsData(Owner, PlayerFields.ComboBonus, 0));
            Fields.Add(PlayerFields.Shield, new StatsData(Owner, PlayerFields.Shield, 0));
        }

        public StatsFields CloneAndChangeOwner(IStatsOwner owner)
        {
            var fields = new StatsFields(owner)
            {
                Fields = Fields.ToDictionary(x => x.Key, x => x.Value.CloneAndChangeOwner(owner))
            };

            return fields;
        }
    }
}