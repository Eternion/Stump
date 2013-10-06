 


// Generated on 10/06/2013 18:02:19
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpellLevels")]
    [D2OClass("SpellLevel", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellLevelRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
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

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint SpellId
        {
            get { return spellId; }
            set { spellId = value; }
        }

        [D2OIgnore]
        public uint SpellBreed
        {
            get { return spellBreed; }
            set { spellBreed = value; }
        }

        [D2OIgnore]
        public uint ApCost
        {
            get { return apCost; }
            set { apCost = value; }
        }

        [D2OIgnore]
        public uint MinRange
        {
            get { return minRange; }
            set { minRange = value; }
        }

        [D2OIgnore]
        public uint Range
        {
            get { return range; }
            set { range = value; }
        }

        [D2OIgnore]
        public Boolean CastInLine
        {
            get { return castInLine; }
            set { castInLine = value; }
        }

        [D2OIgnore]
        public Boolean CastInDiagonal
        {
            get { return castInDiagonal; }
            set { castInDiagonal = value; }
        }

        [D2OIgnore]
        public Boolean CastTestLos
        {
            get { return castTestLos; }
            set { castTestLos = value; }
        }

        [D2OIgnore]
        public uint CriticalHitProbability
        {
            get { return criticalHitProbability; }
            set { criticalHitProbability = value; }
        }

        [D2OIgnore]
        public uint CriticalFailureProbability
        {
            get { return criticalFailureProbability; }
            set { criticalFailureProbability = value; }
        }

        [D2OIgnore]
        public Boolean NeedFreeCell
        {
            get { return needFreeCell; }
            set { needFreeCell = value; }
        }

        [D2OIgnore]
        public Boolean NeedTakenCell
        {
            get { return needTakenCell; }
            set { needTakenCell = value; }
        }

        [D2OIgnore]
        public Boolean NeedFreeTrapCell
        {
            get { return needFreeTrapCell; }
            set { needFreeTrapCell = value; }
        }

        [D2OIgnore]
        public Boolean RangeCanBeBoosted
        {
            get { return rangeCanBeBoosted; }
            set { rangeCanBeBoosted = value; }
        }

        [D2OIgnore]
        public int MaxStack
        {
            get { return maxStack; }
            set { maxStack = value; }
        }

        [D2OIgnore]
        public uint MaxCastPerTurn
        {
            get { return maxCastPerTurn; }
            set { maxCastPerTurn = value; }
        }

        [D2OIgnore]
        public uint MaxCastPerTarget
        {
            get { return maxCastPerTarget; }
            set { maxCastPerTarget = value; }
        }

        [D2OIgnore]
        public uint MinCastInterval
        {
            get { return minCastInterval; }
            set { minCastInterval = value; }
        }

        [D2OIgnore]
        public uint InitialCooldown
        {
            get { return initialCooldown; }
            set { initialCooldown = value; }
        }

        [D2OIgnore]
        public int GlobalCooldown
        {
            get { return globalCooldown; }
            set { globalCooldown = value; }
        }

        [D2OIgnore]
        public uint MinPlayerLevel
        {
            get { return minPlayerLevel; }
            set { minPlayerLevel = value; }
        }

        [D2OIgnore]
        public Boolean CriticalFailureEndsTurn
        {
            get { return criticalFailureEndsTurn; }
            set { criticalFailureEndsTurn = value; }
        }

        [D2OIgnore]
        public Boolean HideEffects
        {
            get { return hideEffects; }
            set { hideEffects = value; }
        }

        [D2OIgnore]
        public Boolean Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        public byte[] StatesRequiredBin
        {
            get { return m_statesRequiredBin; }
            set
            {
                m_statesRequiredBin = value;
                statesRequired = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        public byte[] StatesForbiddenBin
        {
            get { return m_statesForbiddenBin; }
            set
            {
                m_statesForbiddenBin = value;
                statesForbidden = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        public byte[] EffectsBin
        {
            get { return m_effectsBin; }
            set
            {
                m_effectsBin = value;
                effects = value == null ? null : value.ToObject<List<EffectInstanceDice>>();
            }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        public byte[] CriticalEffectBin
        {
            get { return m_criticalEffectBin; }
            set
            {
                m_criticalEffectBin = value;
                criticalEffect = value == null ? null : value.ToObject<List<EffectInstanceDice>>();
            }
        }

        public virtual void AssignFields(object obj)
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SpellLevel)parent : new SpellLevel();
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