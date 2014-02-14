using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Npcs.Replies;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs
{
    public class NpcManager : DataManager<NpcManager>
    {
        private Dictionary<uint, NpcSpawn> m_npcsSpawns;
        private Dictionary<int, NpcTemplate> m_npcsTemplates;
        private Dictionary<uint, NpcActionRecord> m_npcsActions;
        private Dictionary<int, NpcReplyRecord> m_npcsReplies;
        private Dictionary<int, NpcMessage> m_npcsMessages;

        [Initialization(InitializationPass.Fifth)]
        public override void Initialize()
        {
            m_npcsSpawns = Database.Fetch<NpcSpawn>(NpcSpawnRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_npcsTemplates = Database.Fetch<NpcTemplate>(NpcTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_npcsActions = Database.Fetch<NpcActionRecord>(NpcActionRecordRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_npcsReplies = Database.Fetch<NpcReplyRecord>(NpcReplyRecordRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_npcsMessages = Database.Fetch<NpcMessage>(NpcMessageRelator.FetchQuery).ToDictionary(entry => entry.Id);
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
            return m_npcsTemplates.TryGetValue(id, out template) ? template : template;
        }

        public NpcTemplate GetNpcTemplate(string name, bool ignorecase)
        {
            return
                m_npcsTemplates.Values.FirstOrDefault(entry => entry.Name.Equals(name,
                                                                                 ignorecase
                                                                                     ? StringComparison.InvariantCultureIgnoreCase
                                                                                     : StringComparison.InvariantCulture));
        }

        public NpcMessage GetNpcMessage(int id)
        {
            NpcMessage message;
            return m_npcsMessages.TryGetValue(id, out message) ? message : message;
        }

        public List<NpcActionRecord> GetNpcActionsRecords(int id)
        {
            return m_npcsActions.Where(entry => entry.Value.NpcId == id).Select(entry => entry.Value).ToList();
        }

        public List<NpcReplyRecord> GetMessageRepliesRecords(int id)
        {
            return m_npcsReplies.Where(entry => entry.Value.MessageId == id).Select(entry => entry.Value).ToList();
        }

        public List<NpcActionDatabase> GetNpcActions(int id)
        {
            return m_npcsActions.Where(entry => entry.Value.NpcId == id).Select(entry => entry.Value.GenerateAction()).ToList();
        }

        public List<NpcReply> GetMessageReplies(int id)
        {
            return m_npcsReplies.Where(entry => entry.Value.MessageId == id).Select(entry => entry.Value.GenerateReply()).ToList();
        }

        public void AddNpcSpawn(NpcSpawn spawn)
        {
            Database.Insert(spawn);
            m_npcsSpawns.Add(spawn.Id, spawn);
        }

        public void RemoveNpcSpawn(NpcSpawn spawn)
        {
            Database.Delete(spawn);
            m_npcsSpawns.Remove(spawn.Id);
        }

        public void AddNpcAction(NpcActionDatabase action)
        {
            Database.Insert(action.Record);
            m_npcsActions.Add(action.Record.Id, action.Record);
        }

        public void RemoveNpcAction(NpcActionDatabase action)
        {
            Database.Delete(action);
            m_npcsActions.Remove(action.Record.Id);
        }
    }
}