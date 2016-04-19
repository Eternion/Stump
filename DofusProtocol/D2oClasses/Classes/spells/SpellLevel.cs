

// Generated on 04/19/2016 10:18:02
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
        public const String MODULE = "SpellLevels";
        public uint id;
        public uint spellId;
        public uint grade;
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
        public Boolean playAnimation;
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint SpellId
        {
            get { return this.spellId; }
            set { this.spellId = value; }
        }
        [D2OIgnore]
        public uint Grade
        {
            get { return this.grade; }
            set { this.grade = value; }
        }
        [D2OIgnore]
        public uint SpellBreed
        {
            get { return this.spellBreed; }
            set { this.spellBreed = value; }
        }
        [D2OIgnore]
        public uint ApCost
        {
            get { return this.apCost; }
            set { this.apCost = value; }
        }
        [D2OIgnore]
        public uint MinRange
        {
            get { return this.minRange; }
            set { this.minRange = value; }
        }
        [D2OIgnore]
        public uint Range
        {
            get { return this.range; }
            set { this.range = value; }
        }
        [D2OIgnore]
        public Boolean CastInLine
        {
            get { return this.castInLine; }
            set { this.castInLine = value; }
        }
        [D2OIgnore]
        public Boolean CastInDiagonal
        {
            get { return this.castInDiagonal; }
            set { this.castInDiagonal = value; }
        }
        [D2OIgnore]
        public Boolean CastTestLos
        {
            get { return this.castTestLos; }
            set { this.castTestLos = value; }
        }
        [D2OIgnore]
        public uint CriticalHitProbability
        {
            get { return this.criticalHitProbability; }
            set { this.criticalHitProbability = value; }
        }
        [D2OIgnore]
        public uint CriticalFailureProbability
        {
            get { return this.criticalFailureProbability; }
            set { this.criticalFailureProbability = value; }
        }
        [D2OIgnore]
        public Boolean NeedFreeCell
        {
            get { return this.needFreeCell; }
            set { this.needFreeCell = value; }
        }
        [D2OIgnore]
        public Boolean NeedTakenCell
        {
            get { return this.needTakenCell; }
            set { this.needTakenCell = value; }
        }
        [D2OIgnore]
        public Boolean NeedFreeTrapCell
        {
            get { return this.needFreeTrapCell; }
            set { this.needFreeTrapCell = value; }
        }
        [D2OIgnore]
        public Boolean RangeCanBeBoosted
        {
            get { return this.rangeCanBeBoosted; }
            set { this.rangeCanBeBoosted = value; }
        }
        [D2OIgnore]
        public int MaxStack
        {
            get { return this.maxStack; }
            set { this.maxStack = value; }
        }
        [D2OIgnore]
        public uint MaxCastPerTurn
        {
            get { return this.maxCastPerTurn; }
            set { this.maxCastPerTurn = value; }
        }
        [D2OIgnore]
        public uint MaxCastPerTarget
        {
            get { return this.maxCastPerTarget; }
            set { this.maxCastPerTarget = value; }
        }
        [D2OIgnore]
        public uint MinCastInterval
        {
            get { return this.minCastInterval; }
            set { this.minCastInterval = value; }
        }
        [D2OIgnore]
        public uint InitialCooldown
        {
            get { return this.initialCooldown; }
            set { this.initialCooldown = value; }
        }
        [D2OIgnore]
        public int GlobalCooldown
        {
            get { return this.globalCooldown; }
            set { this.globalCooldown = value; }
        }
        [D2OIgnore]
        public uint MinPlayerLevel
        {
            get { return this.minPlayerLevel; }
            set { this.minPlayerLevel = value; }
        }
        [D2OIgnore]
        public Boolean CriticalFailureEndsTurn
        {
            get { return this.criticalFailureEndsTurn; }
            set { this.criticalFailureEndsTurn = value; }
        }
        [D2OIgnore]
        public Boolean HideEffects
        {
            get { return this.hideEffects; }
            set { this.hideEffects = value; }
        }
        [D2OIgnore]
        public Boolean Hidden
        {
            get { return this.hidden; }
            set { this.hidden = value; }
        }
        [D2OIgnore]
        public Boolean PlayAnimation
        {
            get { return this.playAnimation; }
            set { this.playAnimation = value; }
        }
        [D2OIgnore]
        public List<int> StatesRequired
        {
            get { return this.statesRequired; }
            set { this.statesRequired = value; }
        }
        [D2OIgnore]
        public List<int> StatesForbidden
        {
            get { return this.statesForbidden; }
            set { this.statesForbidden = value; }
        }
        [D2OIgnore]
        public List<EffectInstanceDice> Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }
        [D2OIgnore]
        public List<EffectInstanceDice> CriticalEffect
        {
            get { return this.criticalEffect; }
            set { this.criticalEffect = value; }
        }
    }
}