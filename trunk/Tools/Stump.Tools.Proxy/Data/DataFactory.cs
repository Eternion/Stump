// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.IO;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Actions.NpcActions;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Npcs.StartActions;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Data
{
    public static class DataFactory
    {
        [Variable]
        public static string Output = "./../../static/";

        [Variable]
        public static string NpcDir = "Npcs/";

        [Variable]
        public static string NpcQuestionsDir = "NpcsQuestions/";

        [Variable]
        public static string NpcRepliesDir = "NpcsReplies/";

        [Variable]
        public static string NpcActionsDir = "NpcsActions/";

        [Variable]
        public static string MonstersDir = "Monsters/";

        public static void HandleNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage)
        {
            BuildActionNpcQuestion(client, dialogQuestionMessage);

            if (!Directory.Exists(Output + NpcQuestionsDir))
            {
                Directory.CreateDirectory(Output + NpcQuestionsDir);
            }

            if (!File.Exists(Output + NpcQuestionsDir + dialogQuestionMessage.messageId + ".xml"))
                XmlUtils.Serialize(Output + NpcQuestionsDir + dialogQuestionMessage.messageId + ".xml",
                                   dialogQuestionMessage);
        }

        public static void BuildActionNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcReply != null)
            {
                uint replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var npcReply = new NpcDialogReply(replyId,
                                                  new ActionDialogQuestion((int) dialogQuestionMessage.messageId));

                if (!Directory.Exists(Output + NpcRepliesDir))
                {
                    Directory.CreateDirectory(Output + NpcRepliesDir);
                }

                if (!File.Exists(Output + NpcRepliesDir + replyId + ".xml"))
                    XmlUtils.Serialize(Output + NpcRepliesDir + replyId + ".xml", npcReply);
            }
            else if (client.GuessNpcFirstAction != null)
            {
                uint npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
                uint actionId = client.GuessNpcFirstAction.npcActionId;
                client.GuessNpcFirstAction = null;

                NpcStartActionSerialized actionSerialized = new TalkAction((int) npcId,
                                                                           (int) dialogQuestionMessage.messageId);

                if (!Directory.Exists(Output + NpcActionsDir))
                {
                    Directory.CreateDirectory(Output + NpcActionsDir);
                }

                if (!File.Exists(Output + NpcActionsDir + npcId + "_" + actionId + ".xml"))
                    XmlUtils.Serialize(Output + NpcActionsDir + npcId + "_" + actionId + ".xml",
                                       actionSerialized);
            }
        }

        public static void BuildActionNpcLeave(WorldClient client, LeaveDialogMessage leaveDialogMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcReply == null)
                return;

            uint replyId = client.GuessNpcReply.replyId;
            client.GuessNpcReply = null;

            var npcReply = new NpcDialogReply(replyId, new ActionDialogLeave());

            if (!Directory.Exists(Output + NpcRepliesDir))
            {
                Directory.CreateDirectory(Output + NpcRepliesDir);
            }

            if (!File.Exists(Output + NpcRepliesDir + replyId + ".xml"))
                XmlUtils.Serialize(Output + NpcRepliesDir + replyId + ".xml", npcReply);
        }

        public static void HandleActorInformations(WorldClient client, GameRolePlayActorInformations actorInformations,
                                                   uint mapId)
        {
            if (actorInformations is GameRolePlayNpcInformations)
                HandleNpcInformations(client, actorInformations as GameRolePlayNpcInformations, mapId);
        }

        public static void HandleNpcInformations(WorldClient client, GameRolePlayNpcInformations npcInformations,
                                                 uint mapId)
        {
            string zoneName = ZonesManager.GetZoneNameByMap(mapId);

            if (!Directory.Exists(Output + NpcDir))
            {
                Directory.CreateDirectory(Output + NpcDir);
            }

            if (!Directory.Exists(Output + NpcDir + zoneName))
            {
                Directory.CreateDirectory(Output + NpcDir + zoneName);
            }

            if (!File.Exists(Output + NpcDir + zoneName + "/" +
                             mapId + "_" + npcInformations.npcId + ".xml"))
                XmlUtils.Serialize(Output + NpcDir + zoneName + "/" +
                                   mapId + "_" + npcInformations.npcId + ".xml", npcInformations);
        }
    }
}