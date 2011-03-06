//// /*************************************************************************
////  *
////  *  Copyright (C) 2010 - 2011 Stump Team
////  *
////  *  This program is free software: you can redistribute it and/or modify
////  *  it under the terms of the GNU General Public License as published by
////  *  the Free Software Foundation, either version 3 of the License, or
////  *  (at your option) any later version.
////  *
////  *  This program is distributed in the hope that it will be useful,
////  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  *  GNU General Public License for more details.
////  *
////  *  You should have received a copy of the GNU General Public License
////  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
////  *
////  *************************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Stump.DofusProtocol.Enums;
//using Stump.Server.BaseServer.Data;
//using Stump.Server.BaseServer.Initializing;
//using Stump.Server.DataProvider.Data;
//using Stump.Server.DataProvider.Data.D2oTool;
//using Stump.Server.WorldServer.Data;
//using Stump.Server.WorldServer.Entities;
//using NpcEx = Stump.DofusProtocol.D2oClasses.Npc;
//using NpcActionEx = Stump.DofusProtocol.D2oClasses.NpcAction;

//namespace Stump.Server.WorldServer.Npcs
//{
//    public class NpcManager
//    {
//        private static readonly Dictionary<int, NpcTemplate> NpcTemplates = new Dictionary<int, NpcTemplate>();

//        private static Dictionary<int, NpcDialogQuestion> m_npcQuestions = new Dictionary<int, NpcDialogQuestion>();


//        [StageStep(Stages.Two, "Loaded Npcs")]
//        public static void LoadNpcs()
//        {
//            IEnumerable<NpcEx> npcsEx = D2OLoader.LoadData<NpcEx>();

//            foreach (NpcEx npc in npcsEx)
//            {
//                var npcTemplate = new NpcTemplate(npc)
//                    {
//                        Name = D2OLoader.GetI18NText((int) npc.nameId)
//                    };

//                NpcTemplates.Add(npcTemplate.Id, npcTemplate);
//            }

//            foreach(var action in NpcLoader.LoadNpcActions())
//            {
//                var template = GetTemplate(action.NpcId);

//                if (template == null)
//                    throw new Exception(string.Format("Template <id:{0}> doesn't exist", action.NpcId));

//                template.StartActions.Add(action.ActionType, action);
//            }

//            m_npcQuestions = NpcLoader.LoadQuestions().ToDictionary(entry => (int)entry.Id);
//        }

//        public static NpcTemplate GetTemplate(int id)
//        {
//            return NpcTemplates.ContainsKey(id) ? NpcTemplates[id] : null;
//        }

//        public static NpcDialogQuestion GetQuestion(int id)
//        {
//            return m_npcQuestions.ContainsKey(id) ? m_npcQuestions[id] : null;
//        }
//    }
//}