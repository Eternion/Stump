using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;

namespace Stump.Server.WorldServer.Game.Spells
{
    public class Spell
    {
        private readonly ISpellRecord m_record;
        private readonly int m_id;
        private sbyte m_level;

        public Spell(ISpellRecord record)
        {
            m_record = record;
            m_id = m_record.SpellId;
            m_level = m_record.Level;

            Template = SpellManager.Instance.GetSpellTemplate(Id);
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            int counter = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Id).ToDictionary(entry => counter++);
        }

        public Spell(int id, sbyte level)
        {
            m_id = id;
            m_level = level;

            Template = SpellManager.Instance.GetSpellTemplate(Id);
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            int counter = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Id).ToDictionary(entry => counter++);
        }

        #region Properties

        public int Id
        {
            get
            {
                return m_id;
            }
        }

        public SpellTemplate Template
        {
            get;
            private set;
        }

        public SpellType SpellType
        {
            get;
            private set;
        }

        public sbyte CurrentLevel
        {
            get
            {
                return m_level;
            }
            internal set
            {
                m_record.Level = value;
                m_level = value;
            }
        }

        public SpellLevelTemplate CurrentSpellLevel
        {
            get
            {
                return !ByLevel.ContainsKey(CurrentLevel) ? ByLevel[1] : ByLevel[CurrentLevel];
            }
        }

        public byte Position
        {
            get
            {
                return 63; // always 63 ?
            }
        }

        public Dictionary<int, SpellLevelTemplate> ByLevel
        {
            get;
            private set;
        }

        #endregion

        public SpellItem GetSpellItem()
        {
            return new SpellItem(Position, Id, CurrentLevel);
        }
    }
}