using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_grades")]
    [D2OClass("MonsterGrade", "com.ankamagames.dofus.datacenter.monsters", AutoBuild = false)]
    public class MonsterGrade : WorldBaseRecord<MonsterGrade>
    {
        private IList<MonsterSpell> m_spells;

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
        [BelongsTo("MonsterId")]
        public MonsterTemplate Template
        {
            get;
            set;
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

        [Property("Wisdom")]
        public short Wisdom
        {
            get;
            set;
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

        [HasMany(typeof (MonsterSpell), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<MonsterSpell> Spells
        {
            get { return m_spells ?? new List<MonsterSpell>(); }
            set { m_spells = value; }
        }
    }
}