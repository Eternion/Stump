using System.Collections.Generic;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    [DefaultEffectHandler]
    public class DefaultItemEffect : ItemEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Delegates

        public delegate void EffectComputeHandler(Character target, EffectInteger effect);

        #endregion

        #region Binds

        private static readonly Dictionary<PlayerFields, EffectComputeHandler> m_addMethods = new Dictionary
            <PlayerFields, EffectComputeHandler>
            {
                {PlayerFields.Health, AddHealth},
                {PlayerFields.Initiative, AddInitiative},
                {PlayerFields.Prospecting, AddProspecting},
                {PlayerFields.AP, AddAP},
                {PlayerFields.MP, AddMP},
                {PlayerFields.Strength, AddStrength},
                {PlayerFields.Vitality, AddVitality},
                {PlayerFields.Wisdom, AddWisdom},
                {PlayerFields.Chance, AddChance},
                {PlayerFields.Agility, AddAgility},
                {PlayerFields.Intelligence, AddIntelligence},
                {PlayerFields.Range, AddRange},
                {PlayerFields.SummonLimit, AddSummonLimit},
                {PlayerFields.DamageReflection, AddDamageReflection},
                {PlayerFields.CriticalHit, AddCriticalHit},
                {PlayerFields.CriticalMiss, AddCriticalMiss},
                {PlayerFields.HealBonus, AddHealBonus},
                {PlayerFields.DamageBonus, AddDamageBonus},
                {PlayerFields.WeaponDamageBonus, AddWeaponDamageBonus},
                {PlayerFields.DamageBonusPercent, AddDamageBonusPercent},
                {PlayerFields.TrapBonus, AddTrapBonus},
                {PlayerFields.TrapBonusPercent, AddTrapBonusPercent},
                {PlayerFields.PermanentDamagePercent, AddPermanentDamagePercent},
                {PlayerFields.TackleBlock, AddTackleBlock},
                {PlayerFields.TackleEvade, AddTackleEvade},
                {PlayerFields.APAttack, AddAPAttack},
                {PlayerFields.MPAttack, AddMPAttack},
                {PlayerFields.PushDamageBonus, AddPushDamageBonus},
                {PlayerFields.CriticalDamageBonus, AddCriticalDamageBonus},
                {PlayerFields.NeutralDamageBonus, AddNeutralDamageBonus},
                {PlayerFields.EarthDamageBonus, AddEarthDamageBonus},
                {PlayerFields.WaterDamageBonus, AddWaterDamageBonus},
                {PlayerFields.AirDamageBonus, AddAirDamageBonus},
                {PlayerFields.FireDamageBonus, AddFireDamageBonus},
                {PlayerFields.DodgeAPProbability, AddDodgeAPProbability},
                {PlayerFields.DodgeMPProbability, AddDodgeMPProbability},
                {PlayerFields.NeutralResistPercent, AddNeutralResistPercent},
                {PlayerFields.EarthResistPercent, AddEarthResistPercent},
                {PlayerFields.WaterResistPercent, AddWaterResistPercent},
                {PlayerFields.AirResistPercent, AddAirResistPercent},
                {PlayerFields.FireResistPercent, AddFireResistPercent},
                {PlayerFields.NeutralElementReduction, AddNeutralElementReduction},
                {PlayerFields.EarthElementReduction, AddEarthElementReduction},
                {PlayerFields.WaterElementReduction, AddWaterElementReduction},
                {PlayerFields.AirElementReduction, AddAirElementReduction},
                {PlayerFields.FireElementReduction, AddFireElementReduction},
                {PlayerFields.PushDamageReduction, AddPushDamageReduction},
                {PlayerFields.CriticalDamageReduction, AddCriticalDamageReduction},
                {PlayerFields.PvpNeutralResistPercent, AddPvpNeutralResistPercent},
                {PlayerFields.PvpEarthResistPercent, AddPvpEarthResistPercent},
                {PlayerFields.PvpWaterResistPercent, AddPvpWaterResistPercent},
                {PlayerFields.PvpAirResistPercent, AddPvpAirResistPercent},
                {PlayerFields.PvpFireResistPercent, AddPvpFireResistPercent},
                {PlayerFields.PvpNeutralElementReduction, AddPvpNeutralElementReduction},
                {PlayerFields.PvpEarthElementReduction, AddPvpEarthElementReduction},
                {PlayerFields.PvpWaterElementReduction, AddPvpWaterElementReduction},
                {PlayerFields.PvpAirElementReduction, AddPvpAirElementReduction},
                {PlayerFields.PvpFireElementReduction, AddPvpFireElementReduction},
                {PlayerFields.GlobalDamageReduction, AddGlobalDamageReduction},
                {PlayerFields.DamageMultiplicator, AddDamageMultiplicator},
                {PlayerFields.PhysicalDamage, AddPhysicalDamage},
                {PlayerFields.MagicDamage, AddMagicDamage},
                {PlayerFields.PhysicalDamageReduction, AddPhysicalDamageReduction},
                {PlayerFields.MagicDamageReduction, AddMagicDamageReduction},
            };

        private static readonly Dictionary<PlayerFields, EffectComputeHandler> m_subMethods = new Dictionary
            <PlayerFields, EffectComputeHandler>
            {
                {PlayerFields.Health, SubHealth},
                {PlayerFields.Initiative, SubInitiative},
                {PlayerFields.Prospecting, SubProspecting},
                {PlayerFields.AP, SubAP},
                {PlayerFields.MP, SubMP},
                {PlayerFields.Strength, SubStrength},
                {PlayerFields.Vitality, SubVitality},
                {PlayerFields.Wisdom, SubWisdom},
                {PlayerFields.Chance, SubChance},
                {PlayerFields.Agility, SubAgility},
                {PlayerFields.Intelligence, SubIntelligence},
                {PlayerFields.Range, SubRange},
                {PlayerFields.SummonLimit, SubSummonLimit},
                {PlayerFields.DamageReflection, SubDamageReflection},
                {PlayerFields.CriticalHit, SubCriticalHit},
                {PlayerFields.CriticalMiss, SubCriticalMiss},
                {PlayerFields.HealBonus, SubHealBonus},
                {PlayerFields.DamageBonus, SubDamageBonus},
                {PlayerFields.WeaponDamageBonus, SubWeaponDamageBonus},
                {PlayerFields.DamageBonusPercent, SubDamageBonusPercent},
                {PlayerFields.TrapBonus, SubTrapBonus},
                {PlayerFields.TrapBonusPercent, SubTrapBonusPercent},
                {PlayerFields.PermanentDamagePercent, SubPermanentDamagePercent},
                {PlayerFields.TackleBlock, SubTackleBlock},
                {PlayerFields.TackleEvade, SubTackleEvade},
                {PlayerFields.APAttack, SubAPAttack},
                {PlayerFields.MPAttack, SubMPAttack},
                {PlayerFields.PushDamageBonus, SubPushDamageBonus},
                {PlayerFields.CriticalDamageBonus, SubCriticalDamageBonus},
                {PlayerFields.NeutralDamageBonus, SubNeutralDamageBonus},
                {PlayerFields.EarthDamageBonus, SubEarthDamageBonus},
                {PlayerFields.WaterDamageBonus, SubWaterDamageBonus},
                {PlayerFields.AirDamageBonus, SubAirDamageBonus},
                {PlayerFields.FireDamageBonus, SubFireDamageBonus},
                {PlayerFields.DodgeAPProbability, SubDodgeAPProbability},
                {PlayerFields.DodgeMPProbability, SubDodgeMPProbability},
                {PlayerFields.NeutralResistPercent, SubNeutralResistPercent},
                {PlayerFields.EarthResistPercent, SubEarthResistPercent},
                {PlayerFields.WaterResistPercent, SubWaterResistPercent},
                {PlayerFields.AirResistPercent, SubAirResistPercent},
                {PlayerFields.FireResistPercent, SubFireResistPercent},
                {PlayerFields.NeutralElementReduction, SubNeutralElementReduction},
                {PlayerFields.EarthElementReduction, SubEarthElementReduction},
                {PlayerFields.WaterElementReduction, SubWaterElementReduction},
                {PlayerFields.AirElementReduction, SubAirElementReduction},
                {PlayerFields.FireElementReduction, SubFireElementReduction},
                {PlayerFields.PushDamageReduction, SubPushDamageReduction},
                {PlayerFields.CriticalDamageReduction, SubCriticalDamageReduction},
                {PlayerFields.PvpNeutralResistPercent, SubPvpNeutralResistPercent},
                {PlayerFields.PvpEarthResistPercent, SubPvpEarthResistPercent},
                {PlayerFields.PvpWaterResistPercent, SubPvpWaterResistPercent},
                {PlayerFields.PvpAirResistPercent, SubPvpAirResistPercent},
                {PlayerFields.PvpFireResistPercent, SubPvpFireResistPercent},
                {PlayerFields.PvpNeutralElementReduction, SubPvpNeutralElementReduction},
                {PlayerFields.PvpEarthElementReduction, SubPvpEarthElementReduction},
                {PlayerFields.PvpWaterElementReduction, SubPvpWaterElementReduction},
                {PlayerFields.PvpAirElementReduction, SubPvpAirElementReduction},
                {PlayerFields.PvpFireElementReduction, SubPvpFireElementReduction},
                {PlayerFields.GlobalDamageReduction, SubGlobalDamageReduction},
                {PlayerFields.DamageMultiplicator, SubDamageMultiplicator},
                {PlayerFields.PhysicalDamage, SubPhysicalDamage},
                {PlayerFields.MagicDamage, SubMagicDamage},
                {PlayerFields.PhysicalDamageReduction, SubPhysicalDamageReduction},
                {PlayerFields.MagicDamageReduction, SubMagicDamageReduction},
            };


        private static readonly Dictionary<EffectsEnum, PlayerFields> m_addEffectsBinds =
            new Dictionary<EffectsEnum, PlayerFields>
                {
                    {EffectsEnum.Effect_AddHealth, PlayerFields.Health},
                    {EffectsEnum.Effect_AddInitiative, PlayerFields.Initiative},
                    {EffectsEnum.Effect_AddProspecting, PlayerFields.Prospecting},
                    {EffectsEnum.Effect_AddAP_111, PlayerFields.AP},
                    {EffectsEnum.Effect_RegainAP, PlayerFields.AP},
                    {EffectsEnum.Effect_AddMP, PlayerFields.MP},
                    {EffectsEnum.Effect_AddMP_128, PlayerFields.MP},
                    {EffectsEnum.Effect_AddStrength, PlayerFields.Strength},
                    {EffectsEnum.Effect_AddVitality, PlayerFields.Vitality},
                    {EffectsEnum.Effect_AddWisdom, PlayerFields.Wisdom},
                    {EffectsEnum.Effect_AddChance, PlayerFields.Chance},
                    {EffectsEnum.Effect_AddAgility, PlayerFields.Agility},
                    {EffectsEnum.Effect_AddIntelligence, PlayerFields.Intelligence},
                    {EffectsEnum.Effect_AddRange, PlayerFields.Range},
                    {EffectsEnum.Effect_AddSummonLimit, PlayerFields.SummonLimit},
                    {EffectsEnum.Effect_AddDamageReflection, PlayerFields.DamageReflection},
                    {EffectsEnum.Effect_AddCriticalHit, PlayerFields.CriticalHit},
                    {EffectsEnum.Effect_AddCriticalMiss, PlayerFields.CriticalMiss},
                    {EffectsEnum.Effect_AddHealBonus, PlayerFields.HealBonus},
                    {EffectsEnum.Effect_AddDamageBonus, PlayerFields.DamageBonus},
                    {EffectsEnum.Effect_IncreaseDamage_138, PlayerFields.DamageBonusPercent},
                    {EffectsEnum.Effect_AddDamageBonusPercent, PlayerFields.DamageBonusPercent},
                    {EffectsEnum.Effect_AddTrapBonus, PlayerFields.TrapBonus},
                    {EffectsEnum.Effect_AddTrapBonusPercent, PlayerFields.TrapBonusPercent},
                    //{EffectsEnum.Effect_AddTackleBlock,PlayerFields.TackleBlock},
                    //{EffectsEnum.Effect_AddTackleEvade,PlayerFields.TackleEvade},
                    //{EffectsEnum.Effect_AddAPAttack,PlayerFields.APAttack},
                    //{EffectsEnum.Effect_AddMPAttack,PlayerFields.MPAttack},
                    //{EffectsEnum.Effect_AddPushDamageBonus,PlayerFields.PushDamageBonus},
                    {EffectsEnum.Effect_AddCriticalDamageBonus, PlayerFields.CriticalDamageBonus},
                    {EffectsEnum.Effect_AddNeutralDamageBonus, PlayerFields.NeutralDamageBonus},
                    {EffectsEnum.Effect_AddEarthDamageBonus, PlayerFields.EarthDamageBonus},
                    {EffectsEnum.Effect_AddWaterDamageBonus, PlayerFields.WaterDamageBonus},
                    {EffectsEnum.Effect_AddAirDamageBonus, PlayerFields.AirDamageBonus},
                    {EffectsEnum.Effect_AddFireDamageBonus, PlayerFields.FireDamageBonus},
                    //{EffectsEnum.Effect_AddDodgeAPProbability,PlayerFields.DodgeAPProbability},
                    //{EffectsEnum.Effect_AddDodgeMPProbability,PlayerFields.DodgeMPProbability},
                    {EffectsEnum.Effect_AddNeutralResistPercent, PlayerFields.NeutralResistPercent},
                    {EffectsEnum.Effect_AddEarthResistPercent, PlayerFields.EarthResistPercent},
                    {EffectsEnum.Effect_AddWaterResistPercent, PlayerFields.WaterResistPercent},
                    {EffectsEnum.Effect_AddAirResistPercent, PlayerFields.AirResistPercent},
                    {EffectsEnum.Effect_AddFireResistPercent, PlayerFields.FireResistPercent},
                    {EffectsEnum.Effect_AddNeutralElementReduction, PlayerFields.NeutralElementReduction},
                    {EffectsEnum.Effect_AddEarthElementReduction, PlayerFields.EarthElementReduction},
                    {EffectsEnum.Effect_AddWaterElementReduction, PlayerFields.WaterElementReduction},
                    {EffectsEnum.Effect_AddAirElementReduction, PlayerFields.AirElementReduction},
                    {EffectsEnum.Effect_AddFireElementReduction, PlayerFields.FireElementReduction},
                    {EffectsEnum.Effect_AddPushDamageReduction, PlayerFields.PushDamageReduction},
                    {EffectsEnum.Effect_AddCriticalDamageReduction, PlayerFields.CriticalDamageReduction},
                    {EffectsEnum.Effect_AddPvpNeutralResistPercent, PlayerFields.PvpNeutralResistPercent},
                    {EffectsEnum.Effect_AddPvpEarthResistPercent, PlayerFields.PvpEarthResistPercent},
                    {EffectsEnum.Effect_AddPvpWaterResistPercent, PlayerFields.PvpWaterResistPercent},
                    {EffectsEnum.Effect_AddPvpAirResistPercent, PlayerFields.PvpAirResistPercent},
                    {EffectsEnum.Effect_AddPvpFireResistPercent, PlayerFields.PvpFireResistPercent},
                    {EffectsEnum.Effect_AddPvpNeutralElementReduction, PlayerFields.PvpNeutralElementReduction},
                    {EffectsEnum.Effect_AddPvpEarthElementReduction, PlayerFields.PvpEarthElementReduction},
                    {EffectsEnum.Effect_AddPvpWaterElementReduction, PlayerFields.PvpWaterElementReduction},
                    {EffectsEnum.Effect_AddPvpAirElementReduction, PlayerFields.PvpAirElementReduction},
                    {EffectsEnum.Effect_AddPvpFireElementReduction, PlayerFields.PvpFireElementReduction},
                    {EffectsEnum.Effect_AddGlobalDamageReduction, PlayerFields.GlobalDamageReduction},
                    {EffectsEnum.Effect_AddDamageMultiplicator, PlayerFields.DamageMultiplicator},
                    {EffectsEnum.Effect_AddPhysicalDamage_137, PlayerFields.PhysicalDamage},
                    {EffectsEnum.Effect_AddPhysicalDamage_142, PlayerFields.PhysicalDamage},
                    //{EffectsEnum.Effect_AddMagicDamage,PlayerFields.MagicDamage},
                    {EffectsEnum.Effect_AddPhysicalDamageReduction, PlayerFields.PhysicalDamageReduction},
                    {EffectsEnum.Effect_AddMagicDamageReduction, PlayerFields.MagicDamageReduction},
                };

        private static readonly Dictionary<EffectsEnum, PlayerFields> m_subEffectsBinds =
            new Dictionary<EffectsEnum, PlayerFields>
                {
                    //EffectsEnum.Effect_SubHealth,PlayerFields.Health},
                    {EffectsEnum.Effect_SubInitiative, PlayerFields.Initiative},
                    {EffectsEnum.Effect_SubProspecting, PlayerFields.Prospecting},
                    {EffectsEnum.Effect_SubAP, PlayerFields.AP},
                    {EffectsEnum.Effect_SubMP, PlayerFields.MP},
                    {EffectsEnum.Effect_SubStrength, PlayerFields.Strength},
                    {EffectsEnum.Effect_SubVitality, PlayerFields.Vitality},
                    {EffectsEnum.Effect_SubWisdom, PlayerFields.Wisdom},
                    {EffectsEnum.Effect_SubChance, PlayerFields.Chance},
                    {EffectsEnum.Effect_SubAgility, PlayerFields.Agility},
                    {EffectsEnum.Effect_SubIntelligence, PlayerFields.Intelligence},
                    {EffectsEnum.Effect_SubRange, PlayerFields.Range},
                    //{EffectsEnum.Effect_SubSummonLimit,PlayerFields.SummonLimit},
                    //{EffectsEnum.Effect_SubDamageReflection,PlayerFields.DamageReflection},
                    {EffectsEnum.Effect_SubCriticalHit, PlayerFields.CriticalHit},
                    //{EffectsEnum.Effect_SubCriticalMiss,PlayerFields.CriticalMiss},
                    {EffectsEnum.Effect_SubHealBonus, PlayerFields.HealBonus},
                    {EffectsEnum.Effect_SubDamageBonus, PlayerFields.DamageBonus},
                    //{EffectsEnum.Effect_SubWeaponDamageBonus,PlayerFields.WeaponDamageBonus},
                    {EffectsEnum.Effect_SubDamageBonusPercent, PlayerFields.DamageBonusPercent},
                    //{EffectsEnum.Effect_SubTrapBonus,PlayerFields.TrapBonus},
                    //{EffectsEnum.Effect_SubTrapBonusPercent,PlayerFields.TrapBonusPercent},
                    //{EffectsEnum.Effect_SubPermanentDamagePercent,PlayerFields.PermanentDamagePercent},
                    //{EffectsEnum.Effect_SubTackleBlock,PlayerFields.TackleBlock},
                    //{EffectsEnum.Effect_SubTackleEvade,PlayerFields.TackleEvade},
                    //{EffectsEnum.Effect_SubAPAttack,PlayerFields.APAttack},
                    //{EffectsEnum.Effect_SubMPAttack,PlayerFields.MPAttack},
                    {EffectsEnum.Effect_SubPushDamageBonus, PlayerFields.PushDamageBonus},
                    {EffectsEnum.Effect_SubCriticalDamageBonus, PlayerFields.CriticalDamageBonus},
                    {EffectsEnum.Effect_SubNeutralDamageBonus, PlayerFields.NeutralDamageBonus},
                    {EffectsEnum.Effect_SubEarthDamageBonus, PlayerFields.EarthDamageBonus},
                    {EffectsEnum.Effect_SubWaterDamageBonus, PlayerFields.WaterDamageBonus},
                    {EffectsEnum.Effect_SubAirDamageBonus, PlayerFields.AirDamageBonus},
                    {EffectsEnum.Effect_SubFireDamageBonus, PlayerFields.FireDamageBonus},
                    {EffectsEnum.Effect_SubDodgeAPProbability, PlayerFields.DodgeAPProbability},
                    {EffectsEnum.Effect_SubDodgeMPProbability, PlayerFields.DodgeMPProbability},
                    {EffectsEnum.Effect_SubNeutralResistPercent, PlayerFields.NeutralResistPercent},
                    {EffectsEnum.Effect_SubEarthResistPercent, PlayerFields.EarthResistPercent},
                    {EffectsEnum.Effect_SubWaterResistPercent, PlayerFields.WaterResistPercent},
                    {EffectsEnum.Effect_SubAirResistPercent, PlayerFields.AirResistPercent},
                    {EffectsEnum.Effect_SubFireResistPercent, PlayerFields.FireResistPercent},
                    {EffectsEnum.Effect_SubNeutralElementReduction, PlayerFields.NeutralElementReduction},
                    {EffectsEnum.Effect_SubEarthElementReduction, PlayerFields.EarthElementReduction},
                    {EffectsEnum.Effect_SubWaterElementReduction, PlayerFields.WaterElementReduction},
                    {EffectsEnum.Effect_SubAirElementReduction, PlayerFields.AirElementReduction},
                    {EffectsEnum.Effect_SubFireElementReduction, PlayerFields.FireElementReduction},
                    {EffectsEnum.Effect_SubPushDamageReduction, PlayerFields.PushDamageReduction},
                    {EffectsEnum.Effect_SubCriticalDamageReduction, PlayerFields.CriticalDamageReduction},
                    {EffectsEnum.Effect_SubPvpNeutralResistPercent, PlayerFields.PvpNeutralResistPercent},
                    {EffectsEnum.Effect_SubPvpEarthResistPercent, PlayerFields.PvpEarthResistPercent},
                    {EffectsEnum.Effect_SubPvpWaterResistPercent, PlayerFields.PvpWaterResistPercent},
                    {EffectsEnum.Effect_SubPvpAirResistPercent, PlayerFields.PvpAirResistPercent},
                    {EffectsEnum.Effect_SubPvpFireResistPercent, PlayerFields.PvpFireResistPercent},
                    //{EffectsEnum.Effect_SubPvpNeutralElementReduction, PlayerFields.PvpNeutralElementReduction},
                    //{EffectsEnum.Effect_SubPvpEarthElementReduction, PlayerFields.PvpEarthElementReduction},
                    //{EffectsEnum.Effect_SubPvpWaterElementReduction, PlayerFields.PvpWaterElementReduction},
                    //{EffectsEnum.Effect_SubPvpAirElementReduction, PlayerFields.PvpAirElementReduction},
                    //{EffectsEnum.Effect_SubPvpFireElementReduction, PlayerFields.PvpFireElementReduction},
                    //{EffectsEnum.Effect_SubGlobalDamageReduction, PlayerFields.GlobalDamageReduction},
                    //{EffectsEnum.Effect_SubDamageMultiplicator, PlayerFields.DamageMultiplicator},
                    //{EffectsEnum.Effect_SubPhysicalDamage, PlayerFields.PhysicalDamage},
                    //{EffectsEnum.Effect_SubMagicDamage, PlayerFields.MagicDamage},
                    {EffectsEnum.Effect_SubPhysicalDamageReduction, PlayerFields.PhysicalDamageReduction},
                    {EffectsEnum.Effect_SubMagicDamageReduction, PlayerFields.MagicDamageReduction},
                };

        #endregion

        public DefaultItemEffect(EffectBase effect, Character target, PlayerItem item)
            : base(effect, target, item)
        {
        }

        public DefaultItemEffect(EffectBase effect, Character target, ItemSetTemplate itemSet, bool apply) 
            : base(effect, target, itemSet, apply)
        {
        }

        public override bool Apply()
        {
            if (!(Effect is EffectInteger))
                return false;

            EffectComputeHandler handler;

            PlayerFields caracteritic;
            if (m_addEffectsBinds.ContainsKey(Effect.EffectId))
            {
                caracteritic = m_addEffectsBinds[Effect.EffectId];

                if (!m_addMethods.ContainsKey(caracteritic) ||
                    !m_subMethods.ContainsKey(caracteritic))
                    return false;

                handler = Operation == HandlerOperation.APPLY ? m_addMethods[caracteritic] : m_subMethods[caracteritic];
            }
            else if (m_subEffectsBinds.ContainsKey(Effect.EffectId))
            {
                caracteritic = m_subEffectsBinds[Effect.EffectId];

                if (!m_addMethods.ContainsKey(caracteritic) ||
                    !m_subMethods.ContainsKey(caracteritic))
                    return false;

                handler = Operation == HandlerOperation.APPLY ? m_subMethods[caracteritic] : m_addMethods[caracteritic];
            }
            else
            {
                return false;
            }

            if (handler != null)
                handler(Target, Effect as EffectInteger);

            return true;
        }

        #region Add Methods

        private static void AddHealth(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Health].Equiped += effect.Value;
        }

        private static void AddInitiative(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Initiative].Equiped += effect.Value;
        }

        private static void AddProspecting(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Prospecting].Equiped += effect.Value;
        }

        private static void AddAP(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AP].Equiped += effect.Value;
        }

        private static void AddMP(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MP].Equiped += effect.Value;
        }

        private static void AddStrength(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Strength].Equiped += effect.Value;
        }

        private static void AddVitality(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Vitality].Equiped += effect.Value;
        }

        private static void AddWisdom(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Wisdom].Equiped += effect.Value;
        }

        private static void AddChance(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Chance].Equiped += effect.Value;
        }

        private static void AddAgility(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Agility].Equiped += effect.Value;
        }

        private static void AddIntelligence(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Intelligence].Equiped += effect.Value;
        }

        private static void AddRange(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Range].Equiped += effect.Value;
        }

        private static void AddSummonLimit(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.SummonLimit].Equiped += effect.Value;
        }

        private static void AddDamageReflection(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageReflection].Equiped += effect.Value;
        }

        private static void AddCriticalHit(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalHit].Equiped += effect.Value;
        }

        private static void AddCriticalMiss(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalMiss].Equiped += effect.Value;
        }

        private static void AddHealBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.HealBonus].Equiped += effect.Value;
        }

        private static void AddDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageBonus].Equiped += effect.Value;
        }

        private static void AddWeaponDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WeaponDamageBonus].Equiped += effect.Value;
        }

        private static void AddDamageBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageBonusPercent].Equiped += effect.Value;
        }

        private static void AddTrapBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TrapBonus].Equiped += effect.Value;
        }

        private static void AddTrapBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TrapBonusPercent].Equiped += effect.Value;
        }

        private static void AddPermanentDamagePercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PermanentDamagePercent].Equiped += effect.Value;
        }

        private static void AddTackleBlock(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TackleBlock].Equiped += effect.Value;
        }

        private static void AddTackleEvade(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TackleEvade].Equiped += effect.Value;
        }

        private static void AddAPAttack(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.APAttack].Equiped += effect.Value;
        }

        private static void AddMPAttack(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MPAttack].Equiped += effect.Value;
        }

        private static void AddPushDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PushDamageBonus].Equiped += effect.Value;
        }

        private static void AddCriticalDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalDamageBonus].Equiped += effect.Value;
        }

        private static void AddNeutralDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralDamageBonus].Equiped += effect.Value;
        }

        private static void AddEarthDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthDamageBonus].Equiped += effect.Value;
        }

        private static void AddWaterDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterDamageBonus].Equiped += effect.Value;
        }

        private static void AddAirDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirDamageBonus].Equiped += effect.Value;
        }

        private static void AddFireDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireDamageBonus].Equiped += effect.Value;
        }

        private static void AddDodgeAPProbability(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DodgeAPProbability].Equiped += effect.Value;
        }

        private static void AddDodgeMPProbability(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DodgeMPProbability].Equiped += effect.Value;
        }

        private static void AddNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralResistPercent].Equiped += effect.Value;
        }

        private static void AddEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthResistPercent].Equiped += effect.Value;
        }

        private static void AddWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterResistPercent].Equiped += effect.Value;
        }

        private static void AddAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirResistPercent].Equiped += effect.Value;
        }

        private static void AddFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireResistPercent].Equiped += effect.Value;
        }

        private static void AddNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralElementReduction].Equiped += effect.Value;
        }

        private static void AddEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthElementReduction].Equiped += effect.Value;
        }

        private static void AddWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterElementReduction].Equiped += effect.Value;
        }

        private static void AddAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirElementReduction].Equiped += effect.Value;
        }

        private static void AddFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireElementReduction].Equiped += effect.Value;
        }

        private static void AddPushDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PushDamageReduction].Equiped += effect.Value;
        }

        private static void AddCriticalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalDamageReduction].Equiped += effect.Value;
        }

        private static void AddPvpNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpNeutralResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpEarthResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpWaterResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpAirResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpFireResistPercent].Equiped += effect.Value;
        }

        private static void AddPvpNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpNeutralElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpEarthElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpWaterElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpAirElementReduction].Equiped += effect.Value;
        }

        private static void AddPvpFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpFireElementReduction].Equiped += effect.Value;
        }

        private static void AddGlobalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.GlobalDamageReduction].Equiped += effect.Value;
        }

        private static void AddDamageMultiplicator(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageMultiplicator].Equiped += effect.Value;
        }

        private static void AddPhysicalDamage(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PhysicalDamage].Equiped += effect.Value;
        }

        private static void AddMagicDamage(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MagicDamage].Equiped += effect.Value;
        }

        private static void AddPhysicalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PhysicalDamageReduction].Equiped += effect.Value;
        }

        private static void AddMagicDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MagicDamageReduction].Equiped += effect.Value;
        }

        #endregion

        #region Sub Methods

        private static void SubHealth(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Health].Equiped -= effect.Value;
        }

        private static void SubInitiative(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Initiative].Equiped -= effect.Value;
        }

        private static void SubProspecting(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Prospecting].Equiped -= effect.Value;
        }

        private static void SubAP(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AP].Equiped -= effect.Value;
        }

        private static void SubMP(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MP].Equiped -= effect.Value;
        }

        private static void SubStrength(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Strength].Equiped -= effect.Value;
        }

        private static void SubVitality(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Vitality].Equiped -= effect.Value;
        }

        private static void SubWisdom(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Wisdom].Equiped -= effect.Value;
        }

        private static void SubChance(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Chance].Equiped -= effect.Value;
        }

        private static void SubAgility(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Agility].Equiped -= effect.Value;
        }

        private static void SubIntelligence(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Intelligence].Equiped -= effect.Value;
        }

        private static void SubRange(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.Range].Equiped -= effect.Value;
        }

        private static void SubSummonLimit(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.SummonLimit].Equiped -= effect.Value;
        }

        private static void SubDamageReflection(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageReflection].Equiped -= effect.Value;
        }

        private static void SubCriticalHit(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalHit].Equiped -= effect.Value;
        }

        private static void SubCriticalMiss(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalMiss].Equiped -= effect.Value;
        }

        private static void SubHealBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.HealBonus].Equiped -= effect.Value;
        }

        private static void SubDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageBonus].Equiped -= effect.Value;
        }

        private static void SubWeaponDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WeaponDamageBonus].Equiped -= effect.Value;
        }

        private static void SubDamageBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageBonusPercent].Equiped -= effect.Value;
        }

        private static void SubTrapBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TrapBonus].Equiped -= effect.Value;
        }

        private static void SubTrapBonusPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TrapBonusPercent].Equiped -= effect.Value;
        }

        private static void SubPermanentDamagePercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PermanentDamagePercent].Equiped -= effect.Value;
        }

        private static void SubTackleBlock(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TackleBlock].Equiped -= effect.Value;
        }

        private static void SubTackleEvade(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.TackleEvade].Equiped -= effect.Value;
        }

        private static void SubAPAttack(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.APAttack].Equiped -= effect.Value;
        }

        private static void SubMPAttack(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MPAttack].Equiped -= effect.Value;
        }

        private static void SubPushDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PushDamageBonus].Equiped -= effect.Value;
        }

        private static void SubCriticalDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalDamageBonus].Equiped -= effect.Value;
        }

        private static void SubNeutralDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralDamageBonus].Equiped -= effect.Value;
        }

        private static void SubEarthDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthDamageBonus].Equiped -= effect.Value;
        }

        private static void SubWaterDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterDamageBonus].Equiped -= effect.Value;
        }

        private static void SubAirDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirDamageBonus].Equiped -= effect.Value;
        }

        private static void SubFireDamageBonus(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireDamageBonus].Equiped -= effect.Value;
        }

        private static void SubDodgeAPProbability(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DodgeAPProbability].Equiped -= effect.Value;
        }

        private static void SubDodgeMPProbability(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DodgeMPProbability].Equiped -= effect.Value;
        }

        private static void SubNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralResistPercent].Equiped -= effect.Value;
        }

        private static void SubEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthResistPercent].Equiped -= effect.Value;
        }

        private static void SubWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterResistPercent].Equiped -= effect.Value;
        }

        private static void SubAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirResistPercent].Equiped -= effect.Value;
        }

        private static void SubFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireResistPercent].Equiped -= effect.Value;
        }

        private static void SubNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.NeutralElementReduction].Equiped -= effect.Value;
        }

        private static void SubEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.EarthElementReduction].Equiped -= effect.Value;
        }

        private static void SubWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.WaterElementReduction].Equiped -= effect.Value;
        }

        private static void SubAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.AirElementReduction].Equiped -= effect.Value;
        }

        private static void SubFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.FireElementReduction].Equiped -= effect.Value;
        }

        private static void SubPushDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PushDamageReduction].Equiped -= effect.Value;
        }

        private static void SubCriticalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.CriticalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubPvpNeutralResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpNeutralResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpEarthResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpEarthResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpWaterResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpWaterResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpAirResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpAirResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpFireResistPercent(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpFireResistPercent].Equiped -= effect.Value;
        }

        private static void SubPvpNeutralElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpNeutralElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpEarthElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpEarthElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpWaterElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpWaterElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpAirElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpAirElementReduction].Equiped -= effect.Value;
        }

        private static void SubPvpFireElementReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PvpFireElementReduction].Equiped -= effect.Value;
        }

        private static void SubGlobalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.GlobalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubDamageMultiplicator(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.DamageMultiplicator].Equiped -= effect.Value;
        }

        private static void SubPhysicalDamage(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PhysicalDamage].Equiped -= effect.Value;
        }

        private static void SubMagicDamage(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MagicDamage].Equiped -= effect.Value;
        }

        private static void SubPhysicalDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.PhysicalDamageReduction].Equiped -= effect.Value;
        }

        private static void SubMagicDamageReduction(Character target, EffectInteger effect)
        {
            target.Stats[PlayerFields.MagicDamageReduction].Equiped -= effect.Value;
        }

        #endregion
    }
}