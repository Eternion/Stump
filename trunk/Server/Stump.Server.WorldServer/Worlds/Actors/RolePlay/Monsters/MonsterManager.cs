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

        [Initialization(InitializationPass.Sixth)]
        public void Initialize()
        {
            m_monsterTemplates = MonsterTemplate.FindAll().ToDictionary(entry => entry.Id);
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
    }
}