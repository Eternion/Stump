

// Generated on 09/01/2015 11:16:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Weapon", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class Weapon : Item
    {
        public int apCost;
        public int minRange;
        public int range;
        public uint maxCastPerTurn;
        public Boolean castInLine;
        public Boolean castInDiagonal;
        public Boolean castTestLos;
        public int criticalHitProbability;
        public int criticalHitBonus;
        public int criticalFailureProbability;
        [D2OIgnore]
        public int ApCost
        {
            get { return this.apCost; }
            set { this.apCost = value; }
        }
        [D2OIgnore]
        public int MinRange
        {
            get { return this.minRange; }
            set { this.minRange = value; }
        }
        [D2OIgnore]
        public int Range
        {
            get { return this.range; }
            set { this.range = value; }
        }
        [D2OIgnore]
        public uint MaxCastPerTurn
        {
            get { return this.maxCastPerTurn; }
            set { this.maxCastPerTurn = value; }
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
        public int CriticalHitProbability
        {
            get { return this.criticalHitProbability; }
            set { this.criticalHitProbability = value; }
        }
        [D2OIgnore]
        public int CriticalHitBonus
        {
            get { return this.criticalHitBonus; }
            set { this.criticalHitBonus = value; }
        }
        [D2OIgnore]
        public int CriticalFailureProbability
        {
            get { return this.criticalFailureProbability; }
            set { this.criticalFailureProbability = value; }
        }
    }
}