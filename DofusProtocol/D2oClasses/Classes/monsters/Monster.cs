

// Generated on 02/19/2015 12:10:48
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
        public const String MODULE = "Monsters";
        public int id;
        [I18NField]
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
        public List<MonsterDrop> drops;
        public List<uint> subareas;
        public List<uint> spells;
        public int favoriteSubareaId;
        public Boolean isMiniBoss;
        public Boolean isQuestMonster;
        public uint correspondingMiniBossId;
        public double speedAdjust = 0.0;
        public int creatureBoneId;
        public Boolean canBePushed;
        public Boolean fastAnimsFun;
        public Boolean canSwitchPos;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint GfxId
        {
            get { return this.gfxId; }
            set { this.gfxId = value; }
        }
        [D2OIgnore]
        public int Race
        {
            get { return this.race; }
            set { this.race = value; }
        }
        [D2OIgnore]
        public List<MonsterGrade> Grades
        {
            get { return this.grades; }
            set { this.grades = value; }
        }
        [D2OIgnore]
        public String Look
        {
            get { return this.look; }
            set { this.look = value; }
        }
        [D2OIgnore]
        public Boolean UseSummonSlot
        {
            get { return this.useSummonSlot; }
            set { this.useSummonSlot = value; }
        }
        [D2OIgnore]
        public Boolean UseBombSlot
        {
            get { return this.useBombSlot; }
            set { this.useBombSlot = value; }
        }
        [D2OIgnore]
        public Boolean CanPlay
        {
            get { return this.canPlay; }
            set { this.canPlay = value; }
        }
        [D2OIgnore]
        public Boolean CanTackle
        {
            get { return this.canTackle; }
            set { this.canTackle = value; }
        }
        [D2OIgnore]
        public List<AnimFunMonsterData> AnimFunList
        {
            get { return this.animFunList; }
            set { this.animFunList = value; }
        }
        [D2OIgnore]
        public Boolean IsBoss
        {
            get { return this.isBoss; }
            set { this.isBoss = value; }
        }
        [D2OIgnore]
        public List<MonsterDrop> Drops
        {
            get { return this.drops; }
            set { this.drops = value; }
        }
        [D2OIgnore]
        public List<uint> Subareas
        {
            get { return this.subareas; }
            set { this.subareas = value; }
        }
        [D2OIgnore]
        public List<uint> Spells
        {
            get { return this.spells; }
            set { this.spells = value; }
        }
        [D2OIgnore]
        public int FavoriteSubareaId
        {
            get { return this.favoriteSubareaId; }
            set { this.favoriteSubareaId = value; }
        }
        [D2OIgnore]
        public Boolean IsMiniBoss
        {
            get { return this.isMiniBoss; }
            set { this.isMiniBoss = value; }
        }
        [D2OIgnore]
        public Boolean IsQuestMonster
        {
            get { return this.isQuestMonster; }
            set { this.isQuestMonster = value; }
        }
        [D2OIgnore]
        public uint CorrespondingMiniBossId
        {
            get { return this.correspondingMiniBossId; }
            set { this.correspondingMiniBossId = value; }
        }
        [D2OIgnore]
        public double SpeedAdjust
        {
            get { return this.speedAdjust; }
            set { this.speedAdjust = value; }
        }
        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
        [D2OIgnore]
        public Boolean CanBePushed
        {
            get { return this.canBePushed; }
            set { this.canBePushed = value; }
        }
        [D2OIgnore]
        public Boolean FastAnimsFun
        {
            get { return this.fastAnimsFun; }
            set { this.fastAnimsFun = value; }
        }
        [D2OIgnore]
        public Boolean CanSwitchPos
        {
            get { return this.canSwitchPos; }
            set { this.canSwitchPos = value; }
        }
    }
}