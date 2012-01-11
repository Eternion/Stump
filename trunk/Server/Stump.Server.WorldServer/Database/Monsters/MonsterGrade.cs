using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_grades")]
    [D2OClass("MonsterGrade", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterGrade : WorldBaseRecord<MonsterGrade>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [D2OField("grade")]
        [Property("Grade")]
        public uint GradeId
        {
            get;
            set;
        }

        [D2OField("gradeXp")]
        [Property]
        public int GradeXp
        {
            get;
            set;
        }

        [D2OField("monsterId")]
        [Property("MonsterId")]
        public int MonsterId
        {
            get;
            set;
        }

        private MonsterTemplate m_template;
        public MonsterTemplate Template
        {
            get
            {
                return m_template ?? ( m_template = MonsterManager.Instance.GetTemplate(MonsterId) );
            }
            set
            {
                m_template = value;
                MonsterId = value.Id;
            }
        }

        [D2OField("level")]
        [Property("Level")]
        public uint Level
        {
            get;
            set;
        }

        [D2OField("paDodge")]
        [Property("PaDodge")]
        public int PaDodge
        {
            get;
            set;
        }

        [D2OField("pmDodge")]
        [Property("PmDodge")]
        public int PmDodge
        {
            get;
            set;
        }

        [D2OField("earthResistance")]
        [Property("EarthResistance")]
        public int EarthResistance
        {
            get;
            set;
        }

        [D2OField("airResistance")]
        [Property("AirResistance")]
        public int AirResistance
        {
            get;
            set;
        }

        [D2OField("fireResistance")]
        [Property("FireResistance")]
        public int FireResistance
        {
            get;
            set;
        }

        [D2OField("waterResistance")]
        [Property("WaterResistance")]
        public int WaterResistance
        {
            get;
            set;
        }

        [D2OField("neutralResistance")]
        [Property("NeutralResistance")]
        public int NeutralResistance
        {
            get;
            set;
        }

        [D2OField("lifePoints")]
        [Property("LifePoints")]
        public int LifePoints
        {
            get;
            set;
        }

        [D2OField("actionPoints")]
        [Property("ActionPoints")]
        public int ActionPoints
        {
            get;
            set;
        }

        [D2OField("movementPoints")]
        [Property("MovementPoints")]
        public int MovementPoints
        {
            get;
            set;
        }

        [Property("Strength")]
        public short Strength
        {
            get;
            set;
        }

        [Property("Chance")]
        public short Chance
        {
            get;
            set;
        }

        [Property("Vitality")]
        public short Vitality
        {
            get;
            set;
        }

        [D2OField("wisdom")]
        [Field("Wisdom")]
        private int m_wisdom;

        public short Wisdom
        {
            get { return (short) m_wisdom; }
            set { m_wisdom = value; }
        }

        [Property("Intelligence")]
        public short Intelligence
        {
            get;
            set;
        }

        [Property("Agility")]
        public short Agility
        {
            get;
            set;
        }

        private List<MonsterSpell> m_spells;
        public List<MonsterSpell> Spells
        {
            get
            {
                return m_spells ?? ( m_spells = MonsterManager.Instance.GetMonsterGradeSpells(Id) );
            }
        }
    }
}