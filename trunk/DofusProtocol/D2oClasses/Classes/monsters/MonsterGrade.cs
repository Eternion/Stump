
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterGrade")]
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

        public uint Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public int MonsterId
        {
            get { return monsterId; }
            set { monsterId = value; }
        }

        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        public int PaDodge
        {
            get { return paDodge; }
            set { paDodge = value; }
        }

        public int PmDodge
        {
            get { return pmDodge; }
            set { pmDodge = value; }
        }

        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = value; }
        }

        public int EarthResistance
        {
            get { return earthResistance; }
            set { earthResistance = value; }
        }

        public int AirResistance
        {
            get { return airResistance; }
            set { airResistance = value; }
        }

        public int FireResistance
        {
            get { return fireResistance; }
            set { fireResistance = value; }
        }

        public int WaterResistance
        {
            get { return waterResistance; }
            set { waterResistance = value; }
        }

        public int NeutralResistance
        {
            get { return neutralResistance; }
            set { neutralResistance = value; }
        }

        public int GradeXp
        {
            get { return gradeXp; }
            set { gradeXp = value; }
        }

        public int LifePoints
        {
            get { return lifePoints; }
            set { lifePoints = value; }
        }

        public int ActionPoints
        {
            get { return actionPoints; }
            set { actionPoints = value; }
        }

        public int MovementPoints
        {
            get { return movementPoints; }
            set { movementPoints = value; }
        }

    }
}