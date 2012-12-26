using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database
{
    public class MonsterGradeConfiguration : EntityTypeConfiguration<MonsterGrade>
    {
        public MonsterGradeConfiguration()
        {
            ToTable("monsters_grades");
        }
    }
    [D2OClass("MonsterGrade", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterGrade : IAssignedByD2O, ISaveIntercepter
    {
        public int Id
        {
            get;
            set;
        }

        public uint GradeId
        {
            get;
            set;
        }

        public int GradeXp
        {
            get;
            set;
        }

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

        public uint Level
        {
            get;
            set;
        }

        public int PaDodge
        {
            get;
            set;
        }

        public int PmDodge
        {
            get;
            set;
        }

        public int EarthResistance
        {
            get;
            set;
        }

        public int AirResistance
        {
            get;
            set;
        }

        public int FireResistance
        {
            get;
            set;
        }

        public int WaterResistance
        {
            get;
            set;
        }

        public int NeutralResistance
        {
            get;
            set;
        }

        public int LifePoints
        {
            get;
            set;
        }

        public int ActionPoints
        {
            get;
            set;
        }

        public int MovementPoints
        {
            get;
            set;
        }

        public short TackleEvade
        {
            get;
            set;
        }

        public short TackleBlock
        {
            get;
            set;
        }

        public short Strength
        {
            get;
            set;
        }

        public short Chance
        {
            get;
            set;
        }

        public short Vitality
        {
            get;
            set;
        }

        private int m_wisdom;

        public short Wisdom
        {
            get { return (short) m_wisdom; }
            set { m_wisdom = value; }
        }

        public short Intelligence
        {
            get;
            set;
        }

        public short Agility
        {
            get;
            set;
        }

        private byte[] m_statsBin;
        private byte[] StatsBin
        {
            get { return m_statsBin; }
            set { m_statsBin = value;

            if (value == null)
                Stats = new Dictionary<PlayerFields, short>();
            else
                Stats = DeserializeStats(value);
            }
        }

        public Dictionary<PlayerFields, short> Stats
        {
            get;
            set;
        }

        private byte[] SerializeStats(Dictionary<PlayerFields, short> stats)
        {
            var serialized = new byte[stats.Count + stats.Count * 2];

            var i = 0;
            foreach (var pair in stats)
            {
                serialized[i] = (byte)pair.Key;

                serialized[i + 1] = (byte) ((pair.Value >> 8) & 0xFF);
                serialized[i + 2] = (byte)( pair.Value & 0xFF );

                i++;
            }

            return serialized;
        }

        private Dictionary<PlayerFields, short> DeserializeStats(byte[] serialized)
        {
            var stats = new Dictionary<PlayerFields, short>();

            for (int i = 0; i < serialized.Length; i += 3)
            {
                stats.Add((PlayerFields)serialized[i], (short)( serialized[i + 1] << 8 | serialized[i + 2] ));
            }

            return stats;
        }

        private List<Monsters.MonsterSpell> m_spells;
        public List<Monsters.MonsterSpell> Spells
        {
            get
            {
                return m_spells ?? ( m_spells = MonsterManager.Instance.GetMonsterGradeSpells(Id) );
            }
        }

        public void AssignFields(object d2oObject)
        {
            var grade = (DofusProtocol.D2oClasses.MonsterGrade)d2oObject;
            GradeId = grade.grade;
            GradeXp = grade.gradeXp;
            MonsterId = grade.monsterId;
            Level = grade.level;
            PaDodge = grade.paDodge;
            PmDodge = grade.pmDodge;
            EarthResistance = grade.earthResistance;
            AirResistance = grade.airResistance;
            FireResistance = grade.fireResistance;
            WaterResistance = grade.waterResistance;
            NeutralResistance = grade.neutralResistance;
            LifePoints = grade.lifePoints;
            ActionPoints = grade.actionPoints;
            MovementPoints = grade.movementPoints;
            Wisdom = (short) grade.wisdom;
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_statsBin = SerializeStats(Stats);
        }
    }
}