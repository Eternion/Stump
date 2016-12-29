using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using SpellType = Stump.Server.WorldServer.Database.Spells.SpellType;

namespace Stump.Server.WorldServer.Game.Spells
{
    public class Spell
    {
        private readonly ISpellRecord m_record;
        private readonly int m_id;
        private byte m_level;
        private SpellLevelTemplate m_currentLevel;
        private int? m_maxLevel;

        public Spell(ISpellRecord record)
        {
            m_record = record;
            m_id = m_record.SpellId;
            m_level = (byte) m_record.Level;

            Template = SpellManager.Instance.GetSpellTemplate(Id);
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            var counter = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Template).ToDictionary(entry => counter++);
        }

        public Spell(int id, byte level)
        {
            m_id = id;
            m_level = level;

            Template = SpellManager.Instance.GetSpellTemplate(Id);
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            var counter = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Template).ToDictionary(entry => counter++);
        }        
        
        public Spell(SpellTemplate template, byte level)
        {
            m_id = template.Id;
            m_level = level;

            Template = template;
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            var counter = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Template).ToDictionary(entry => counter++);
        }


        #region Properties

        public int Id => m_id;

        public SpellTemplate Template
        {
            get;
        }

        public SpellType SpellType
        {
            get;
        }

        public int MaxLevel => (m_maxLevel ?? (m_maxLevel = ByLevel.Keys.Max())).Value;

        public byte CurrentLevel
        {
            get
            {
                return ByLevel.ContainsKey(m_level) ? m_level : (byte)MaxLevel;
            }
            set
            {            
                if (m_record != null)
                    m_record.Level = value;

                m_level = value;
                m_currentLevel = !ByLevel.ContainsKey(CurrentLevel) ? ByLevel[MaxLevel] : ByLevel[CurrentLevel];
            }
        }

        public SpellLevelTemplate CurrentSpellLevel => m_currentLevel ?? (m_currentLevel = !ByLevel.ContainsKey(CurrentLevel) ? ByLevel[MaxLevel] : ByLevel[CurrentLevel]);

        public byte Position => 63;

        public Dictionary<int, SpellLevelTemplate> ByLevel
        {
            get;
        }

        #endregion

        public bool CanBoostSpell() => ByLevel.ContainsKey(CurrentLevel + 1);

        public bool BoostSpell()
        {
            if (!CanBoostSpell())
                return false;

            m_level++;
            if (m_record != null)
                m_record.Level = m_level;
            m_currentLevel = ByLevel[m_level];
            return true;
        }

        public bool UnBoostSpell()
        {
            if (!ByLevel.ContainsKey(CurrentLevel - 1))
                return false;

            m_level--;
            if (m_record != null)
                m_record.Level = m_level;
            m_currentLevel = ByLevel[m_level];
            return true;
        }

        public SpellItem GetSpellItem() => new SpellItem(Id, (sbyte)CurrentLevel);

        public override string ToString() => string.Format("{0} ({1}) (lvl:{2})", Template.Name, Template.Id, CurrentLevel);
    }
}