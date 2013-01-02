using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives
{
    public class InteractiveManager : DataManager<InteractiveManager>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();
        private Dictionary<int, InteractiveSpawn> m_interactivesSpawns;
        private Dictionary<int, InteractiveTemplate> m_interactivesTemplates;
        private Dictionary<int, InteractiveSkillTemplate> m_skillsTemplates;

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_interactivesTemplates = Database.Fetch<InteractiveTemplate>(InteractiveTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_interactivesSpawns = Database.Fetch<InteractiveSpawn>(InteractiveSpawnRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_skillsTemplates = Database.Fetch<InteractiveSkillTemplate>(InteractiveSkillTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public int PopSkillId()
        {
            return m_idProvider.Pop();
        }

        public void FreeSkillId(int id)
        {
            m_idProvider.Push(id);
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
            if (m_interactivesTemplates.TryGetValue(id, out template))
                return template;

            return template;
        }

        public InteractiveSkillTemplate GetSkillTemplate(int id)
        {
            InteractiveSkillTemplate template;
            if (m_skillsTemplates.TryGetValue(id, out template))
                return template;

            return template;
        }

        public void AddInteractiveSpawn(InteractiveSpawn spawn)
        {
            Database.Insert(spawn);
            m_interactivesSpawns.Add(spawn.Id, spawn);
        }
    }
}