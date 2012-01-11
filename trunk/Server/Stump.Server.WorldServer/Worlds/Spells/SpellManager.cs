using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects;

namespace Stump.Server.WorldServer.Worlds.Spells
{
    public class SpellManager : Singleton<SpellManager>
    {
        private Dictionary<uint, SpellLevelTemplate> m_spellsLevels;
        private Dictionary<int, SpellTemplate> m_spells;
        private Dictionary<int, SpellType> m_spellsTypes;

        #region Fields

        #endregion

        [Initialization(typeof(EffectManager))]
        public void Initialize()
        {
            m_spellsLevels = SpellLevelTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_spells = SpellTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_spellsTypes = SpellType.FindAll().ToDictionary(entry => entry.Id);
        }

        public CharacterSpellRecord CreateSpellRecord(CharacterRecord owner, SpellTemplate template)
        {
            return new CharacterSpellRecord
            {
                OwnerId = owner.Id,
                Level = 1,
                Position = 63, // always 63
                SpellId = template.Id
            };
        }

        public SpellTemplate GetSpellTemplate(int id)
        {
            SpellTemplate template;
            if (m_spells.TryGetValue(id, out template))
                return template;

            return null;
        }

        public SpellTemplate GetFirstSpellTemplate(Predicate<SpellTemplate> predicate)
        {
            return m_spells.Values.FirstOrDefault(entry => predicate(entry));
        }

        public IEnumerable<SpellTemplate> GetSpellTemplates()
        {
            return m_spells.Values;
        }

        public SpellLevelTemplate GetSpellLevel(int id)
        {
            SpellLevelTemplate template;
            if (m_spellsLevels.TryGetValue((uint) id, out template))
                return template;

            return null;
        }

        public IEnumerable<SpellLevelTemplate> GetSpellLevels(int id)
        {
            return m_spellsLevels.Values.Where(entry => entry.Spell.Id == id).OrderBy(entry => entry.Id);
        }

        public SpellType GetSpellType(uint id)
        {
            SpellType template;
            if (m_spellsTypes.TryGetValue((int) id, out template))
                return template;

            return null;
        }
    }
}