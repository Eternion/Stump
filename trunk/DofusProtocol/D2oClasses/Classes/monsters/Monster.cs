

// Generated on 10/06/2013 17:58:55
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Monster", "com.ankamagames.dofus.datacenter.monsters")]
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }
        [D2OIgnore]
        public int Race
        {
            get { return race; }
            set { race = value; }
        }
        [D2OIgnore]
        public List<MonsterGrade> Grades
        {
            get { return grades; }
            set { grades = value; }
        }
        [D2OIgnore]
        public String Look
        {
            get { return look; }
            set { look = value; }
        }
        [D2OIgnore]
        public Boolean UseSummonSlot
        {
            get { return useSummonSlot; }
            set { useSummonSlot = value; }
        }
        [D2OIgnore]
        public Boolean UseBombSlot
        {
            get { return useBombSlot; }
            set { useBombSlot = value; }
        }
        [D2OIgnore]
        public Boolean CanPlay
        {
            get { return canPlay; }
            set { canPlay = value; }
        }
        [D2OIgnore]
        public Boolean CanTackle
        {
            get { return canTackle; }
            set { canTackle = value; }
        }
        [D2OIgnore]
        public List<AnimFunMonsterData> AnimFunList
        {
            get { return animFunList; }
            set { animFunList = value; }
        }
        [D2OIgnore]
        public Boolean IsBoss
        {
            get { return isBoss; }
            set { isBoss = value; }
        }
    }
}