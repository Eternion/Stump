

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterGrade", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterGrade : IDataObject, IIndexedData
    {
        public uint grade;
        public int monsterId;
        public uint level;
        public int paDodge;
        public int pmDodge;
        public int wisdom;
        public int earthResistance;
        public int airResistance;
        public int fireResistance;
        public int waterResistance;
        public int neutralResistance;
        public int gradeXp;
        public int lifePoints;
        public int actionPoints;
        public int movementPoints;
        int IIndexedData.Id
        {
            get { return (int)monsterId; }
        }
        [D2OIgnore]
        public uint Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        [D2OIgnore]
        public int MonsterId
        {
            get { return monsterId; }
            set { monsterId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }
        [D2OIgnore]
        public int PaDodge
        {
            get { return paDodge; }
            set { paDodge = value; }
        }
        [D2OIgnore]
        public int PmDodge
        {
            get { return pmDodge; }
            set { pmDodge = value; }
        }
        [D2OIgnore]
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = value; }
        }
        [D2OIgnore]
        public int EarthResistance
        {
            get { return earthResistance; }
            set { earthResistance = value; }
        }
        [D2OIgnore]
        public int AirResistance
        {
            get { return airResistance; }
            set { airResistance = value; }
        }
        [D2OIgnore]
        public int FireResistance
        {
            get { return fireResistance; }
            set { fireResistance = value; }
        }
        [D2OIgnore]
        public int WaterResistance
        {
            get { return waterResistance; }
            set { waterResistance = value; }
        }
        [D2OIgnore]
        public int NeutralResistance
        {
            get { return neutralResistance; }
            set { neutralResistance = value; }
        }
        [D2OIgnore]
        public int GradeXp
        {
            get { return gradeXp; }
            set { gradeXp = value; }
        }
        [D2OIgnore]
        public int LifePoints
        {
            get { return lifePoints; }
            set { lifePoints = value; }
        }
        [D2OIgnore]
        public int ActionPoints
        {
            get { return actionPoints; }
            set { actionPoints = value; }
        }
        [D2OIgnore]
        public int MovementPoints
        {
            get { return movementPoints; }
            set { movementPoints = value; }
        }
    }
}