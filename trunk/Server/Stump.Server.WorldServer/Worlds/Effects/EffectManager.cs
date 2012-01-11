using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Effects;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects.Handlers;
using Stump.Server.WorldServer.Worlds.Effects.Handlers.Items;
using Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Item = Stump.Server.WorldServer.Worlds.Items.Item;
using Spell = Stump.Server.WorldServer.Worlds.Spells.Spell;

namespace Stump.Server.WorldServer.Worlds.Effects
{
    public class EffectManager : Singleton<EffectManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private delegate ItemEffectHandler ItemEffectConstructor(Character target, Item item, EffectBase effect);
        private delegate SpellEffectHandler SpellEffectConstructor(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical);

        private Dictionary<short, EffectTemplate> m_effects = new Dictionary<short, EffectTemplate>();
        private readonly Dictionary<EffectsEnum, ItemEffectConstructor> m_itemsEffectHandler = new Dictionary<EffectsEnum, ItemEffectConstructor>();
        private readonly Dictionary<EffectsEnum, SpellEffectConstructor> m_spellsEffectHandler = new Dictionary<EffectsEnum, SpellEffectConstructor>();

        [Initialization(InitializationPass.Third)]
        public void Initialize()
        {
            m_effects = ActiveRecordBase<EffectTemplate>.FindAll().ToDictionary(entry => (short) entry.Id);

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(entry => entry.IsSubclassOf(typeof(EffectHandler)) && !entry.IsAbstract))
            {
                if (type.GetCustomAttribute<DefaultEffectHandlerAttribute>() != null)
                    continue; // we don't mind about default handlers

                var attributes = type.GetCustomAttributes<EffectHandlerAttribute>();

                if (attributes.Length == 0)
                {
                    logger.Error("EffectHandler '{0}' has no EffectHandlerAttribute", type.Name);
                    continue;
                }

                foreach (var effect in attributes.Select(entry => entry.Effect))
                {
                    if (type.IsSubclassOf(typeof(ItemEffectHandler)))
                    {
                        var ctor = type.GetConstructor(new [] { typeof(Character), typeof(Item), typeof(EffectBase)});
                        m_itemsEffectHandler.Add(effect, ctor.CreateDelegate<ItemEffectConstructor>());
                    }
                    else if (type.IsSubclassOf(typeof(SpellEffectHandler)))
                    {
                        var ctor = type.GetConstructor(new[] { typeof(EffectDice), typeof(FightActor), typeof(Spell), typeof(Cell), typeof(bool) });
                        m_spellsEffectHandler.Add(effect, ctor.CreateDelegate<SpellEffectConstructor>());
                    }
                }
            } 
        }

        /// <summary>
        ///   D2O Effect class to stump effect class
        /// </summary>
        /// <param name = "effect"></param>
        /// <returns></returns>
        public EffectBase ConvertExportedEffect(EffectInstance effect)
        {
            if (effect is EffectInstanceLadder)
                return new EffectLadder(effect as EffectInstanceLadder);
            if (effect is EffectInstanceCreature)
                return new EffectCreature(effect as EffectInstanceCreature);
            if (effect is EffectInstanceDate)
                return new EffectDate(effect as EffectInstanceDate);
            if (effect is EffectInstanceDice)
                return new EffectDice(effect as EffectInstanceDice);
            if (effect is EffectInstanceDuration)
                return new EffectDuration(effect as EffectInstanceDuration);
            if (effect is EffectInstanceMinMax)
                return new EffectMinMax(effect as EffectInstanceMinMax);
            if (effect is EffectInstanceMount)
                return new EffectMount(effect as EffectInstanceMount);
            if (effect is EffectInstanceString)
                return new EffectString(effect as EffectInstanceString);
            if (effect is EffectInstanceInteger)
                return new EffectInteger(effect as EffectInstanceInteger);

            return new EffectBase(effect);
        }

        public IEnumerable<EffectBase> ConvertExportedEffect(IEnumerable<EffectInstance> effects)
        {
            return effects.Select(ConvertExportedEffect);
        }

        public EffectTemplate GetTemplate(short id)
        {
            return !m_effects.ContainsKey(id) ? null : m_effects[id];
        }

        public ItemEffectHandler GetItemEffectHandler(Character target, Item item, EffectBase effect)
        {
            ItemEffectConstructor handler;
            if (m_itemsEffectHandler.TryGetValue(effect.EffectId, out handler))
            {
                return handler(target, item, effect);
            }

            return new DefaultItemEffect(effect, target, item);
        }

        public SpellEffectHandler GetSpellEffectHandler(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
        {
            SpellEffectConstructor handler;
            if (m_spellsEffectHandler.TryGetValue(effect.EffectId, out handler))
            {
                return handler(effect, caster, spell, targetedCell, critical);
            }

            return new DefaultSpellEffect(effect, caster, spell, targetedCell, critical);
        }

        public bool IsEffectRandomable(EffectsEnum effect)
        {
            return m_randomablesEffects.Contains(effect);
        }

        public EffectInstance GuessRealEffect(EffectInstance effect)
        {
            if (!(effect is EffectInstanceDice))
                return effect;

            var effectDice = effect as EffectInstanceDice;

            if (effectDice.value == 0 && effectDice.diceNum > 0 && effectDice.diceSide > 0)
            {
                return new EffectInstanceMinMax
                           {
                               duration = effectDice.duration,
                               effectId = effectDice.effectId,
                               max = effectDice.diceSide,
                               min = effectDice.diceNum,
                               modificator = effectDice.modificator,
                               random = effectDice.random,
                               targetId = effectDice.targetId,
                               trigger = effectDice.trigger,
                               zoneShape = effectDice.zoneShape,
                               zoneSize = effectDice.zoneSize
                           };
            }

            if (effectDice.value == 0 && effectDice.diceNum == 0 && effectDice.diceSide > 0)
            {
                return new EffectInstanceMinMax
                           {
                               duration = effectDice.duration,
                               effectId = effectDice.effectId,
                               max = effectDice.diceSide,
                               min = effectDice.diceNum,
                               modificator = effectDice.modificator,
                               random = effectDice.random,
                               targetId = effectDice.targetId,
                               trigger = effectDice.trigger,
                               zoneShape = effectDice.zoneShape,
                               zoneSize = effectDice.zoneSize
                           };
            }

            return effect;
        }

        public byte[] SerializeEffect(EffectInstance effectInstance)
        {
            return ConvertExportedEffect(effectInstance).Serialize();
        }

        public byte[] SerializeEffect(EffectBase effect)
        {
            return effect.Serialize();
        }

        public byte[] SerializeEffects(IEnumerable<EffectBase> effects)
        {
            var buffer = new List<byte>();

            foreach (var effect in effects)
            {
                buffer.AddRange(effect.Serialize());
            }

            return buffer.ToArray();
        }

        public byte[] SerializeEffects(IEnumerable<EffectInstance> effects)
        {
            var buffer = new List<byte>();

            foreach (var effect in effects)
            {
                buffer.AddRange(SerializeEffect(effect));
            }

            return buffer.ToArray();
        }


        public List<EffectBase> DeserializeEffects(byte[] buffer, int index = 0)
        {
            var result = new List<EffectBase>();

            int i = 0;
            while (i + 1 < buffer.Length)
            {
                result.Add(DeserializeEffect(buffer, ref i));
            }

            return result;
        }

        public EffectBase DeserializeEffect(byte[] buffer, ref int index)
        {
            if (buffer.Length < index)
                throw new Exception("buffer too small to contain an Effect");

            var identifier = buffer[0 + index];
            EffectBase effect;

            switch (identifier)
            {
                case 1:
                    effect = new EffectBase();
                    break;
                case 2:
                    effect = new EffectCreature();
                    break;
                case 3:
                    effect = new EffectDate();
                    break;
                case 4:
                    effect = new EffectDice();
                    break;
                case 5:
                    effect = new EffectDuration();
                    break;
                case 6:
                    effect = new EffectInteger();
                    break;
                case 7:
                    effect = new EffectLadder();
                    break;
                case 8:
                    effect = new EffectMinMax();
                    break;
                case 9:
                    effect = new EffectMount();
                    break;
                case 10:
                    effect = new EffectString();
                    break;
                default:
                    throw new Exception(string.Format("Incorrect identifier : {0}", identifier));
            }

            index++;
            effect.DeSerialize(buffer, ref index);

            return effect;
        }

        #region Randomable Effects

        /// <summary>
        ///   Effects that are random when a new item is generated
        /// </summary>
        private readonly EffectsEnum[] m_randomablesEffects =
            new[]
                {
                    EffectsEnum.Effect_AddMP,
                    EffectsEnum.Effect_AddGlobalDamageReduction_105,
                    EffectsEnum.Effect_AddDamageReflection,
                    EffectsEnum.Effect_AddHealth,
                    EffectsEnum.Effect_AddAP_111,
                    EffectsEnum.Effect_AddDamageBonus,
                    EffectsEnum.Effect_AddDamageMultiplicator,
                    EffectsEnum.Effect_AddCriticalHit,
                    EffectsEnum.Effect_SubRange,
                    EffectsEnum.Effect_AddRange,
                    EffectsEnum.Effect_AddStrength,
                    EffectsEnum.Effect_AddAgility,
                    EffectsEnum.Effect_RegainAP,
                    EffectsEnum.Effect_AddDamageBonus_121,
                    EffectsEnum.Effect_AddCriticalMiss,
                    EffectsEnum.Effect_AddChance,
                    EffectsEnum.Effect_AddWisdom,
                    EffectsEnum.Effect_AddVitality,
                    EffectsEnum.Effect_AddIntelligence,
                    EffectsEnum.Effect_AddMP_128,
                    EffectsEnum.Effect_SubRange_135,
                    EffectsEnum.Effect_AddRange_136,
                    EffectsEnum.Effect_AddPhysicalDamage_137,
                    EffectsEnum.Effect_IncreaseDamage_138,
                    EffectsEnum.Effect_AddPhysicalDamage_142,
                    EffectsEnum.Effect_SubDamageBonus,
                    EffectsEnum.Effect_SubChance,
                    EffectsEnum.Effect_SubVitality,
                    EffectsEnum.Effect_SubAgility,
                    EffectsEnum.Effect_SubIntelligence,
                    EffectsEnum.Effect_SubWisdom,
                    EffectsEnum.Effect_SubStrength,
                    EffectsEnum.Effect_IncreaseWeight,
                    EffectsEnum.Effect_DecreaseWeight,
                    EffectsEnum.Effect_IncreaseAPAvoid,
                    EffectsEnum.Effect_IncreaseMPAvoid,
                    EffectsEnum.Effect_SubDodgeAPProbability,
                    EffectsEnum.Effect_SubDodgeMPProbability,
                    EffectsEnum.Effect_AddGlobalDamageReduction,
                    EffectsEnum.Effect_AddDamageBonusPercent,
                    EffectsEnum.Effect_SubAP,
                    EffectsEnum.Effect_SubMP,
                    EffectsEnum.Effect_SubCriticalHit,
                    EffectsEnum.Effect_SubMagicDamageReduction,
                    EffectsEnum.Effect_SubPhysicalDamageReduction,
                    EffectsEnum.Effect_AddInitiative,
                    EffectsEnum.Effect_SubInitiative,
                    EffectsEnum.Effect_AddProspecting,
                    EffectsEnum.Effect_SubProspecting,
                    EffectsEnum.Effect_AddHealBonus,
                    EffectsEnum.Effect_SubHealBonus,
                    EffectsEnum.Effect_AddSummonLimit,
                    EffectsEnum.Effect_AddMagicDamageReduction,
                    EffectsEnum.Effect_AddPhysicalDamageReduction,
                    EffectsEnum.Effect_SubDamageBonusPercent,
                    EffectsEnum.Effect_AddEarthResistPercent,
                    EffectsEnum.Effect_AddWaterResistPercent,
                    EffectsEnum.Effect_AddAirResistPercent,
                    EffectsEnum.Effect_AddFireResistPercent,
                    EffectsEnum.Effect_AddNeutralResistPercent,
                    EffectsEnum.Effect_SubEarthResistPercent,
                    EffectsEnum.Effect_SubWaterResistPercent,
                    EffectsEnum.Effect_SubAirResistPercent,
                    EffectsEnum.Effect_SubFireResistPercent,
                    EffectsEnum.Effect_SubNeutralResistPercent,
                    EffectsEnum.Effect_AddTrapBonus,
                    EffectsEnum.Effect_AddTrapBonusPercent,
                    EffectsEnum.Effect_AddEarthElementReduction,
                    EffectsEnum.Effect_AddWaterElementReduction,
                    EffectsEnum.Effect_AddAirElementReduction,
                    EffectsEnum.Effect_AddFireElementReduction,
                    EffectsEnum.Effect_AddNeutralElementReduction,
                    EffectsEnum.Effect_SubEarthElementReduction,
                    EffectsEnum.Effect_SubWaterElementReduction,
                    EffectsEnum.Effect_SubAirElementReduction,
                    EffectsEnum.Effect_SubFireElementReduction,
                    EffectsEnum.Effect_SubNeutralElementReduction,
                    EffectsEnum.Effect_AddPvpEarthResistPercent,
                    EffectsEnum.Effect_AddPvpWaterResistPercent,
                    EffectsEnum.Effect_AddPvpAirResistPercent,
                    EffectsEnum.Effect_AddPvpFireResistPercent,
                    EffectsEnum.Effect_AddPvpNeutralResistPercent,
                    EffectsEnum.Effect_SubPvpEarthResistPercent,
                    EffectsEnum.Effect_SubPvpWaterResistPercent,
                    EffectsEnum.Effect_SubPvpAirResistPercent,
                    EffectsEnum.Effect_SubPvpFireResistPercent,
                    EffectsEnum.Effect_SubPvpNeutralResistPercent,
                    EffectsEnum.Effect_AddPvpEarthElementReduction,
                    EffectsEnum.Effect_AddPvpWaterElementReduction,
                    EffectsEnum.Effect_AddPvpAirElementReduction,
                    EffectsEnum.Effect_AddPvpFireElementReduction,
                    EffectsEnum.Effect_AddPvpNeutralElementReduction,
                    EffectsEnum.Effect_AddPushDamageBonus,
                    EffectsEnum.Effect_SubPushDamageBonus,
                    EffectsEnum.Effect_AddPushDamageReduction,
                    EffectsEnum.Effect_SubPushDamageReduction,
                    EffectsEnum.Effect_AddCriticalDamageBonus,
                    EffectsEnum.Effect_SubCriticalDamageBonus,
                    EffectsEnum.Effect_AddCriticalDamageReduction,
                    EffectsEnum.Effect_SubCriticalDamageReduction,
                    EffectsEnum.Effect_AddEarthDamageBonus,
                    EffectsEnum.Effect_SubEarthDamageBonus,
                    EffectsEnum.Effect_AddFireDamageBonus,
                    EffectsEnum.Effect_SubFireDamageBonus,
                    EffectsEnum.Effect_AddWaterDamageBonus,
                    EffectsEnum.Effect_SubWaterDamageBonus,
                    EffectsEnum.Effect_AddAirDamageBonus,
                    EffectsEnum.Effect_SubAirDamageBonus,
                    EffectsEnum.Effect_AddNeutralDamageBonus,
                    EffectsEnum.Effect_SubNeutralDamageBonus,
                };

        #endregion
    }
}