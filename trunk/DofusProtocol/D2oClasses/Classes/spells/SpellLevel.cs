
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellLevels")]
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

        public List<int> StatesRequired
        {
            get { return statesRequired; }
            set { statesRequired = value; }
        }

        public List<int> StatesForbidden
        {
            get { return statesForbidden; }
            set { statesForbidden = value; }
        }

        public List<EffectInstanceDice> Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        public List<EffectInstanceDice> CriticalEffect
        {
            get { return criticalEffect; }
            set { criticalEffect = value; }
        }

    }
}