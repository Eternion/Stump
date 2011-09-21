using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs
{
    public class NpcManager : Singleton<NpcManager>
    {
        private Dictionary<uint, NpcSpawn> m_npcsSpawns;
        private Dictionary<int, NpcTemplate> m_npcsTemplates;
        private Dictionary<uint, NpcAction> m_npcsActions;
        private Dictionary<uint, NpcReply> m_npcsReplies;
        private Dictionary<int, NpcMessage> m_npcsMessages;

        [Initialization(InitializationPass.Fifth)]
        public void Initialize()
        {
            m_npcsSpawns = NpcSpawn.FindAll().ToDictionary(entry => entry.Id);
            m_npcsTemplates = NpcTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_npcsActions = NpcAction.FindAll().ToDictionary(entry => entry.Id);
            m_npcsReplies = NpcReply.FindAll().ToDictionary(entry => entry.Id);
            m_npcsMessages = NpcMessage.FindAll().ToDictionary(entry => entry.Id);
        }

        public NpcSpawn GetNpcSpawn(uint id)
        {
            NpcSpawn spawn;
            if (m_npcsSpawns.TryGetValue(id, out spawn))
                return spawn;

            return spawn;
        }

        public IEnumerable<NpcSpawn> GetNpcSpawns()
        {
            return m_npcsSpawns.Values;
        }

        public NpcTemplate GetNpcTemplate(int id)
        {
            NpcTemplate template;
            if (m_npcsTemplates.TryGetValue(id, out template))
                return template;

            return template;
        }

        public NpcTemplate GetNpcTemplate(string name, bool ignorecase)
        {
            return
                m_npcsTemplates.Values.Where(
                    entry =>
                    entry.Name.Equals(name,
                                      ignorecase
                                          ? StringComparison.InvariantCultureIgnoreCase
                                          : StringComparison.InvariantCulture)).FirstOrDefault();
        }

    }
}