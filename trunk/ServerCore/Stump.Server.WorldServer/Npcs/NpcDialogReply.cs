
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Actions;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialogReply
    {
        private NpcDialogReply()
        {
            
        }

        public NpcDialogReply(uint id, params ActionSerialized[] actions)
        {
            Id = id;
            ActionsSerialized = actions;
        }

        public uint Id
        {
            get;
            set;
        }

        public ActionSerialized[] ActionsSerialized
        {
            get;
            set;
        }

        public void Execute(NpcSpawn npc, Character dialoger)
        {
            foreach (var actionBase in ActionsSerialized)
            {
                ActionBase.ExecuteAction(actionBase, npc, dialoger);
            }
        }
    }
}