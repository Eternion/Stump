 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Monsters")]
    public class MonsterRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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
        public byte[] GradesBin
        {
            get { return m_gradesBin; }
            set
            {
                m_gradesBin = value;
                grades = value == null ? null : value.ToObject<List<MonsterGrade>>();
            }
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
        public byte[] AnimFunListBin
        {
            get { return m_animFunListBin; }
            set
            {
                m_animFunListBin = value;
                animFunList = value == null ? null : value.ToObject<List<AnimFunMonsterData>>();
            }
        }

        public Boolean IsBoss
        {
            get { return isBoss; }
            set { isBoss = value; }
        }

        public void AssignFields(object obj)
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
        }
        
        public object CreateObject()
        {
            var obj = new Monster();
            
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
            return obj;
        
        }
    }
}