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
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Actions.ActionsCharacter;
using Stump.Server.WorldServer.Actions.ActionsNpcs;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Npcs.StartActions;
using Stump.Server.WorldServer.Skills;
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

        [Variable]
        public static string InteractiveObjectsDir = "InteractiveObjects/";

        [Variable]
        public static string SkillActionsDir = "SkillActions/";

        public static void HandleNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage)
        {
            BuildActionNpcQuestion(client, dialogQuestionMessage);

            if (!Directory.Exists(Output + NpcQuestionsDir))
            {
                Directory.CreateDirectory(Output + NpcQuestionsDir);
            }

            SerializeToXml(
                Output + NpcQuestionsDir + dialogQuestionMessage.messageId + ".xml",
                dialogQuestionMessage);
        }

        public static void BuildActionTeleport(WorldClient client, CurrentMapMessage message)
        {
            if (client.GuessNpcReply != null)
            {
                uint replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var npcReply = new NpcDialogReply(replyId,
                                                  new ActionTeleport(message.mapId, (ushort) client.Disposition.cellId,
                                                                     (DirectionsEnum) client.Disposition.direction));

                if (!Directory.Exists(Output + NpcRepliesDir))
                {
                    Directory.CreateDirectory(Output + NpcRepliesDir);
                }

                SerializeToXml(Output + NpcRepliesDir + replyId + ".xml",
                               npcReply);
            }
            else if (client.GuessSkillAction != null)
            {
                uint mapId = client.GuessSkillAction.Item1;
                uint skillId = client.GuessSkillAction.Item2.skillInstanceUid;
                uint elementId = client.GuessSkillAction.Item2.elemId;
                uint duration = client.GuessSkillAction.Item3.duration;
                client.GuessSkillAction = null;

                var skill = new SkillInstance(skillId,
                                              new SkillUse(duration,
                                                           actions:
                                                               new ActionTeleport(message.mapId,
                                                                                  (ushort) client.Disposition.cellId,
                                                                                  (DirectionsEnum)
                                                                                  client.Disposition.direction)));

                SerializeToXml(
                    GetDirectoryPath(client, SkillActionsDir) + mapId + "_" + elementId + "_" + skillId + ".xml",
                    skill);
            }
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

                SerializeToXml(Output + NpcRepliesDir + replyId + ".xml",
                               npcReply);
            }
            else if (client.GuessNpcFirstAction != null)
            {
                uint npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
                uint actionId = client.GuessNpcFirstAction.npcActionId;
                client.GuessNpcFirstAction = null;

                NpcStartActionSerialized actionSerialized = new TalkAction((int) npcId,
                                                                           (int) dialogQuestionMessage.messageId);

                SerializeToXml(
                    GetDirectoryPath(client, NpcActionsDir) + client.CurrentMap + "_" + npcId + "_" + actionId + ".xml",
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

            SerializeToXml(Output + NpcRepliesDir + replyId + ".xml",
                           npcReply);
        }

        public static void BuildActionNpcShop(WorldClient client, ExchangeStartOkNpcShopMessage npcShopMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcFirstAction == null)
                return;

            uint actionId = client.GuessNpcFirstAction.npcActionId;
            uint npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
            client.GuessNpcFirstAction = null;

            NpcStartActionSerialized action = new ShopAction((int) npcId, npcShopMessage.tokenId,
                                                             npcShopMessage.objectsInfos.Select(
                                                                 entry => new ItemToSellInNpcShop(entry)).ToList());

            SerializeToXml(
                GetDirectoryPath(client, NpcActionsDir) + client.CurrentMap + "_" + npcId + "_" +
                actionId + ".xml",
                action);
        }

        public static void HandleActorInformations(WorldClient client, GameRolePlayActorInformations actorInformations)
        {
            if (actorInformations is GameRolePlayNpcInformations)
                HandleNpcInformations(client, actorInformations as GameRolePlayNpcInformations);
        }

        public static void HandleNpcInformations(WorldClient client, GameRolePlayNpcInformations npcInformations)
        {
            SerializeToXml(
                GetDirectoryPath(client, NpcDir) + client.CurrentMap + "_" + npcInformations.npcId + ".xml",
                npcInformations);
        }

        public static void HandleInteractiveObject(WorldClient client, InteractiveElement interactiveElement)
        {
            SerializeToXml(
                GetDirectoryPath(client, InteractiveObjectsDir) + client.CurrentMap + "_" + interactiveElement.elementId +
                ".xml",
                interactiveElement);
        }

        public static string GetDirectoryPath(WorldClient client, string specificDirectory)
        {
            string zoneName = ZonesManager.GetRegionNameByMap(client.CurrentMap);

            if (!Directory.Exists(Output + specificDirectory))
            {
                Directory.CreateDirectory(Output + specificDirectory);
            }

            if (!Directory.Exists(Output + specificDirectory + zoneName))
            {
                Directory.CreateDirectory(Output + specificDirectory + zoneName);
            }

            return Output + specificDirectory + zoneName + "/";
        }

        public static void SerializeToXml<T>(string path, T item)
        {
            if (!File.Exists(path))
                XmlUtils.Serialize(path, item);
        }
    }
}