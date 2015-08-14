using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives
{
    public class InteractiveManager : DataManager<InteractiveManager>
    {
        private UniqueIdProvider m_idProviderSpawn = new UniqueIdProvider();
        private UniqueIdProvider m_idProviderSkill = new UniqueIdProvider();
        private Dictionary<int, InteractiveSpawn> m_interactivesSpawns;
        private Dictionary<int, InteractiveTemplate> m_interactivesTemplates;
        private Dictionary<int, InteractiveSkillTemplate> m_skillsTemplates;
        private Dictionary<int, InteractiveSkillRecord> m_interactivesSkills;

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_interactivesTemplates = Database.Query<InteractiveTemplate, InteractiveTemplateSkills, InteractiveSkillRecord, InteractiveTemplate>
                (new InteractiveTemplateRelator().Map, InteractiveTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_interactivesSpawns = Database.Query<InteractiveSpawn, InteractiveSpawnSkills, InteractiveSkillRecord, InteractiveSpawn>
                (new InteractiveSpawnRelator().Map, InteractiveSpawnRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_skillsTemplates = Database.Query<InteractiveSkillTemplate>(InteractiveSkillTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_interactivesSkills =
                Database.Query<InteractiveSkillRecord>(InteractiveSkillRelator.FetchQuery).ToDictionary(entry => entry.Id);

            m_idProviderSpawn = m_interactivesSpawns.Any()
                ? new UniqueIdProvider(m_interactivesSpawns.Select(x => x.Value.Id).Max())
                : new UniqueIdProvider(0);
            m_idProviderSkill = m_interactivesSkills.Any()
                ? new UniqueIdProvider(m_interactivesSkills.Select(x => x.Value.Id).Max())
                : new UniqueIdProvider(0);
        }

        public int PopSkillId()
        {
            return m_idProviderSkill.Pop();
        }

        public int PopSpawnId()
        {
            return m_idProviderSpawn.Pop();
        }

        public void FreeSkillId(int id)
        {
            m_idProviderSkill.Push(id);
        }

        public IEnumerable<InteractiveSpawn> GetInteractiveSpawns()
        {
            return m_interactivesSpawns.Values;
        }

        public InteractiveSpawn GetOneSpawn(Predicate<InteractiveSpawn> predicate)
        {
            return m_interactivesSpawns.Values.SingleOrDefault(entry => predicate(entry));
        }

        public InteractiveTemplate GetTemplate(int id)
        {
            InteractiveTemplate template;
            return m_interactivesTemplates.TryGetValue(id, out template) ? template : template;
        }

        public InteractiveSkillTemplate GetSkillTemplate(int id)
        {
            InteractiveSkillTemplate template;
            return m_skillsTemplates.TryGetValue(id, out template) ? template : template;
        }

        public void AddInteractiveSpawn(InteractiveSpawn spawn, InteractiveSkillRecord skill, InteractiveSpawnSkills spawnSkill)
        {
            Database.Insert(spawn);
            Database.Insert(skill);
            Database.Insert(spawnSkill);

            m_interactivesSpawns.Add(spawn.Id, spawn);

            spawn.GetMap().SpawnInteractive(spawn);
        }

        public void RemoveInteractiveSpawn(InteractiveSpawn spawn)
        {
            var skills = spawn.GetSkills();

            foreach (var skill in skills)
            {
                Database.Delete(skill);
                Database.Delete("interactives_spawns_skills", "SkillId", skill.Id);
            }

            spawn.GetMap().UnSpawnInteractive(new InteractiveObject(spawn));

            Database.Delete(spawn);
            m_interactivesSpawns.Remove(spawn.Id);     
        }
    }
}