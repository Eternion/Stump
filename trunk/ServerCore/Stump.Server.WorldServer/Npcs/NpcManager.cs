
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Entities;
using NpcEx = Stump.DofusProtocol.D2oClasses.Npc;
using NpcActionEx = Stump.DofusProtocol.D2oClasses.NpcAction;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcManager
    {
        private static readonly Dictionary<int, NpcTemplate> NpcTemplates = new Dictionary<int, NpcTemplate>();

        private static Dictionary<int, NpcDialogQuestion> m_npcQuestions = new Dictionary<int, NpcDialogQuestion>();


        [StageStep(Stages.Two, "Loaded Npcs")]
        public static void LoadNpcs()
        {
            IEnumerable<NpcEx> npcsEx = DataLoader.LoadData<NpcEx>();

            foreach (NpcEx npc in npcsEx)
            {
                var npcTemplate = new NpcTemplate(npc)
                    {
                        Name = DataLoader.GetI18NText((int) npc.nameId)
                    };

                NpcTemplates.Add(npcTemplate.Id, npcTemplate);
            }

            foreach(var action in NpcLoader.LoadNpcActions())
            {
                var template = GetTemplate(action.NpcId);

                if (template == null)
                    throw new Exception(string.Format("Template <id:{0}> doesn't exist", action.NpcId));

                template.StartActions.Add(action.ActionType, action);
            }

            m_npcQuestions = NpcLoader.LoadQuestions().ToDictionary(entry => (int)entry.Id);
        }

        public static NpcTemplate GetTemplate(int id)
        {
            return NpcTemplates.ContainsKey(id) ? NpcTemplates[id] : null;
        }

        public static NpcDialogQuestion GetQuestion(int id)
        {
            return m_npcQuestions.ContainsKey(id) ? m_npcQuestions[id] : null;
        }
    }
}