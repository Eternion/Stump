using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Database.Spells
{
    [Serializable]
    [ActiveRecord("spells_level")]
    [D2OClass("SpellLevel", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellLevelTemplate : WorldBaseRecord<SpellLevelTemplate>
    {
        private List<EffectDice> m_criticalEffects;
        private List<EffectDice> m_effects;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [D2OField("spellId")]
        [Property("SpellId")]
        public int SpellId
        {
            get;
            set;
        }

        private SpellTemplate m_spell;

        public SpellTemplate Spell
        {
            get { return m_spell ?? (m_spell = SpellManager.Instance.GetSpellTemplate(SpellId)); }
        }

        [D2OField("spellBreed")]
        [Property("SpellBreed")]
        public uint SpellBreed
        {
            get;
            set;
        }

        [D2OField("apCost")]
        [Property("ApCost")]
        public uint ApCost
        {
            get;
            set;
        }

        [D2OField("range")]
        [Property("`Range`")]
        public uint Range
        {
            get;
            set;
        }

        [D2OField("castInLine")]
        [Property("CastInLine")]
        public Boolean CastInLine
        {
            get;
            set;
        }

        [D2OField("castInDiagonal")]
        [Property("CastInDiagonal")]
        public Boolean CastInDiagonal
        {
            get;
            set;
        }

        [D2OField("castTestLos")]
        [Property("CastTestLos")]
        public Boolean CastTestLos
        {
            get;
            set;
        }

        [D2OField("criticalHitProbability")]
        [Property("CriticalHitProbability")]
        public uint CriticalHitProbability
        {
            get;
            set;
        }

        [D2OField("statesRequired")]
        [Property("StatesRequired", ColumnType = "Serializable")]
        public List<int> StatesRequired
        {
            get;
            set;
        }

        [D2OField("criticalFailureProbability")]
        [Property("CriticalFailureProbability")]
        public uint CriticalFailureProbability
        {
            get;
            set;
        }

        [D2OField("needFreeCell")]
        [Property("NeedFreeCell")]
        public Boolean NeedFreeCell
        {
            get;
            set;
        }

        [D2OField("needFreeTrapCell")]
        [Property("NeedFreeTrapCell")]
        public Boolean NeedFreeTrapCell
        {
            get;
            set;
        }

        [D2OField("rangeCanBeBoosted")]
        [Property("RangeCanBeBoosted")]
        public Boolean RangeCanBeBoosted
        {
            get;
            set;
        }

        [D2OField("maxCastPerTurn")]
        [Property("MaxCastPerTurn")]
        public uint MaxCastPerTurn
        {
            get;
            set;
        }

        [D2OField("maxCastPerTarget")]
        [Property("MaxCastPerTarget")]
        public uint MaxCastPerTarget
        {
            get;
            set;
        }

        [D2OField("minCastInterval")]
        [Property("MinCastInterval")]
        public uint MinCastInterval
        {
            get;
            set;
        }

        [D2OField("initialCooldown")]
        [Property("InitialCooldown")]
        public uint InitialCooldown
        {
            get;
            set;
        }

        [D2OField("minPlayerLevel")]
        [Property("MinPlayerLevel")]
        public uint MinPlayerLevel
        {
            get;
            set;
        }

        [D2OField("criticalFailureEndsTurn")]
        [Property("CriticalFailureEndsTurn")]
        public Boolean CriticalFailureEndsTurn
        {
            get;
            set;
        }

        [D2OField("hideEffects")]
        [Property("HideEffects")]
        public Boolean HideEffects
        {
            get;
            set;
        }

        [D2OField("minRange")]
        [Property("MinRange")]
        public uint MinRange
        {
            get;
            set;
        }

        [D2OField("statesForbidden")]
        [Property("StatesForbidden", ColumnType = "Serializable")]
        public List<int> StatesForbidden
        {
            get;
            set;
        }

        [D2OField("effects")]
        [Property("Effects", ColumnType = "Serializable")]
        public List<EffectInstanceDice> RawEffects
        {
            get;
            set;
        }

        public List<EffectDice> Effects
        {
            get { return m_effects ?? (m_effects = new List<EffectDice>(RawEffects.Select(entry => EffectManager.Instance.ConvertExportedEffect(entry) as EffectDice))); }
            set { m_effects = value; }
        }

        [D2OField("criticalEffect")]
        [Property("CriticalEffect", ColumnType = "Serializable")]
        public List<EffectInstanceDice> RawCriticalEffect
        {
            get;
            set;
        }

        public List<EffectDice> CritialEffects
        {
            get { return m_criticalEffects ?? (m_criticalEffects = new List<EffectDice>(RawCriticalEffect.Select(entry => EffectManager.Instance.ConvertExportedEffect(entry) as EffectDice))); }
            set { m_criticalEffects = value; }
        }
    }
}