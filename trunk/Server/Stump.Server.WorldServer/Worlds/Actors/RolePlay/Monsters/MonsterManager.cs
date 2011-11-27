using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Monsters;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters
{
    public class MonsterManager : Singleton<MonsterManager>
    {
        private Dictionary<int, MonsterTemplate> m_monsterTemplates;
        private Dictionary<int, MonsterSpell> m_monsterSpells;
        private Dictionary<int, MonsterSpawn> m_monsterSpawns;

        [Initialization(InitializationPass.Sixth)]
        public void Initialize()
        {
            m_monsterTemplates = MonsterTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_monsterSpells = MonsterSpell.FindAll().ToDictionary(entry => entry.Id);
            m_monsterSpawns = MonsterSpawn.FindAll().ToDictionary(entry => entry.Id);
        }

        public MonsterGrade GetMonsterGrade(int monsterId, int grade)
        {
            var template = GetTemplate(monsterId);

            if (template.Grades.Count <= grade)
                return null;

            return template.Grades[grade];
        }

        public MonsterTemplate GetTemplate(int id)
        {
            MonsterTemplate result;
            if (!m_monsterTemplates.TryGetValue(id, out result))
                return null;

            return result;
        }

        public MonsterTemplate GetTemplate(string name, bool ignoreCommandCase)
        {
            return
                m_monsterTemplates.Values.Where(
                    entry =>
                    entry.Name.Equals(name,
                                      ignoreCommandCase
                                          ? StringComparison.InvariantCultureIgnoreCase
                                          : StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public MonsterSpell GetOneMonsterSpell(Predicate<MonsterSpell> predicate)
        {
            return m_monsterSpells.Values.SingleOrDefault(entry => predicate(entry));
        }

        public void AddMonsterSpell(MonsterSpell spell)
        {
            spell.Save();
            m_monsterSpells.Add(spell.Id, spell);
        }

        public IEnumerable<MonsterSpawn> GetMonsterSpawns()
        {
            return m_monsterSpawns.Values;
        }

        public MonsterSpawn GetOneMonsterSpawn(Predicate<MonsterSpawn> predicate)
        {
            return m_monsterSpawns.Values.SingleOrDefault(entry => predicate(entry));
        }

        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            spawn.Save();
            m_monsterSpawns.Add(spawn.Id, spawn);
        }
    }
}