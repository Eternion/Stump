

// Generated on 10/28/2013 14:03:21
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellLevel", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class SpellLevel : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
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
        public List<int> StatesRequired
        {
            get { return statesRequired; }
            set { statesRequired = value; }
        }
        [D2OIgnore]
        public List<int> StatesForbidden
        {
            get { return statesForbidden; }
            set { statesForbidden = value; }
        }
        [D2OIgnore]
        public List<EffectInstanceDice> Effects
        {
            get { return effects; }
            set { effects = value; }
        }
        [D2OIgnore]
        public List<EffectInstanceDice> CriticalEffect
        {
            get { return criticalEffect; }
            set { criticalEffect = value; }
        }
    }
}