 


// Generated on 10/06/2013 01:11:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("SpellLevels")]
    public class SpellLevelRecord : ID2ORecord
    {
        private const String MODULE = "SpellLevels";
        public uint id;
        public uint spellId;
        public uint spellBreed;
        public uint apCost;
        public uint minRange;
        public uint range;
        public Boolean castInLine;
        public Boolean castInDiagonal;
        public Boolean castTestLos;
        public uint criticalHitProbability;
        public uint criticalFailureProbability;
        public Boolean needFreeCell;
        public Boolean needTakenCell;
        public Boolean needFreeTrapCell;
        public Boolean rangeCanBeBoosted;
        public int maxStack;
        public uint maxCastPerTurn;
        public uint maxCastPerTarget;
        public uint minCastInterval;
        public uint initialCooldown;
        public int globalCooldown;
        public uint minPlayerLevel;
        public Boolean criticalFailureEndsTurn;
        public Boolean hideEffects;
        public Boolean hidden;
        public List<int> statesRequired;
        public List<int> statesForbidden;
        public List<EffectInstanceDice> effects;
        public List<EffectInstanceDice> criticalEffect;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint SpellId
        {
            get { return spellId; }
            set { spellId = value; }
        }

        public uint SpellBreed
        {
            get { return spellBreed; }
            set { spellBreed = value; }
        }

        public uint ApCost
        {
            get { return apCost; }
            set { apCost = value; }
        }

        public uint MinRange
        {
            get { return minRange; }
            set { minRange = value; }
        }

        public uint Range
        {
            get { return range; }
            set { range = value; }
        }

        public Boolean CastInLine
        {
            get { return castInLine; }
            set { castInLine = value; }
        }

        public Boolean CastInDiagonal
        {
            get { return castInDiagonal; }
            set { castInDiagonal = value; }
        }

        public Boolean CastTestLos
        {
            get { return castTestLos; }
            set { castTestLos = value; }
        }

        public uint CriticalHitProbability
        {
            get { return criticalHitProbability; }
            set { criticalHitProbability = value; }
        }

        public uint CriticalFailureProbability
        {
            get { return criticalFailureProbability; }
            set { criticalFailureProbability = value; }
        }

        public Boolean NeedFreeCell
        {
            get { return needFreeCell; }
            set { needFreeCell = value; }
        }

        public Boolean NeedTakenCell
        {
            get { return needTakenCell; }
            set { needTakenCell = value; }
        }

        public Boolean NeedFreeTrapCell
        {
            get { return needFreeTrapCell; }
            set { needFreeTrapCell = value; }
        }

        public Boolean RangeCanBeBoosted
        {
            get { return rangeCanBeBoosted; }
            set { rangeCanBeBoosted = value; }
        }

        public int MaxStack
        {
            get { return maxStack; }
            set { maxStack = value; }
        }

        public uint MaxCastPerTurn
        {
            get { return maxCastPerTurn; }
            set { maxCastPerTurn = value; }
        }

        public uint MaxCastPerTarget
        {
            get { return maxCastPerTarget; }
            set { maxCastPerTarget = value; }
        }

        public uint MinCastInterval
        {
            get { return minCastInterval; }
            set { minCastInterval = value; }
        }

        public uint InitialCooldown
        {
            get { return initialCooldown; }
            set { initialCooldown = value; }
        }

        public int GlobalCooldown
        {
            get { return globalCooldown; }
            set { globalCooldown = value; }
        }

        public uint MinPlayerLevel
        {
            get { return minPlayerLevel; }
            set { minPlayerLevel = value; }
        }

        public Boolean CriticalFailureEndsTurn
        {
            get { return criticalFailureEndsTurn; }
            set { criticalFailureEndsTurn = value; }
        }

        public Boolean HideEffects
        {
            get { return hideEffects; }
            set { hideEffects = value; }
        }

        public Boolean Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        [Ignore]
        public List<int> StatesRequired
        {
            get { return statesRequired; }
            set
            {
                statesRequired = value;
                m_statesRequiredBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statesRequiredBin;
        public byte[] StatesRequiredBin
        {
            get { return m_statesRequiredBin; }
            set
            {
                m_statesRequiredBin = value;
                statesRequired = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [Ignore]
        public List<int> StatesForbidden
        {
            get { return statesForbidden; }
            set
            {
                statesForbidden = value;
                m_statesForbiddenBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statesForbiddenBin;
        public byte[] StatesForbiddenBin
        {
            get { return m_statesForbiddenBin; }
            set
            {
                m_statesForbiddenBin = value;
                statesForbidden = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [Ignore]
        public List<EffectInstanceDice> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
                m_effectsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_effectsBin;
        public byte[] EffectsBin
        {
            get { return m_effectsBin; }
            set
            {
                m_effectsBin = value;
                effects = value == null ? null : value.ToObject<List<EffectInstanceDice>>();
            }
        }

        [Ignore]
        public List<EffectInstanceDice> CriticalEffect
        {
            get { return criticalEffect; }
            set
            {
                criticalEffect = value;
                m_criticalEffectBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_criticalEffectBin;
        public byte[] CriticalEffectBin
        {
            get { return m_criticalEffectBin; }
            set
            {
                m_criticalEffectBin = value;
                criticalEffect = value == null ? null : value.ToObject<List<EffectInstanceDice>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SpellLevel)obj;
            
            Id = castedObj.id;
            SpellId = castedObj.spellId;
            SpellBreed = castedObj.spellBreed;
            ApCost = castedObj.apCost;
            MinRange = castedObj.minRange;
            Range = castedObj.range;
            CastInLine = castedObj.castInLine;
            CastInDiagonal = castedObj.castInDiagonal;
            CastTestLos = castedObj.castTestLos;
            CriticalHitProbability = castedObj.criticalHitProbability;
            CriticalFailureProbability = castedObj.criticalFailureProbability;
            NeedFreeCell = castedObj.needFreeCell;
            NeedTakenCell = castedObj.needTakenCell;
            NeedFreeTrapCell = castedObj.needFreeTrapCell;
            RangeCanBeBoosted = castedObj.rangeCanBeBoosted;
            MaxStack = castedObj.maxStack;
            MaxCastPerTurn = castedObj.maxCastPerTurn;
            MaxCastPerTarget = castedObj.maxCastPerTarget;
            MinCastInterval = castedObj.minCastInterval;
            InitialCooldown = castedObj.initialCooldown;
            GlobalCooldown = castedObj.globalCooldown;
            MinPlayerLevel = castedObj.minPlayerLevel;
            CriticalFailureEndsTurn = castedObj.criticalFailureEndsTurn;
            HideEffects = castedObj.hideEffects;
            Hidden = castedObj.hidden;
            StatesRequired = castedObj.statesRequired;
            StatesForbidden = castedObj.statesForbidden;
            Effects = castedObj.effects;
            CriticalEffect = castedObj.criticalEffect;
        }
        
        public object CreateObject()
        {
            var obj = new SpellLevel();
            
            obj.id = Id;
            obj.spellId = SpellId;
            obj.spellBreed = SpellBreed;
            obj.apCost = ApCost;
            obj.minRange = MinRange;
            obj.range = Range;
            obj.castInLine = CastInLine;
            obj.castInDiagonal = CastInDiagonal;
            obj.castTestLos = CastTestLos;
            obj.criticalHitProbability = CriticalHitProbability;
            obj.criticalFailureProbability = CriticalFailureProbability;
            obj.needFreeCell = NeedFreeCell;
            obj.needTakenCell = NeedTakenCell;
            obj.needFreeTrapCell = NeedFreeTrapCell;
            obj.rangeCanBeBoosted = RangeCanBeBoosted;
            obj.maxStack = MaxStack;
            obj.maxCastPerTurn = MaxCastPerTurn;
            obj.maxCastPerTarget = MaxCastPerTarget;
            obj.minCastInterval = MinCastInterval;
            obj.initialCooldown = InitialCooldown;
            obj.globalCooldown = GlobalCooldown;
            obj.minPlayerLevel = MinPlayerLevel;
            obj.criticalFailureEndsTurn = CriticalFailureEndsTurn;
            obj.hideEffects = HideEffects;
            obj.hidden = Hidden;
            obj.statesRequired = StatesRequired;
            obj.statesForbidden = StatesForbidden;
            obj.effects = Effects;
            obj.criticalEffect = CriticalEffect;
            return obj;
        
        }
    }
}