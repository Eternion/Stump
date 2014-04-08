

// Generated on 12/12/2013 16:57:38
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
            get { return apCost; }
            set { apCost = value; }
        }
        [D2OIgnore]
        public int MinRange
        {
            get { return minRange; }
            set { minRange = value; }
        }
        [D2OIgnore]
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        [D2OIgnore]
        public uint MaxCastPerTurn
        {
            get { return maxCastPerTurn; }
            set { maxCastPerTurn = value; }
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
        public int CriticalHitProbability
        {
            get { return criticalHitProbability; }
            set { criticalHitProbability = value; }
        }
        [D2OIgnore]
        public int CriticalHitBonus
        {
            get { return criticalHitBonus; }
            set { criticalHitBonus = value; }
        }
        [D2OIgnore]
        public int CriticalFailureProbability
        {
            get { return criticalFailureProbability; }
            set { criticalFailureProbability = value; }
        }
    }
}