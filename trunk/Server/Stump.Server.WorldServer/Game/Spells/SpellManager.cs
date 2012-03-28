using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Spells
{
    public class SpellManager : Singleton<SpellManager>
    {
        private Dictionary<uint, SpellLevelTemplate> m_spellsLevels;
        private Dictionary<int, SpellTemplate> m_spells;
        private Dictionary<int, SpellType> m_spellsTypes;
        private Dictionary<int, SpellState> m_spellsState;

        private delegate SpellCastHandler SpellCastConstructor(FightActor caster, Spell spell, Cell targetedCell, bool critical);

        private readonly Dictionary<int, SpellCastConstructor> m_spellsCastHandler = new Dictionary<int, SpellCastConstructor>();

            #region Fields

        #endregion

        [Initialization(typeof(EffectManager))]
        public void Initialize()
        {
            m_spellsLevels = SpellLevelTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_spells = SpellTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_spellsTypes = SpellType.FindAll().ToDictionary(entry => entry.Id);
            m_spellsState = SpellState.FindAll().ToDictionary(entry => entry.Id);
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

        public SpellTemplate GetSpellTemplate(string name, bool ignorecase)
        {
            return
                m_spells.Values.Where(
                    entry =>
                    entry.Name.Equals(name,
                                      ignorecase
                                          ? StringComparison.InvariantCultureIgnoreCase
                                          : StringComparison.InvariantCulture)).FirstOrDefault();
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

        public SpellState GetSpellState(uint id)
        {
            SpellState state;
            if (m_spellsState.TryGetValue((int)id, out state))
                return state;

            return null;
        }

        public void AddSpellCastHandler(SpellEffectHandler handler, SpellTemplate spell)
        {
            var type = handler.GetType();
            var ctor = type.GetConstructor(new[] { typeof(FightActor), typeof(Spell), typeof(Cell), typeof(bool) });

            if (ctor == null)
                throw new Exception("No valid constructors found ! ");

            m_spellsCastHandler.Add(spell.Id, ctor.CreateDelegate<SpellCastConstructor>());
        }

        public SpellCastHandler GetSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
        {
            SpellCastConstructor ctor;
            if (m_spellsCastHandler.TryGetValue(spell.Template.Id, out ctor))
            {
                return ctor(caster, spell, targetedCell, critical);
            }

            return new DefaultSpellCastHandler(caster, spell, targetedCell, critical);
        }
    }
}