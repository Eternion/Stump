
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Weapon")]
    [Serializable]
    public class Weapon : Item, IIndexedData
    {
        public int apCost;
        public int minRange;
        public int range;
        public Boolean castInLine;
        public Boolean castInDiagonal;
        public Boolean castTestLos;
        public int criticalHitProbability;
        public int criticalHitBonus;
        public int criticalFailureProbability;

        public int ApCost
        {
            get { return apCost; }
            set { apCost = value; }
        }

        public int MinRange
        {
            get { return minRange; }
            set { minRange = value; }
        }

        public int Range
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

        public int CriticalHitProbability
        {
            get { return criticalHitProbability; }
            set { criticalHitProbability = value; }
        }

        public int CriticalHitBonus
        {
            get { return criticalHitBonus; }
            set { criticalHitBonus = value; }
        }

        public int CriticalFailureProbability
        {
            get { return criticalFailureProbability; }
            set { criticalFailureProbability = value; }
        }

    }
}