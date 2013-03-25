using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Npcs.Replies;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Data
{
    public static class DataFactory
    {
        private static Database Database
        {
            get { return Proxy.Instance.DatabaseAccessor.Database; }
        }

        public static void HandleNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage)
        {
            NpcMessage message = NpcManager.Instance.GetNpcMessage(dialogQuestionMessage.messageId);
            BuildActionNpcQuestion(client, dialogQuestionMessage, message);

            client.LastNpcMessage = message;
        }

        public static void BuildActionTeleport(WorldClient client, CurrentMapMessage message)
        {
            if (client.GuessNpcReply != null)
            {
                int replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var npcReply = new TeleportReply
                    {
                        ReplyId = replyId,
                        Message = client.LastNpcMessage,
                        MapId = client.CurrentMap.Id,
                        CellId = client.Disposition.cellId,
                        Direction = (DirectionsEnum) client.Disposition.direction,
                    };

                ExecuteIOTask(() =>
                    {
                        if (npcReply.Message.Replies.Count(entry => entry is TeleportReply &&
                                                                    (entry as TeleportReply).ReplyId == npcReply.ReplyId) >
                            0)
                            return;

                        Database.Insert(npcReply.Record);
                        npcReply.Message.Replies.Add(npcReply);

                        client.SendChatMessage("Npc reply added");
                    });
            }
            if (client.GuessSkillAction != null)
            {
                Map map = client.GuessSkillAction.Item1;
                int skillId = client.GuessSkillAction.Item2.skillInstanceUid;
                int elementId = client.GuessSkillAction.Item2.elemId;
                int duration = client.GuessSkillAction.Item3.duration;
                client.GuessSkillAction = null;

                if (!client.IsSkillActionValid())
                    return;

                var skill = new InteractiveSkillRecord
                    {
                        Type = "Teleport",
                        Parameter0 = client.CurrentMap.Id.ToString(),
                        Parameter1 = client.Disposition.cellId.ToString(),
                        Parameter2 = ((int) client.Disposition.direction).ToString(),
                        Condition = string.Empty,
                        Duration = duration,
                    };

                InteractiveSpawn io =
                    InteractiveManager.Instance.GetOneSpawn(
                        entry => entry.MapId == map.Id && entry.ElementId == elementId);

                if (io == null)
                    return;


                ExecuteIOTask(() =>
                    {
                        if (io.Template == null && io.Skills.Count > 0)
                            return;

                        if (io.Template != null && io.Template.Skills.Count(entry => entry.Type == "Teleport" &&
                                                                                     entry.Parameter0 ==
                                                                                     skill.Parameter0 &&
                                                                                     entry.Parameter1 ==
                                                                                     skill.Parameter1 &&
                                                                                     entry.Parameter2 ==
                                                                                     skill.Parameter2) > 0)
                            return;

                        using (Transaction transaction = Database.GetTransaction())
                        {
                            Database.Insert(skill);
                            if (io.Template == null)
                            {
                                io.Skills.Add(skill);

                                var record = new InteractiveSpawnSkills
                                    {
                                        InteractiveSpawnId = io.Id,
                                        SkillId = skill.Id,
                                    };

                                Database.Insert(record);
                            }
                            else
                            {
                                io.Template.Skills.Add(skill);

                                var record = new InteractiveTemplateSkills
                                    {
                                        InteractiveTemplateId = io.TemplateId,
                                        SkillId = skill.Id,
                                    };

                                Database.Insert(record);
                            }

                            transaction.Complete();
                        }

                        client.SendChatMessage("Teleport skill added");
                    });
            }
            else if (client.GuessCellTrigger != null)
            {
                if (client.LastMap.Cells[client.GuessCellTrigger.Value].MapChangeData > 0)
                    return;

                var cell = (short) client.GuessCellTrigger.Value;
                client.GuessCellTrigger = null;

                if (!client.IsCellTriggerValid())
                    return;

                var trigger = new CellTriggerRecord
                    {
                        Type = "Teleport",
                        MapId = client.LastMap.Id,
                        CellId = cell,
                        TriggerType = CellTriggerType.END_MOVE_ON,
                        Condition = string.Empty,
                        Parameter0 = client.Disposition.cellId.ToString(),
                        Parameter1 = client.CurrentMap.Id.ToString(),
                    };


                ExecuteIOTask(() =>
                    {
                        if (CellTriggerManager.Instance.GetOneCellTrigger(entry => entry.Type == "Teleport" &&
                                                                                   entry.MapId == trigger.MapId &&
                                                                                   entry.CellId == trigger.CellId &&
                                                                                   entry.Parameter0 ==
                                                                                   trigger.Parameter0 &&
                                                                                   entry.Parameter1 ==
                                                                                   trigger.Parameter1 &&
                                                                                   entry.TriggerType ==
                                                                                   trigger.TriggerType) != null)
                            return;

                        CellTriggerManager.Instance.AddCellTrigger(trigger);

                        client.SendChatMessage("Cell trigger added");
                    });
            }
        }

        public static void BuildActionNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage,
                                                  NpcMessage currentMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcReply != null)
            {
                int replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var record = new NpcReplyRecord
                    {
                        Type = "Dialog",
                        ReplyId = replyId,
                        Message = client.LastNpcMessage,
                        Parameter0 = currentMessage.Id.ToString(),
                    };

                var npcReply = (ContinueDialogReply) record.GenerateReply();

                ExecuteIOTask(() =>
                    {
                        if (npcReply.Message.Replies.Count(entry => entry is ContinueDialogReply &&
                                                                    (entry as ContinueDialogReply).ReplyId ==
                                                                    npcReply.ReplyId &&
                                                                    (entry as ContinueDialogReply).NextMessage.Id ==
                                                                    npcReply.NextMessage.Id) > 0)
                            return;

                        Database.Insert(record);
                        npcReply.Message.Replies.Add(npcReply);

                        client.SendChatMessage("Npc reply added");
                    });
            }
            else if (client.GuessNpcFirstAction != null)
            {
                int npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
                int actionId = client.GuessNpcFirstAction.npcActionId;
                client.GuessNpcFirstAction = null;

                var record = new NpcActionRecord
                    {
                        Type = "Talk",
                        NpcId = npcId,
                        Parameter0 = currentMessage.Id.ToString(),
                    };

                var action = (NpcTalkAction) record.GenerateAction();


                ExecuteIOTask(() =>
                    {
                        NpcTemplate npc = NpcManager.Instance.GetNpcTemplate(npcId);

                        if (npc.Actions.Count(entry => entry is NpcTalkAction) > 0)
                            return;

                        Database.Insert(record);
                        npc.Actions.Add(action);

                        client.SendChatMessage("Npc action added");
                    });
            }
        }

        public static void BuildActionNpcLeave(WorldClient client, LeaveDialogMessage leaveDialogMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcReply == null)
                return;

            int replyId = client.GuessNpcReply.replyId;
            client.GuessNpcReply = null;

            var record = new NpcReplyRecord
                {
                    Type = "EndDialog",
                    ReplyId = replyId,
                    Message = client.LastNpcMessage,
                };

            var npcReply = (EndDialogReply) record.GenerateReply();


            ExecuteIOTask(() =>
                {
                    if (npcReply.Message.Replies.Count(entry => entry is EndDialogReply &&
                                                                (entry as EndDialogReply).ReplyId == npcReply.ReplyId) >
                        0)
                        return;

                    Database.Insert(npcReply);
                    npcReply.Message.Replies.Add(npcReply);

                    client.SendChatMessage("Npc reply added");
                });
        }

        public static void BuildActionNpcShop(WorldClient client, ExchangeStartOkNpcShopMessage npcShopMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcFirstAction == null)
                return;

            int actionId = client.GuessNpcFirstAction.npcActionId;
            int npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
            client.GuessNpcFirstAction = null;


            ExecuteIOTask(() =>
                {
                    NpcTemplate npc = NpcManager.Instance.GetNpcTemplate(npcId);


                    var record = new NpcActionRecord
                        {
                            Type = NpcBuySellAction.Discriminator,
                            NpcId = npcId,
                        };

                    var action = (NpcBuySellAction) record.GenerateAction();

                    if (npc.Actions.Count(entry => entry is NpcBuySellAction) > 0)
                        return;

                    using (Transaction transaction = Database.GetTransaction())
                    {
                        Database.Insert(record);

                        foreach (ObjectItemToSellInNpcShop objInfo in npcShopMessage.objectsInfos)
                        {
                            var item = new NpcItem
                                {
                                    Item = ItemManager.Instance.TryGetTemplate(objInfo.objectGID),
                                    NpcShopId = (int) action.Id,
                                    BuyCriterion = string.Empty,
                                    CustomPrice = objInfo.objectPrice
                                };
                            Database.Insert(item);
                            action.Items.Add(item);
                        }

                        npc.Actions.Add(action);
                        transaction.Complete();
                    }

                    client.SendChatMessage("Npc shop added");
                });
        }

        public static void BuildMonsterSpell(WorldClient client, GameFightMonsterInformations monster,
                                             GameActionFightSpellCastMessage spell)
        {
            ExecuteIOTask(() =>
                {
                    var monsterSpell = new MonsterSpell();
                    MonsterGrade grade = MonsterManager.Instance.GetMonsterGrade(monster.creatureGenericId,
                                                                                 monster.creatureGrade);

                    if (MonsterManager.Instance.GetOneMonsterSpell(entry =>
                                                                   entry.MonsterGrade != null &&
                                                                   entry.MonsterGrade.Id == grade.Id &&
                                                                   entry.SpellId == spell.spellId) != null)
                        return;

                    monsterSpell.MonsterGrade = grade;
                    monsterSpell.SpellId = spell.spellId;
                    monsterSpell.Level = spell.spellLevel;

                    MonsterManager.Instance.AddMonsterSpell(monsterSpell);
                    client.SendChatMessage("Monster spell added");
                });
        }

        public static void BuildMapFightPlacement(WorldClient client, Map map, IEnumerable<short> blueCells,
                                                  IEnumerable<short> redCells)
        {
            if (map.Record.BlueFightCells.Length > 0 && map.Record.RedFightCells.Length > 0)
                return;

            ExecuteIOTask(() =>
                {
                    map.Record.BlueFightCells = blueCells.ToArray();
                    map.Record.RedFightCells = redCells.ToArray();

                    Database.Update(map.Record);

                    client.SendChatMessage("Fights placements added");
                });
        }

        public static void HandleActorInformations(WorldClient client, GameRolePlayActorInformations actorInformations)
        {
            if (actorInformations is GameRolePlayNpcInformations)
                HandleNpcInformations(client, actorInformations as GameRolePlayNpcInformations);
            else if (actorInformations is GameRolePlayGroupMonsterInformations)
                HandleMonsterGroup(client, actorInformations as GameRolePlayGroupMonsterInformations);
        }

        public static void HandleNpcInformations(WorldClient client, GameRolePlayNpcInformations npcInformations)
        {
            var spawn = new NpcSpawn
                {
                    CellId = npcInformations.disposition.cellId,
                    Direction = (DirectionsEnum) npcInformations.disposition.direction,
                    MapId = client.CurrentMap.Id,
                    Template = NpcManager.Instance.GetNpcTemplate(npcInformations.npcId),
                    Look = npcInformations.look,
                };

            ExecuteIOTask(() =>
                {
                    if (NpcManager.Instance.GetOneNpcSpawn(entry =>
                                                           entry.CellId == spawn.CellId &&
                                                           entry.Direction == spawn.Direction &&
                                                           entry.MapId == spawn.MapId) != null)
                        return;

                    NpcManager.Instance.AddNpcSpawn(spawn);
                    client.SendChatMessage("Npc added");
                });
        }

        public static void HandleMonsterGroup(WorldClient client, GameRolePlayGroupMonsterInformations monsterGroup)
        {
            HandleMonsterSpawn(client, monsterGroup.staticInfos.mainCreatureLightInfos.creatureGenericId,
                               client.CurrentMap);

            foreach (MonsterInGroupInformations monster in monsterGroup.staticInfos.underlings)
            {
                HandleMonsterSpawn(client, monster.creatureGenericId, client.CurrentMap);
            }
        }

        public static void HandleMonsterSpawn(WorldClient client, int creatureId, Map currentMap)
        {
            ExecuteIOTask(() =>
                {
                    if (MonsterManager.Instance.GetOneMonsterSpawn(entry =>
                                                                   entry.SubArea.Id == currentMap.SubArea.Id &&
                                                                   entry.MonsterId == creatureId) != null)
                        return;

                    var spawn = new MonsterSpawn
                        {
                            MonsterId = creatureId,
                            MinGrade = 1,
                            MaxGrade = 5,
                            Frequency = 1.0,
                            SubAreaId = client.CurrentMap.SubArea.Id
                        };

                    MonsterManager.Instance.AddMonsterSpawn(spawn);
                    client.SendChatMessage("Monster spawn added");
                });
        }

        public static void HandleInteractiveObject(WorldClient client, InteractiveElement interactiveElement)
        {
            var ioSpawn = new InteractiveSpawn
                {
                    TemplateId = interactiveElement.elementTypeId,
                    ElementId = interactiveElement.elementId,
                    MapId = client.CurrentMap.Id,
                };

            ExecuteIOTask(() =>
                {
                    if (InteractiveManager.Instance.GetOneSpawn(entry =>
                                                                entry.MapId == ioSpawn.MapId &&
                                                                entry.ElementId == ioSpawn.ElementId) != null)
                        return;

                    InteractiveManager.Instance.AddInteractiveSpawn(ioSpawn);
                    client.SendChatMessage("Interactive Object added");
                });
        }

        public static void ExecuteIOTask(Action action)
        {
            Proxy.Instance.IOTaskPool.AddMessage(action);
        }
    }
}