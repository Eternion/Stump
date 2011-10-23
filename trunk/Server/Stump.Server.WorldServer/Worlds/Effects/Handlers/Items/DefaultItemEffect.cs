using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Items
{
    [DefaultEffectHandler]
    public class DefaultItemEffect : ItemEffectHandler
    {
        #region Delegates

        public delegate void EffectComputeHandler(Character target, EffectInteger effect);

        #endregion

        #region Binds

        private static readonly Dictionary<CaracteristicsEnum, EffectComputeHandler> m_addMethods = new Dictionary
            <CaracteristicsEnum, EffectComputeHandler>
            {
                {CaracteristicsEnum.Health, AddHealth},
                {CaracteristicsEnum.Initiative, AddInitiative},
                {CaracteristicsEnum.Prospecting, AddProspecting},
                {CaracteristicsEnum.AP, AddAP},
                {CaracteristicsEnum.MP, AddMP},
                {CaracteristicsEnum.Strength, AddStrength},
                {CaracteristicsEnum.Vitality, AddVitality},
                {CaracteristicsEnum.Wisdom, AddWisdom},
                {CaracteristicsEnum.Chance, AddChance},
                {CaracteristicsEnum.Agility, AddAgility},
                {CaracteristicsEnum.Intelligence, AddIntelligence},
                {CaracteristicsEnum.Range, AddRange},
                {CaracteristicsEnum.SummonLimit, AddSummonLimit},
                {CaracteristicsEnum.DamageReflection, AddDamageReflection},
                {CaracteristicsEnum.CriticalHit, AddCriticalHit},
                {CaracteristicsEnum.CriticalMiss, AddCriticalMiss},
                {CaracteristicsEnum.HealBonus, AddHealBonus},
                {CaracteristicsEnum.DamageBonus, AddDamageBonus},
                {CaracteristicsEnum.WeaponDamageBonus, AddWeaponDamageBonus},
                {CaracteristicsEnum.DamageBonusPercent, AddDamageBonusPercent},
                {CaracteristicsEnum.TrapBonus, AddTrapBonus},
                {CaracteristicsEnum.TrapBonusPercent, AddTrapBonusPercent},
                {CaracteristicsEnum.PermanentDamagePercent, AddPermanentDamagePercent},
                {CaracteristicsEnum.TackleBlock, AddTackleBlock},
                {CaracteristicsEnum.TackleEvade, AddTackleEvade},
                {CaracteristicsEnum.APAttack, AddAPAttack},
                {CaracteristicsEnum.MPAttack, AddMPAttack},
                {CaracteristicsEnum.PushDamageBonus, AddPushDamageBonus},
                {CaracteristicsEnum.CriticalDamageBonus, AddCriticalDamageBonus},
                {CaracteristicsEnum.NeutralDamageBonus, AddNeutralDamageBonus},
                {CaracteristicsEnum.EarthDamageBonus, AddEarthDamageBonus},
                {CaracteristicsEnum.WaterDamageBonus, AddWaterDamageBonus},
                {CaracteristicsEnum.AirDamageBonus, AddAirDamageBonus},
                {CaracteristicsEnum.FireDamageBonus, AddFireDamageBonus},
                {CaracteristicsEnum.DodgeAPProbability, AddDodgeAPProbability},
                {CaracteristicsEnum.DodgeMPProbability, AddDodgeMPProbability},
                {CaracteristicsEnum.NeutralResistPercent, AddNeutralResistPercent},
                {CaracteristicsEnum.EarthResistPercent, AddEarthResistPercent},
                {CaracteristicsEnum.WaterResistPercent, AddWaterResistPercent},
                {CaracteristicsEnum.AirResistPercent, AddAirResistPercent},
                {CaracteristicsEnum.FireResistPercent, AddFireResistPercent},
                {CaracteristicsEnum.NeutralElementReduction, AddNeutralElementReduction},
                {CaracteristicsEnum.EarthElementReduction, AddEarthElementReduction},
                {CaracteristicsEnum.WaterElementReduction, AddWaterElementReduction},
                {CaracteristicsEnum.AirElementReduction, AddAirElementReduction},
                {CaracteristicsEnum.FireElementReduction, AddFireElementReduction},
                {CaracteristicsEnum.PushDamageReduction, AddPushDamageReduction},
                {CaracteristicsEnum.CriticalDamageReduction, AddCriticalDamageReduction},
                {CaracteristicsEnum.PvpNeutralResistPercent, AddPvpNeutralResistPercent},
                {CaracteristicsEnum.PvpEarthResistPercent, AddPvpEarthResistPercent},
                {CaracteristicsEnum.PvpWaterResistPercent, AddPvpWaterResistPercent},
                {CaracteristicsEnum.PvpAirResistPercent, AddPvpAirResistPercent},
                {CaracteristicsEnum.PvpFireResistPercent, AddPvpFireResistPercent},
                {CaracteristicsEnum.PvpNeutralElementReduction, AddPvpNeutralElementReduction},
                {CaracteristicsEnum.PvpEarthElementReduction, AddPvpEarthElementReduction},
                {CaracteristicsEnum.PvpWaterElementReduction, AddPvpWaterElementReduction},
                {CaracteristicsEnum.PvpAirElementReduction, AddPvpAirElementReduction},
                {CaracteristicsEnum.PvpFireElementReduction, AddPvpFireElementReduction},
                {CaracteristicsEnum.GlobalDamageReduction, AddGlobalDamageReduction},
                {CaracteristicsEnum.DamageMultiplicator, AddDamageMultiplicator},
                {CaracteristicsEnum.PhysicalDamage, AddPhysicalDamage},
                {CaracteristicsEnum.MagicDamage, AddMagicDamage},
                {CaracteristicsEnum.PhysicalDamageReduction, AddPhysicalDamageReduction},
                {CaracteristicsEnum.MagicDamageReduction, AddMagicDamageReduction},
            };

        private static readonly Dictionary<CaracteristicsEnum, EffectComputeHandler> m_subMethods = new Dictionary
            <CaracteristicsEnum, EffectComputeHandler>
            {
                {CaracteristicsEnum.Health, SubHealth},
                {CaracteristicsEnum.Initiative, SubInitiative},
                {CaracteristicsEnum.Prospecting, SubProspecting},
                {CaracteristicsEnum.AP, SubAP},
                {CaracteristicsEnum.MP, SubMP},
                {CaracteristicsEnum.Strength, SubStrength},
                {CaracteristicsEnum.Vitality, SubVitality},
                {CaracteristicsEnum.Wisdom, SubWisdom},
                {CaracteristicsEnum.Chance, SubChance},
                {CaracteristicsEnum.Agility, SubAgility},
                {CaracteristicsEnum.Intelligence, SubIntelligence},
                {CaracteristicsEnum.Range, SubRange},
                {CaracteristicsEnum.SummonLimit, SubSummonLimit},
                {CaracteristicsEnum.DamageReflection, SubDamageReflection},
                {CaracteristicsEnum.CriticalHit, SubCriticalHit},
                {CaracteristicsEnum.CriticalMiss, SubCriticalMiss},
                {CaracteristicsEnum.HealBonus, SubHealBonus},
                {CaracteristicsEnum.DamageBonus, SubDamageBonus},
                {CaracteristicsEnum.WeaponDamageBonus, SubWeaponDamageBonus},
                {CaracteristicsEnum.DamageBonusPercent, SubDamageBonusPercent},
                {CaracteristicsEnum.TrapBonus, SubTrapBonus},
                {CaracteristicsEnum.TrapBonusPercent, SubTrapBonusPercent},
                {CaracteristicsEnum.PermanentDamagePercent, SubPermanentDamagePercent},
                {CaracteristicsEnum.TackleBlock, SubTackleBlock},
                {CaracteristicsEnum.TackleEvade, SubTackleEvade},
                {CaracteristicsEnum.APAttack, SubAPAttack},
                {CaracteristicsEnum.MPAttack, SubMPAttack},
                {CaracteristicsEnum.PushDamageBonus, SubPushDamageBonus},
                {CaracteristicsEnum.CriticalDamageBonus, SubCriticalDamageBonus},
                {CaracteristicsEnum.NeutralDamageBonus, SubNeutralDamageBonus},
                {CaracteristicsEnum.EarthDamageBonus, SubEarthDamageBonus},
                {CaracteristicsEnum.WaterDamageBonus, SubWaterDamageBonus},
                {CaracteristicsEnum.AirDamageBonus, SubAirDamageBonus},
                {CaracteristicsEnum.FireDamageBonus, SubFireDamageBonus},
                {CaracteristicsEnum.DodgeAPProbability, SubDodgeAPProbability},
                {CaracteristicsEnum.DodgeMPProbability, SubDodgeMPProbability},
                {CaracteristicsEnum.NeutralResistPercent, SubNeutralResistPercent},
                {CaracteristicsEnum.EarthResistPercent, SubEarthResistPercent},
                {CaracteristicsEnum.WaterResistPercent, SubWaterResistPercent},
                {CaracteristicsEnum.AirResistPercent, SubAirResistPercent},
                {CaracteristicsEnum.FireResistPercent, SubFireResistPercent},
                {CaracteristicsEnum.NeutralElementReduction, SubNeutralElementReduction},
                {CaracteristicsEnum.EarthElementReduction, SubEarthElementReduction},
                {CaracteristicsEnum.WaterElementReduction, SubWaterElementReduction},
                {CaracteristicsEnum.AirElementReduction, SubAirElementReduction},
                {CaracteristicsEnum.FireElementReduction, SubFireElementReduction},
                {CaracteristicsEnum.PushDamageReduction, SubPushDamageReduction},
                {CaracteristicsEnum.CriticalDamageReduction, SubCriticalDamageReduction},
                {CaracteristicsEnum.PvpNeutralResistPercent, SubPvpNeutralResistPercent},
                {CaracteristicsEnum.PvpEarthResistPercent, SubPvpEarthResistPercent},
                {CaracteristicsEnum.PvpWaterResistPercent, SubPvpWaterResistPercent},
                {CaracteristicsEnum.PvpAirResistPercent, SubPvpAirResistPercent},
                {CaracteristicsEnum.PvpFireResistPercent, SubPvpFireResistPercent},
                {CaracteristicsEnum.PvpNeutralElementReduction, SubPvpNeutralElementReduction},
                {CaracteristicsEnum.PvpEarthElementReduction, SubPvpEarthElementReduction},
                {CaracteristicsEnum.PvpWaterElementReduction, SubPvpWaterElementReduction},
                {CaracteristicsEnum.PvpAirElementReduction, SubPvpAirElementReduction},
                {CaracteristicsEnum.PvpFireElementReduction, SubPvpFireElementReduction},
                {CaracteristicsEnum.GlobalDamageReduction, SubGlobalDamageReduction},
                {CaracteristicsEnum.DamageMultiplicator, SubDamageMultiplicator},
                {CaracteristicsEnum.PhysicalDamage, SubPhysicalDamage},
                {CaracteristicsEnum.MagicDamage, SubMagicDamage},
                {CaracteristicsEnum.PhysicalDamageReduction, SubPhysicalDamageReduction},
                {CaracteristicsEnum.MagicDamageReduction, SubMagicDamageReduction},
            };


        private static readonly Dictionary<EffectsEnum, CaracteristicsEnum> m_addEffectsBinds =
            new Dictionary<EffectsEnum, CaracteristicsEnum>
                {
                    {EffectsEnum.Effect_AddHealth, CaracteristicsEnum.Health},
                    {EffectsEnum.Effect_AddInitiative, CaracteristicsEnum.Initiative},
                    {EffectsEnum.Effect_AddProspecting, CaracteristicsEnum.Prospecting},
                    {EffectsEnum.Effect_AddAP_111, CaracteristicsEnum.AP},
                    {EffectsEnum.Effect_RegainAP, CaracteristicsEnum.AP},
                    {EffectsEnum.Effect_AddMP, CaracteristicsEnum.MP},
                    {EffectsEnum.Effect_AddMP_128, CaracteristicsEnum.MP},
                    {EffectsEnum.Effect_AddStrength, CaracteristicsEnum.Strength},
                    {EffectsEnum.Effect_AddVitality, CaracteristicsEnum.Vitality},
                    {EffectsEnum.Effect_AddWisdom, CaracteristicsEnum.Wisdom},
                    {EffectsEnum.Effect_AddChance, CaracteristicsEnum.Chance},
                    {EffectsEnum.Effect_AddAgility, CaracteristicsEnum.Agility},
                    {EffectsEnum.Effect_AddIntelligence, CaracteristicsEnum.Intelligence},
                    {EffectsEnum.Effect_AddRange, CaracteristicsEnum.Range},
                    {EffectsEnum.Effect_AddSummonLimit, CaracteristicsEnum.SummonLimit},
                    {EffectsEnum.Effect_AddDamageReflection, CaracteristicsEnum.DamageReflection},
                    {EffectsEnum.Effect_AddCriticalHit, CaracteristicsEnum.CriticalHit},
                    {EffectsEnum.Effect_AddCriticalMiss, CaracteristicsEnum.CriticalMiss},
                    {EffectsEnum.Effect_AddHealBonus, CaracteristicsEnum.HealBonus},
                    {EffectsEnum.Effect_AddDamageBonus, CaracteristicsEnum.DamageBonus},
                    {EffectsEnum.Effect_AddDamageBonusPercent, CaracteristicsEnum.DamageBonusPercent},
                    {EffectsEnum.Effect_AddTrapBonus, CaracteristicsEnum.TrapBonus},
                    {EffectsEnum.Effect_AddTrapBonusPercent, CaracteristicsEnum.TrapBonusPercent},
                    //{EffectsEnum.Effect_AddTackleBlock,CaracteristicsEnum.TackleBlock},
                    //{EffectsEnum.Effect_AddTackleEvade,CaracteristicsEnum.TackleEvade},
                    //{EffectsEnum.Effect_AddAPAttack,CaracteristicsEnum.APAttack},
                    //{EffectsEnum.Effect_AddMPAttack,CaracteristicsEnum.MPAttack},
                    //{EffectsEnum.Effect_AddPushDamageBonus,CaracteristicsEnum.PushDamageBonus},
                    {EffectsEnum.Effect_AddCriticalDamageBonus, CaracteristicsEnum.CriticalDamageBonus},
                    {EffectsEnum.Effect_AddNeutralDamageBonus, CaracteristicsEnum.NeutralDamageBonus},
                    {EffectsEnum.Effect_AddEarthDamageBonus, CaracteristicsEnum.EarthDamageBonus},
                    {EffectsEnum.Effect_AddWaterDamageBonus, CaracteristicsEnum.WaterDamageBonus},
                    {EffectsEnum.Effect_AddAirDamageBonus, CaracteristicsEnum.AirDamageBonus},
                    {EffectsEnum.Effect_AddFireDamageBonus, CaracteristicsEnum.FireDamageBonus},
                    //{EffectsEnum.Effect_AddDodgeAPProbability,CaracteristicsEnum.DodgeAPProbability},
                    //{EffectsEnum.Effect_AddDodgeMPProbability,CaracteristicsEnum.DodgeMPProbability},
                    {EffectsEnum.Effect_AddNeutralResistPercent, CaracteristicsEnum.NeutralResistPercent},
                    {EffectsEnum.Effect_AddEarthResistPercent, CaracteristicsEnum.EarthResistPercent},
                    {EffectsEnum.Effect_AddWaterResistPercent, CaracteristicsEnum.WaterResistPercent},
                    {EffectsEnum.Effect_AddAirResistPercent, CaracteristicsEnum.AirResistPercent},
                    {EffectsEnum.Effect_AddFireResistPercent, CaracteristicsEnum.FireResistPercent},
                    {EffectsEnum.Effect_AddNeutralElementReduction, CaracteristicsEnum.NeutralElementReduction},
                    {EffectsEnum.Effect_AddEarthElementReduction, CaracteristicsEnum.EarthElementReduction},
                    {EffectsEnum.Effect_AddWaterElementReduction, CaracteristicsEnum.WaterElementReduction},
                    {EffectsEnum.Effect_AddAirElementReduction, CaracteristicsEnum.AirElementReduction},
                    {EffectsEnum.Effect_AddFireElementReduction, CaracteristicsEnum.FireElementReduction},
                    {EffectsEnum.Effect_AddPushDamageReduction, CaracteristicsEnum.PushDamageReduction},
                    {EffectsEnum.Effect_AddCriticalDamageReduction, CaracteristicsEnum.CriticalDamageReduction},
                    {EffectsEnum.Effect_AddPvpNeutralResistPercent, CaracteristicsEnum.PvpNeutralResistPercent},
                    {EffectsEnum.Effect_AddPvpEarthResistPercent, CaracteristicsEnum.PvpEarthResistPercent},
                    {EffectsEnum.Effect_AddPvpWaterResistPercent, CaracteristicsEnum.PvpWaterResistPercent},
                    {EffectsEnum.Effect_AddPvpAirResistPercent, CaracteristicsEnum.PvpAirResistPercent},
                    {EffectsEnum.Effect_AddPvpFireResistPercent, CaracteristicsEnum.PvpFireResistPercent},
                    {EffectsEnum.Effect_AddPvpNeutralElementReduction, CaracteristicsEnum.PvpNeutralElementReduction},
                    {EffectsEnum.Effect_AddPvpEarthElementReduction, CaracteristicsEnum.PvpEarthElementReduction},
                    {EffectsEnum.Effect_AddPvpWaterElementReduction, CaracteristicsEnum.PvpWaterElementReduction},
                    {EffectsEnum.Effect_AddPvpAirElementReduction, CaracteristicsEnum.PvpAirElementReduction},
                    {EffectsEnum.Effect_AddPvpFireElementReduction, CaracteristicsEnum.PvpFireElementReduction},
                    {EffectsEnum.Effect_AddGlobalDamageReduction, CaracteristicsEnum.GlobalDamageReduction},
                    {EffectsEnum.Effect_AddDamageMultiplicator, CaracteristicsEnum.DamageMultiplicator},
                    {EffectsEnum.Effect_AddPhysicalDamage_137, CaracteristicsEnum.PhysicalDamage},
                    {EffectsEnum.Effect_AddPhysicalDamage_142, CaracteristicsEnum.PhysicalDamage},
                    //{EffectsEnum.Effect_AddMagicDamage,CaracteristicsEnum.MagicDamage},
                    {EffectsEnum.Effect_AddPhysicalDamageReduction, CaracteristicsEnum.PhysicalDamageReduction},
                    {EffectsEnum.Effect_AddMagicDamageReduction, CaracteristicsEnum.MagicDamageReduction},
                };

        private static readonly Dictionary<EffectsEnum, CaracteristicsEnum> m_subEffectsBinds =
            new Dictionary<EffectsEnum, CaracteristicsEnum>
                {
                    //EffectsEnum.Effect_SubHealth,CaracteristicsEnum.Health},
                    {EffectsEnum.Effect_SubInitiative, CaracteristicsEnum.Initiative},
                    {EffectsEnum.Effect_SubProspecting, CaracteristicsEnum.Prospecting},
                    {EffectsEnum.Effect_SubAP, CaracteristicsEnum.AP},
                    {EffectsEnum.Effect_SubMP, CaracteristicsEnum.MP},
                    {EffectsEnum.Effect_SubStrength, CaracteristicsEnum.Strength},
                    {EffectsEnum.Effect_SubVitality, CaracteristicsEnum.Vitality},
                    {EffectsEnum.Effect_SubWisdom, CaracteristicsEnum.Wisdom},
                    {EffectsEnum.Effect_SubChance, CaracteristicsEnum.Chance},
                    {EffectsEnum.Effect_SubAgility, CaracteristicsEnum.Agility},
                    {EffectsEnum.Effect_SubIntelligence, CaracteristicsEnum.Intelligence},
                    {EffectsEnum.Effect_SubRange, CaracteristicsEnum.Range},
                    //{EffectsEnum.Effect_SubSummonLimit,CaracteristicsEnum.SummonLimit},
                    //{EffectsEnum.Effect_SubDamageReflection,CaracteristicsEnum.DamageReflection},
                    {EffectsEnum.Effect_SubCriticalHit, CaracteristicsEnum.CriticalHit},
                    //{EffectsEnum.Effect_SubCriticalMiss,CaracteristicsEnum.CriticalMiss},
                    {EffectsEnum.Effect_SubHealBonus, CaracteristicsEnum.HealBonus},
                    {EffectsEnum.Effect_SubDamageBonus, CaracteristicsEnum.DamageBonus},
                    //{EffectsEnum.Effect_SubWeaponDamageBonus,CaracteristicsEnum.WeaponDamageBonus},
                    {EffectsEnum.Effect_SubDamageBonusPercent, CaracteristicsEnum.DamageBonusPercent},
                    //{EffectsEnum.Effect_SubTrapBonus,CaracteristicsEnum.TrapBonus},
                    //{EffectsEnum.Effect_SubTrapBonusPercent,CaracteristicsEnum.TrapBonusPercent},
                    //{EffectsEnum.Effect_SubPermanentDamagePercent,CaracteristicsEnum.PermanentDamagePercent},
                    //{EffectsEnum.Effect_SubTackleBlock,CaracteristicsEnum.TackleBlock},
                    //{EffectsEnum.Effect_SubTackleEvade,CaracteristicsEnum.TackleEvade},
                    //{EffectsEnum.Effect_SubAPAttack,CaracteristicsEnum.APAttack},
                    //{EffectsEnum.Effect_SubMPAttack,CaracteristicsEnum.MPAttack},
                    {EffectsEnum.Effect_SubPushDamageBonus, CaracteristicsEnum.PushDamageBonus},
                    {EffectsEnum.Effect_SubCriticalDamageBonus, CaracteristicsEnum.CriticalDamageBonus},
                    {EffectsEnum.Effect_SubNeutralDamageBonus, CaracteristicsEnum.NeutralDamageBonus},
                    {EffectsEnum.Effect_SubEarthDamageBonus, CaracteristicsEnum.EarthDamageBonus},
                    {EffectsEnum.Effect_SubWaterDamageBonus, CaracteristicsEnum.WaterDamageBonus},
                    {EffectsEnum.Effect_SubAirDamageBonus, CaracteristicsEnum.AirDamageBonus},
                    {EffectsEnum.Effect_SubFireDamageBonus, CaracteristicsEnum.FireDamageBonus},
                    {EffectsEnum.Effect_SubDodgeAPProbability, CaracteristicsEnum.DodgeAPProbability},
                    {EffectsEnum.Effect_SubDodgeMPProbability, CaracteristicsEnum.DodgeMPProbability},
                    {EffectsEnum.Effect_SubNeutralResistPercent, CaracteristicsEnum.NeutralResistPercent},
                    {EffectsEnum.Effect_SubEarthResistPercent, CaracteristicsEnum.EarthResistPercent},
                    {EffectsEnum.Effect_SubWaterResistPercent, CaracteristicsEnum.WaterResistPercent},
                    {EffectsEnum.Effect_SubAirResistPercent, CaracteristicsEnum.AirResistPercent},
                    {EffectsEnum.Effect_SubFireResistPercent, CaracteristicsEnum.FireResistPercent},
                    {EffectsEnum.Effect_SubNeutralElementReduction, CaracteristicsEnum.NeutralElementReduction},
                    {EffectsEnum.Effect_SubEarthElementReduction, CaracteristicsEnum.EarthElementReduction},
                    {EffectsEnum.Effect_SubWaterElementReduction, CaracteristicsEnum.WaterElementReduction},
                    {EffectsEnum.Effect_SubAirElementReduction, CaracteristicsEnum.AirElementReduction},
                    {EffectsEnum.Effect_SubFireElementReduction, CaracteristicsEnum.FireElementReduction},
                    {EffectsEnum.Effect_SubPushDamageReduction, CaracteristicsEnum.PushDamageReduction},
                    {EffectsEnum.Effect_SubCriticalDamageReduction, CaracteristicsEnum.CriticalDamageReduction},
                    {EffectsEnum.Effect_SubPvpNeutralResistPercent, CaracteristicsEnum.PvpNeutralResistPercent},
                    {EffectsEnum.Effect_SubPvpEarthResistPercent, CaracteristicsEnum.PvpEarthResistPercent},
                    {EffectsEnum.Effect_SubPvpWaterResistPercent, CaracteristicsEnum.PvpWaterResistPercent},
                    {EffectsEnum.Effect_SubPvpAirResistPercent, CaracteristicsEnum.PvpAirResistPercent},
                    {EffectsEnum.Effect_SubPvpFireResistPercent, CaracteristicsEnum.PvpFireResistPercent},
                    //{EffectsEnum.Effect_SubPvpNeutralElementReduction, CaracteristicsEnum.PvpNeutralElementReduction},
                    //{EffectsEnum.Effect_SubPvpEarthElementReduction, CaracteristicsEnum.PvpEarthElementReduction},
                    //{EffectsEnum.Effect_SubPvpWaterElementReduction, CaracteristicsEnum.PvpWaterElementReduction},
                    //{EffectsEnum.Effect_SubPvpAirElementReduction, CaracteristicsEnum.PvpAirElementReduction},
                    //{EffectsEnum.Effect_SubPvpFireElementReduction, CaracteristicsEnum.PvpFireElementReduction},
                    //{EffectsEnum.Effect_SubGlobalDamageReduction, CaracteristicsEnum.GlobalDamageReduction},
                    //{EffectsEnum.Effect_SubDamageMultiplicator, CaracteristicsEnum.DamageMultiplicator},
                    //{EffectsEnum.Effect_SubPhysicalDamage, CaracteristicsEnum.PhysicalDamage},
                    //{EffectsEnum.Effect_SubMagicDamage, CaracteristicsEnum.MagicDamage},
                    {EffectsEnum.Effect_SubPhysicalDamageReduction, CaracteristicsEnum.PhysicalDamageReduction},
                    {EffectsEnum.Effect_SubMagicDamageReduction, CaracteristicsEnum.MagicDamageReduction},
                };

        #endregion

        public DefaultItemEffect(EffectBase effect, Character target, Item item)
            : base(effect, target, item)
        {
        }

        public override void Apply()
        {
            if (!(Effect is EffectInteger))
                return;

            EffectComputeHandler handler = null;

            if (m_addEffectsBinds.ContainsKey(Effect.EffectId))
            {
                var caracteritic = m_addEffectsBinds[Effect.EffectId];

                handler = Equiped ? m_addMethods[caracteritic] : m_subMethods[caracteritic];
            }
            else if (m_subEffectsBinds.ContainsKey(Effect.EffectId))
            {
                var caracteritic = m_addEffectsBinds[Effect.EffectId];

                handler = Equiped ? m_subMethods[caracteritic] : m_addMethods[caracteritic];
            }

            if (handler != null)
                handler(Target, Effect as EffectInteger);
        }

        #region Add Methods

        private static void AddHealth(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Health].Equiped += effect.Value;
        }

        private static void AddInitiative(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Initiative].Equiped += effect.Value;
        }

        private static void AddProspecting(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Prospecting].Equiped += effect.Value;
        }

        private static void AddAP(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AP].Equiped += effect.Value;
        }

        private static void AddMP(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MP].Equiped += effect.Value;
        }

        private static void AddStrength(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Strength].Equiped += effect.Value;
        }

        private static void AddVitality(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Vitality].Equiped += effect.Value;
        }

        private static void AddWisdom(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Wisdom].Equiped += effect.Value;
        }

        private static void AddChance(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Chance].Equiped += effect.Value;
        }

        private static void AddAgility(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Agility].Equiped += effect.Value;
        }

        private static void AddIntelligence(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Intelligence].Equiped += effect.Value;
        }

        private static void AddRange(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Range].Equiped += effect.Value;
        }

        private static void AddSummonLimit(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.SummonLimit].Equiped += effect.Value;
        }

        private static void AddDamageReflection(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageReflection].Equiped += effect.Value;
        }

        private static void AddCriticalHit(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalHit].Equiped += effect.Value;
        }

        private static void AddCriticalMiss(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalMiss].Equiped += effect.Value;
        }

        private static void AddHealBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.HealBonus].Equiped += effect.Value;
        }

        private static void AddDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageBonus].Equiped += effect.Value;
        }

        private static void AddWeaponDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WeaponDamageBonus].Equiped += effect.Value;
        }

        private static void AddDamageBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageBonusPercent].Equiped += effect.Value;
        }

        private static void AddTrapBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TrapBonus].Equiped += effect.Value;
        }

        private static void AddTrapBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TrapBonusPercent].Equiped += effect.Value;
        }

        private static void AddPermanentDamagePercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PermanentDamagePercent].Equiped += effect.Value;
        }

        private static void AddTackleBlock(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TackleBlock].Equiped += effect.Value;
        }

        private static void AddTackleEvade(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TackleEvade].Equiped += effect.Value;
        }

        private static void AddAPAttack(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.APAttack].Equiped += effect.Value;
        }

        private static void AddMPAttack(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MPAttack].Equiped += effect.Value;
        }

        private static void AddPushDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PushDamageBonus].Equiped += effect.Value;
        }

        private static void AddCriticalDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalDamageBonus].Equiped += effect.Value;
        }

        private static void AddNeutralDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralDamageBonus].Equiped += effect.Value;
        }

        private static void AddEarthDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthDamageBonus].Equiped += effect.Value;
        }

        private static void AddWaterDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterDamageBonus].Equiped += effect.Value;
        }

        private static void AddAirDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirDamageBonus].Equiped += effect.Value;
        }

        private static void AddFireDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireDamageBonus].Equiped += effect.Value;
        }

        private static void AddDodgeAPProbability(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DodgeAPProbability].Equiped += effect.Value;
        }

        private static void AddDodgeMPProbability(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DodgeMPProbability].Equiped += effect.Value;
        }

        private static void AddNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralResistPercent].Equiped += effect.Value;
        }

        private static void AddEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthResistPercent].Equiped += effect.Value;
        }

        private static void AddWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterResistPercent].Equiped += effect.Value;
        }

        private static void AddAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirResistPercent].Equiped += effect.Value;
        }

        private static void AddFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireResistPercent].Equiped += effect.Value;
        }

        private static void AddNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralElementReduction].Equiped += effect.Value;
        }

        private static void AddEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthElementReduction].Equiped += effect.Value;
        }

        private static void AddWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterElementReduction].Equiped += effect.Value;
        }

        private static void AddAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirElementReduction].Equiped += effect.Value;
        }

        private static void AddFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireElementReduction].Equiped += effect.Value;
        }

        private static void AddPushDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PushDamageReduction].Equiped += effect.Value;
        }

        private static void AddCriticalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalDamageReduction].Equiped += effect.Value;
        }

        private static void AddPvpNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpNeutralResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpEarthResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpWaterResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpAirResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpFireResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpNeutralElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpEarthElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpWaterElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpAirElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpFireElementReduction].Equiped += effect.Value;
        }

        private static void AddGlobalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.GlobalDamageReduction].Equiped += effect.Value;
        }

        private static void AddDamageMultiplicator(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageMultiplicator].Equiped += effect.Value;
        }

        private static void AddPhysicalDamage(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PhysicalDamage].Equiped += effect.Value;
        }

        private static void AddMagicDamage(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MagicDamage].Equiped += effect.Value;
        }

        private static void AddPhysicalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PhysicalDamageReduction].Equiped += effect.Value;
        }

        private static void AddMagicDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MagicDamageReduction].Equiped += effect.Value;
        }

        #endregion

        #region Sub Methods

        private static void SubHealth(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Health].Equiped -= effect.Value;
        }

        private static void SubInitiative(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Initiative].Equiped -= effect.Value;
        }

        private static void SubProspecting(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Prospecting].Equiped -= effect.Value;
        }

        private static void SubAP(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AP].Equiped -= effect.Value;
        }

        private static void SubMP(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MP].Equiped -= effect.Value;
        }

        private static void SubStrength(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Strength].Equiped -= effect.Value;
        }

        private static void SubVitality(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Vitality].Equiped -= effect.Value;
        }

        private static void SubWisdom(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Wisdom].Equiped -= effect.Value;
        }

        private static void SubChance(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Chance].Equiped -= effect.Value;
        }

        private static void SubAgility(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Agility].Equiped -= effect.Value;
        }

        private static void SubIntelligence(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Intelligence].Equiped -= effect.Value;
        }

        private static void SubRange(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.Range].Equiped -= effect.Value;
        }

        private static void SubSummonLimit(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.SummonLimit].Equiped -= effect.Value;
        }

        private static void SubDamageReflection(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageReflection].Equiped -= effect.Value;
        }

        private static void SubCriticalHit(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalHit].Equiped -= effect.Value;
        }

        private static void SubCriticalMiss(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalMiss].Equiped -= effect.Value;
        }

        private static void SubHealBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.HealBonus].Equiped -= effect.Value;
        }

        private static void SubDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageBonus].Equiped -= effect.Value;
        }

        private static void SubWeaponDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WeaponDamageBonus].Equiped -= effect.Value;
        }

        private static void SubDamageBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageBonusPercent].Equiped -= effect.Value;
        }

        private static void SubTrapBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TrapBonus].Equiped -= effect.Value;
        }

        private static void SubTrapBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TrapBonusPercent].Equiped -= effect.Value;
        }

        private static void SubPermanentDamagePercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PermanentDamagePercent].Equiped -= effect.Value;
        }

        private static void SubTackleBlock(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TackleBlock].Equiped -= effect.Value;
        }

        private static void SubTackleEvade(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.TackleEvade].Equiped -= effect.Value;
        }

        private static void SubAPAttack(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.APAttack].Equiped -= effect.Value;
        }

        private static void SubMPAttack(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MPAttack].Equiped -= effect.Value;
        }

        private static void SubPushDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PushDamageBonus].Equiped -= effect.Value;
        }

        private static void SubCriticalDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalDamageBonus].Equiped -= effect.Value;
        }

        private static void SubNeutralDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralDamageBonus].Equiped -= effect.Value;
        }

        private static void SubEarthDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthDamageBonus].Equiped -= effect.Value;
        }

        private static void SubWaterDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterDamageBonus].Equiped -= effect.Value;
        }

        private static void SubAirDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirDamageBonus].Equiped -= effect.Value;
        }

        private static void SubFireDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireDamageBonus].Equiped -= effect.Value;
        }

        private static void SubDodgeAPProbability(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DodgeAPProbability].Equiped -= effect.Value;
        }

        private static void SubDodgeMPProbability(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DodgeMPProbability].Equiped -= effect.Value;
        }

        private static void SubNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralResistPercent].Equiped -= effect.Value;
        }

        private static void SubEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthResistPercent].Equiped -= effect.Value;
        }

        private static void SubWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterResistPercent].Equiped -= effect.Value;
        }

        private static void SubAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirResistPercent].Equiped -= effect.Value;
        }

        private static void SubFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireResistPercent].Equiped -= effect.Value;
        }

        private static void SubNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.NeutralElementReduction].Equiped -= effect.Value;
        }

        private static void SubEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.EarthElementReduction].Equiped -= effect.Value;
        }

        private static void SubWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.WaterElementReduction].Equiped -= effect.Value;
        }

        private static void SubAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.AirElementReduction].Equiped -= effect.Value;
        }

        private static void SubFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.FireElementReduction].Equiped -= effect.Value;
        }

        private static void SubPushDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PushDamageReduction].Equiped -= effect.Value;
        }

        private static void SubCriticalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.CriticalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubPvpNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpNeutralResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpEarthResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpWaterResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpAirResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpFireResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpNeutralElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpEarthElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpWaterElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpAirElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PvpFireElementReduction].Equiped -= effect.Value;
        }

        private static void SubGlobalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.GlobalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubDamageMultiplicator(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.DamageMultiplicator].Equiped -= effect.Value;
        }

        private static void SubPhysicalDamage(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PhysicalDamage].Equiped -= effect.Value;
        }

        private static void SubMagicDamage(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MagicDamage].Equiped -= effect.Value;
        }

        private static void SubPhysicalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.PhysicalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubMagicDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[CaracteristicsEnum.MagicDamageReduction].Equiped -= effect.Value;
        }

        #endregion
    }
}