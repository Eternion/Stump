 


// Generated on 10/06/2013 14:21:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Weapons")]
    [D2OClass("Weapon")]
    public class WeaponRecord : ItemRecord, ID2ORecord
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

        public override void AssignFields(object obj)
        {
            var castedObj = (Weapon)obj;

            base.AssignFields(obj);
            ApCost = castedObj.apCost;
            MinRange = castedObj.minRange;
            Range = castedObj.range;
            CastInLine = castedObj.castInLine;
            CastInDiagonal = castedObj.castInDiagonal;
            CastTestLos = castedObj.castTestLos;
            CriticalHitProbability = castedObj.criticalHitProbability;
            CriticalHitBonus = castedObj.criticalHitBonus;
            CriticalFailureProbability = castedObj.criticalFailureProbability;
        }
        
        public override object CreateObject()
        {
            
            var obj = (Weapon)base.CreateObject();
            obj.apCost = ApCost;
            obj.minRange = MinRange;
            obj.range = Range;
            obj.castInLine = CastInLine;
            obj.castInDiagonal = CastInDiagonal;
            obj.castTestLos = CastTestLos;
            obj.criticalHitProbability = CriticalHitProbability;
            obj.criticalHitBonus = CriticalHitBonus;
            obj.criticalFailureProbability = CriticalFailureProbability;
            return obj;
        
        }
    }
}