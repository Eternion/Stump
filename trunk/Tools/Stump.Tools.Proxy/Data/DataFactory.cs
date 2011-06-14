
using System.IO;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Xml;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Actions.ActionsCharacter;
using Stump.Server.WorldServer.Actions.ActionsNpcs;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Npcs.StartActions;
using Stump.Server.WorldServer.Skills;
using Stump.Server.WorldServer.XmlSerialize;
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

        [Variable]
        public static string CellTriggersDir = "CellTriggers/";

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

                var action = new ActionTeleport(message.mapId,
                                                (ushort) client.Disposition.cellId,
                                                (DirectionsEnum) client.Disposition.direction);

                var skill = new SkillInstance(skillId,
                                              new SkillUse(duration, actions: action));

                SerializeToXml(
                    GetDirectoryPath(client, mapId, SkillActionsDir) + mapId + "_" + elementId + "_" + skillId + ".xml",
                    new SkillInstanceSerialized(mapId, elementId, skill));
            }
            else if (client.GuessCellTrigger != null)
            {
                var trigger = new CellTrigger((ushort) client.GuessCellTrigger, client.LastMap,
                                              CellTrigger.TriggerEvent.OnReached,
                                              new ActionTeleport(client.CurrentMap,
                                                                 (ushort) client.Disposition.cellId,
                                                                 (DirectionsEnum) client.Disposition.direction));

                SerializeToXml(
                    GetDirectoryPath(client, client.LastMap, CellTriggersDir) + client.LastMap + "_" + client.GuessCellTrigger + "_" +
                    (int) CellTrigger.TriggerEvent.OnReached + ".xml",
                    trigger);

                client.GuessCellTrigger = null;
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
                new NpcSerialized(client.CurrentMap, npcInformations));
        }

        public static void HandleInteractiveObject(WorldClient client, InteractiveElement interactiveElement)
        {
            SerializeToXml(
                GetDirectoryPath(client, InteractiveObjectsDir) + client.CurrentMap + "_" + interactiveElement.elementId +
                ".xml",
                new InteractiveElementSerialized(client.CurrentMap, interactiveElement));
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

        public static string GetDirectoryPath(WorldClient client, uint mapId, string specificDirectory)
        {
            string zoneName = ZonesManager.GetRegionNameByMap(mapId);

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