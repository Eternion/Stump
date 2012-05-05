using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Monsters;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public class MonsterManager : Singleton<MonsterManager>
    {
        private Dictionary<int, MonsterTemplate> m_monsterTemplates;
        private Dictionary<int, MonsterSpell> m_monsterSpells;
        private Dictionary<int, MonsterSpawn> m_monsterSpawns;
        private Dictionary<int, MonsterDungeonSpawn> m_monsterDungeonsSpawns;
        private Dictionary<int, DroppableItem> m_droppableItems;
        private Dictionary<int, MonsterGrade> m_monsterGrades;
        private Dictionary<int, MonsterSuperRace> m_monsterSuperRaces;

        [Initialization(InitializationPass.Sixth)]
        public void Initialize()
        {
            m_monsterTemplates = MonsterTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_monsterGrades = MonsterGrade.FindAll().ToDictionary(entry => entry.Id);
            m_monsterSpells = MonsterSpell.FindAll().ToDictionary(entry => entry.Id);
            m_monsterSpawns = MonsterSpawn.FindAll().ToDictionary(entry => entry.Id);
            m_monsterDungeonsSpawns = MonsterDungeonSpawn.FindAll().ToDictionary(entry => entry.Id);
            m_droppableItems = DroppableItem.FindAll().ToDictionary(entry => entry.Id);
            m_monsterSuperRaces = MonsterSuperRace.FindAll().ToDictionary(entry => entry.Id);
        }

        public MonsterGrade[] GetMonsterGrades()
        {
            return m_monsterGrades.Values.ToArray();
        }

        public MonsterGrade GetMonsterGrade(int id)
        {
            MonsterGrade result;
            if (!m_monsterGrades.TryGetValue(id, out result))
                return null;

            return result;
        }

        public MonsterGrade GetMonsterGrade(int monsterId, int grade)
        {
            var template = GetTemplate(monsterId);

            if (template.Grades.Count <= grade - 1)
                return null;

            return template.Grades[grade - 1];
        }

        public List<MonsterGrade> GetMonsterGrades(int monsterId)
        {
            return m_monsterGrades.Where(entry => entry.Value.MonsterId == monsterId).Select(entry => entry.Value).ToList();
        }

        public List<MonsterSpell> GetMonsterGradeSpells(int id)
        {
            return m_monsterSpells.Where(entry => entry.Value.MonsterGradeId == id).Select(entry => entry.Value).ToList();
        }

        public List<DroppableItem> GetMonsterDroppableItems(int id)
        {
            return m_droppableItems.Where(entry => entry.Value.MonsterOwnerId == id).Select(entry => entry.Value).ToList();
        }

        public MonsterSuperRace GetSuperRace(int id)
        {
            MonsterSuperRace result;
            if (!m_monsterSuperRaces.TryGetValue(id, out result))
                return null;

            return result;
        }

        public MonsterTemplate GetTemplate(int id)
        {
            MonsterTemplate result;
            if (!m_monsterTemplates.TryGetValue(id, out result))
                return null;

            return result;
        }

        public MonsterTemplate[] GetTemplates()
        {
            return m_monsterTemplates.Values.ToArray();
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

        public void RemoveMonsterSpell(MonsterSpell spell)
        {
            spell.Delete();
            m_monsterSpells.Remove(spell.Id);
        }

        public MonsterSpawn[] GetMonsterSpawns()
        {
            return m_monsterSpawns.Values.ToArray();
        }

        public MonsterDungeonSpawn[] GetMonsterDungeonsSpawns()
        {
            return m_monsterDungeonsSpawns.Values.ToArray();
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

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            spawn.Delete();
            m_monsterSpawns.Remove(spawn.Id);
        }

        public void AddMonsterDrop(DroppableItem drop)
        {
            drop.Save();
            m_droppableItems.Add(drop.Id, drop);
        }

        public void RemoveMonsterDrop(DroppableItem drop)
        {
            drop.Delete();
            m_droppableItems.Remove(drop.Id);
        }
    }
}