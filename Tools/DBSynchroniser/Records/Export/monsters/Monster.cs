 


// Generated on 11/16/2015 14:26:41
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
    [TableName("Monsters")]
    [D2OClass("Monster", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterRecord : ID2ORecord, ISaveIntercepter
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
        public List<uint> incompatibleIdols;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        [Ignore]
        public List<MonsterGrade> Grades
        {
            get { return grades; }
            set
            {
                grades = value;
                m_gradesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_gradesBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] GradesBin
        {
            get { return m_gradesBin; }
            set
            {
                m_gradesBin = value;
                grades = value == null ? null : value.ToObject<List<MonsterGrade>>();
            }
        }

        [D2OIgnore]
        [NullString]
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
        [Ignore]
        public List<AnimFunMonsterData> AnimFunList
        {
            get { return animFunList; }
            set
            {
                animFunList = value;
                m_animFunListBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_animFunListBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] AnimFunListBin
        {
            get { return m_animFunListBin; }
            set
            {
                m_animFunListBin = value;
                animFunList = value == null ? null : value.ToObject<List<AnimFunMonsterData>>();
            }
        }

        [D2OIgnore]
        public Boolean IsBoss
        {
            get { return isBoss; }
            set { isBoss = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<MonsterDrop> Drops
        {
            get { return drops; }
            set
            {
                drops = value;
                m_dropsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_dropsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] DropsBin
        {
            get { return m_dropsBin; }
            set
            {
                m_dropsBin = value;
                drops = value == null ? null : value.ToObject<List<MonsterDrop>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Subareas
        {
            get { return subareas; }
            set
            {
                subareas = value;
                m_subareasBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_subareasBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SubareasBin
        {
            get { return m_subareasBin; }
            set
            {
                m_subareasBin = value;
                subareas = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Spells
        {
            get { return spells; }
            set
            {
                spells = value;
                m_spellsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_spellsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SpellsBin
        {
            get { return m_spellsBin; }
            set
            {
                m_spellsBin = value;
                spells = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public int FavoriteSubareaId
        {
            get { return favoriteSubareaId; }
            set { favoriteSubareaId = value; }
        }

        [D2OIgnore]
        public Boolean IsMiniBoss
        {
            get { return isMiniBoss; }
            set { isMiniBoss = value; }
        }

        [D2OIgnore]
        public Boolean IsQuestMonster
        {
            get { return isQuestMonster; }
            set { isQuestMonster = value; }
        }

        [D2OIgnore]
        public uint CorrespondingMiniBossId
        {
            get { return correspondingMiniBossId; }
            set { correspondingMiniBossId = value; }
        }

        [D2OIgnore]
        public double SpeedAdjust
        {
            get { return speedAdjust; }
            set { speedAdjust = value; }
        }

        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return creatureBoneId; }
            set { creatureBoneId = value; }
        }

        [D2OIgnore]
        public Boolean CanBePushed
        {
            get { return canBePushed; }
            set { canBePushed = value; }
        }

        [D2OIgnore]
        public Boolean FastAnimsFun
        {
            get { return fastAnimsFun; }
            set { fastAnimsFun = value; }
        }

        [D2OIgnore]
        public Boolean CanSwitchPos
        {
            get { return canSwitchPos; }
            set { canSwitchPos = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> IncompatibleIdols
        {
            get { return incompatibleIdols; }
            set
            {
                incompatibleIdols = value;
                m_incompatibleIdolsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_incompatibleIdolsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] IncompatibleIdolsBin
        {
            get { return m_incompatibleIdolsBin; }
            set
            {
                m_incompatibleIdolsBin = value;
                incompatibleIdols = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Monster)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            GfxId = castedObj.gfxId;
            Race = castedObj.race;
            Grades = castedObj.grades;
            Look = castedObj.look;
            UseSummonSlot = castedObj.useSummonSlot;
            UseBombSlot = castedObj.useBombSlot;
            CanPlay = castedObj.canPlay;
            CanTackle = castedObj.canTackle;
            AnimFunList = castedObj.animFunList;
            IsBoss = castedObj.isBoss;
            Drops = castedObj.drops;
            Subareas = castedObj.subareas;
            Spells = castedObj.spells;
            FavoriteSubareaId = castedObj.favoriteSubareaId;
            IsMiniBoss = castedObj.isMiniBoss;
            IsQuestMonster = castedObj.isQuestMonster;
            CorrespondingMiniBossId = castedObj.correspondingMiniBossId;
            SpeedAdjust = castedObj.speedAdjust;
            CreatureBoneId = castedObj.creatureBoneId;
            CanBePushed = castedObj.canBePushed;
            FastAnimsFun = castedObj.fastAnimsFun;
            CanSwitchPos = castedObj.canSwitchPos;
            IncompatibleIdols = castedObj.incompatibleIdols;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Monster)parent : new Monster();
            obj.id = Id;
            obj.nameId = NameId;
            obj.gfxId = GfxId;
            obj.race = Race;
            obj.grades = Grades;
            obj.look = Look;
            obj.useSummonSlot = UseSummonSlot;
            obj.useBombSlot = UseBombSlot;
            obj.canPlay = CanPlay;
            obj.canTackle = CanTackle;
            obj.animFunList = AnimFunList;
            obj.isBoss = IsBoss;
            obj.drops = Drops;
            obj.subareas = Subareas;
            obj.spells = Spells;
            obj.favoriteSubareaId = FavoriteSubareaId;
            obj.isMiniBoss = IsMiniBoss;
            obj.isQuestMonster = IsQuestMonster;
            obj.correspondingMiniBossId = CorrespondingMiniBossId;
            obj.speedAdjust = SpeedAdjust;
            obj.creatureBoneId = CreatureBoneId;
            obj.canBePushed = CanBePushed;
            obj.fastAnimsFun = FastAnimsFun;
            obj.canSwitchPos = CanSwitchPos;
            obj.incompatibleIdols = IncompatibleIdols;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_gradesBin = grades == null ? null : grades.ToBinary();
            m_animFunListBin = animFunList == null ? null : animFunList.ToBinary();
            m_dropsBin = drops == null ? null : drops.ToBinary();
            m_subareasBin = subareas == null ? null : subareas.ToBinary();
            m_spellsBin = spells == null ? null : spells.ToBinary();
            m_incompatibleIdolsBin = incompatibleIdols == null ? null : incompatibleIdols.ToBinary();
        
        }
    }
}