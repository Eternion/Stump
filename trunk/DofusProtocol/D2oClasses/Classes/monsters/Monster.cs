
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Monsters")]
    [Serializable]
    public class Monster : IDataObject, IIndexedData
    {
        private const String MODULE = "Monsters";
        public int id;
        public uint nameId;
        public uint gfxId;
        public int race;
        public List<MonsterGrade> grades;
        public String look;
        public Boolean useSummonSlot;
        public Boolean useBombSlot;
        public Boolean canPlay;
        public Boolean canTackle;
        public List<AnimFunMonsterData> animFunList;
        public Boolean isBoss;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        public int Race
        {
            get { return race; }
            set { race = value; }
        }

        public List<MonsterGrade> Grades
        {
            get { return grades; }
            set { grades = value; }
        }

        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public Boolean UseSummonSlot
        {
            get { return useSummonSlot; }
            set { useSummonSlot = value; }
        }

        public Boolean UseBombSlot
        {
            get { return useBombSlot; }
            set { useBombSlot = value; }
        }

        public Boolean CanPlay
        {
            get { return canPlay; }
            set { canPlay = value; }
        }

        public Boolean CanTackle
        {
            get { return canTackle; }
            set { canTackle = value; }
        }

        public List<AnimFunMonsterData> AnimFunList
        {
            get { return animFunList; }
            set { animFunList = value; }
        }

        public Boolean IsBoss
        {
            get { return isBoss; }
            set { isBoss = value; }
        }

    }
}