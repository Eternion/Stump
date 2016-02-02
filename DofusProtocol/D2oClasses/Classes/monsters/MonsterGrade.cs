

// Generated on 02/02/2016 14:15:05
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
        public int damageReflect;
        public uint hiddenLevel;
        int IIndexedData.Id
        {
            get { return (int)monsterId; }
        }
        [D2OIgnore]
        public uint Grade
        {
            get { return this.grade; }
            set { this.grade = value; }
        }
        [D2OIgnore]
        public int MonsterId
        {
            get { return this.monsterId; }
            set { this.monsterId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
        [D2OIgnore]
        public int PaDodge
        {
            get { return this.paDodge; }
            set { this.paDodge = value; }
        }
        [D2OIgnore]
        public int PmDodge
        {
            get { return this.pmDodge; }
            set { this.pmDodge = value; }
        }
        [D2OIgnore]
        public int Wisdom
        {
            get { return this.wisdom; }
            set { this.wisdom = value; }
        }
        [D2OIgnore]
        public int EarthResistance
        {
            get { return this.earthResistance; }
            set { this.earthResistance = value; }
        }
        [D2OIgnore]
        public int AirResistance
        {
            get { return this.airResistance; }
            set { this.airResistance = value; }
        }
        [D2OIgnore]
        public int FireResistance
        {
            get { return this.fireResistance; }
            set { this.fireResistance = value; }
        }
        [D2OIgnore]
        public int WaterResistance
        {
            get { return this.waterResistance; }
            set { this.waterResistance = value; }
        }
        [D2OIgnore]
        public int NeutralResistance
        {
            get { return this.neutralResistance; }
            set { this.neutralResistance = value; }
        }
        [D2OIgnore]
        public int GradeXp
        {
            get { return this.gradeXp; }
            set { this.gradeXp = value; }
        }
        [D2OIgnore]
        public int LifePoints
        {
            get { return this.lifePoints; }
            set { this.lifePoints = value; }
        }
        [D2OIgnore]
        public int ActionPoints
        {
            get { return this.actionPoints; }
            set { this.actionPoints = value; }
        }
        [D2OIgnore]
        public int MovementPoints
        {
            get { return this.movementPoints; }
            set { this.movementPoints = value; }
        }
        [D2OIgnore]
        public int DamageReflect
        {
            get { return this.damageReflect; }
            set { this.damageReflect = value; }
        }
        [D2OIgnore]
        public uint HiddenLevel
        {
            get { return this.hiddenLevel; }
            set { this.hiddenLevel = value; }
        }
    }
}