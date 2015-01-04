 


// Generated on 01/04/2015 01:23:46
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Weapons")]
    [D2OClass("Weapon", "com.ankamagames.dofus.datacenter.items")]
    public class WeaponRecord : ItemRecord, ID2ORecord, ISaveIntercepter
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

        public override void AssignFields(object obj)
        {
            var castedObj = (Weapon)obj;
            
            base.AssignFields(obj);
            ApCost = castedObj.apCost;
            MinRange = castedObj.minRange;
            Range = castedObj.range;
            MaxCastPerTurn = castedObj.maxCastPerTurn;
            CastInLine = castedObj.castInLine;
            CastInDiagonal = castedObj.castInDiagonal;
            CastTestLos = castedObj.castTestLos;
            CriticalHitProbability = castedObj.criticalHitProbability;
            CriticalHitBonus = castedObj.criticalHitBonus;
            CriticalFailureProbability = castedObj.criticalFailureProbability;
        }
        
        public override object CreateObject(object parent = null)
        {
            var obj = new Weapon();
            base.CreateObject(obj);
            obj.apCost = ApCost;
            obj.minRange = MinRange;
            obj.range = Range;
            obj.maxCastPerTurn = MaxCastPerTurn;
            obj.castInLine = CastInLine;
            obj.castInDiagonal = CastInDiagonal;
            obj.castTestLos = CastTestLos;
            obj.criticalHitProbability = CriticalHitProbability;
            obj.criticalHitBonus = CriticalHitBonus;
            obj.criticalFailureProbability = CriticalFailureProbability;
            return obj;
        }
        
        public override void BeforeSave(bool insert)
        {
            base.BeforeSave(insert);
        
        }
    }
}