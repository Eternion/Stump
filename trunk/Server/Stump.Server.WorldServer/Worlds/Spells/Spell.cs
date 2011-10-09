using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Spells
{
    public class Spell
    {
        private readonly ISpellRecord m_record;

        public Spell(ISpellRecord record)
        {
            m_record = record;
            Template = SpellManager.Instance.GetSpellTemplate(Id);
            SpellType = SpellManager.Instance.GetSpellType(Template.TypeId);
            int level = 1;
            ByLevel = SpellManager.Instance.GetSpellLevels(Id).ToDictionary(entry => level++);
        }

        #region Properties

        public int Id
        {
            get
            {
                return m_record.SpellId;
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
                return m_record.Level;
            }
            internal set
            {
                m_record.Level = value;
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