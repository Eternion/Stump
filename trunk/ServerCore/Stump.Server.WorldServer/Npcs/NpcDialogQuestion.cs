
using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialogQuestion
    {
        private NpcDialogQuestion()
        {
            Replies = new Dictionary<uint, NpcDialogReply>();
        }

        public NpcDialogQuestion(uint id, string[] parameters)
             : this()
        {
            Id = id;
            Parameters = parameters;
        }

        public uint Id
        {
            get;
            private set;
        }

        public string[] Parameters
        {
            get;
            private set;
        }

        public Dictionary<uint, NpcDialogReply> Replies
        {
            get;
            private set;
        }

        public void CallReply(uint id, NpcSpawn npc, Character dialoger)
        {
            if (!Replies.ContainsKey(id))
                throw new ArgumentException("Reply with id '" + id + "' doesn't exist");

            Replies[id].Execute(npc, dialoger);
        }
    }
}