using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs
{
    public class NpcManager : Singleton<NpcManager>
    {
        private Dictionary<uint, NpcSpawn> m_npcsSpawns;
        private Dictionary<int, NpcTemplate> m_npcsTemplates;
        private Dictionary<uint, NpcAction> m_npcsActions;
        private Dictionary<int, NpcReply> m_npcsReplies;
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

        public NpcSpawn GetOneNpcSpawn(Predicate<NpcSpawn> predicate)
        {
            return m_npcsSpawns.Values.SingleOrDefault(entry => predicate(entry));
        }

        public IEnumerable<NpcSpawn> GetNpcSpawns()
        {
            return m_npcsSpawns.Values;
        }

        public IEnumerable<NpcTemplate> GetNpcTemplates()
        {
            return m_npcsTemplates.Values;
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

        public NpcMessage GetNpcMessage(int id)
        {
            NpcMessage message;
            if (m_npcsMessages.TryGetValue(id, out message))
                return message;

            return message;
        }

        public List<NpcAction> GetNpcActions(int id)
        {
            return m_npcsActions.Where(entry => entry.Value.NpcId == id).Select(entry => entry.Value).ToList();
        }

        public List<NpcReply> GetMessageReplies(int id)
        {
            return m_npcsReplies.Where(entry => entry.Value.MessageId == id).Select(entry => entry.Value).ToList();
        }

        public void AddNpcSpawn(NpcSpawn spawn)
        {
            spawn.Save();
            m_npcsSpawns.Add(spawn.Id, spawn);
        }

        public void RemoveNpcSpawn(NpcSpawn spawn)
        {
            spawn.Delete();
            m_npcsSpawns.Remove(spawn.Id);
        }

        public void AddNpcAction(NpcAction action)
        {
            action.Save();
            m_npcsActions.Add(action.Id, action);
        }

        public void RemoveNpcAction(NpcAction action)
        {
            action.Delete();
            m_npcsActions.Remove(action.Id);
        }
    }
}