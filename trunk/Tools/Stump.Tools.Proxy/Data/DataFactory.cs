using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Npcs.Replies;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Interactives;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells.Triggers;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Data
{
    public static class DataFactory
    {
        public static void HandleNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage)
        {
            NpcMessage message = NpcManager.Instance.GetNpcMessage(dialogQuestionMessage.messageId);
            BuildActionNpcQuestion(client, dialogQuestionMessage, message);

            client.LastNpcMessage = message;
        }

        public static void BuildActionTeleport(WorldClient client, CurrentMapMessage message)
        {
            /*if (client.GuessNpcReply != null)
            {
                int replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var npcReply = new NpcDialogReply(replyId,
                                                  new ActionTeleport(message.mapId, (ushort) client.Disposition.cellId,
                                                                     (OrdinalDirectionsEnum) client.Disposition.direction));

                if (!Directory.Exists(Output + NpcRepliesDir))
                {
                    Directory.CreateDirectory(Output + NpcRepliesDir);
                }

                SerializeToXml(Output + NpcRepliesDir + replyId + ".xml",
                               npcReply);
            }*/
            if (client.GuessSkillAction != null)
            {
                Map map = client.GuessSkillAction.Item1;
                int skillId = client.GuessSkillAction.Item2.skillInstanceUid;
                int elementId = client.GuessSkillAction.Item2.elemId;
                int duration = client.GuessSkillAction.Item3.duration;
                client.GuessSkillAction = null;

                var skill = new SkillTeleportTemplate
                                {
                                    MapId = client.CurrentMap.Id,
                                    CellId = client.Disposition.cellId,
                                    Direction = (DirectionsEnum) client.Disposition.direction,
                                    Condition = string.Empty,
                                    Duration = (uint) duration,
                                };

                InteractiveSpawn io = InteractiveManager.Instance.GetOneSpawn(entry => entry.MapId == map.Id && entry.ElementId == elementId);

                if (io == null)
                    return;


                ExecuteIOTask(() =>
                                  {
                                      if (io.Template == null && io.Skills.Count > 0)
                                          return;

                                      if (io.Template != null && io.Template.Skills.Count(entry => entry is SkillTeleportTemplate &&
                                                                                                   (entry as SkillTeleportTemplate).CellId == skill.CellId &&
                                                                                                   (entry as SkillTeleportTemplate).MapId == skill.MapId &&
                                                                                                   (entry as SkillTeleportTemplate).Direction == skill.Direction) > 0)
                                          return;

                                      skill.Save();
                                      if (io.Template == null)
                                      {
                                          io.Skills.Add(skill);
                                          io.Save();
                                      }
                                      else
                                      {
                                          io.Template.Skills.Add(skill);
                                          io.Template.Save();
                                      }

                                      client.SendChatMessage("Teleport skill added");
                                  });
            }
            else if (client.GuessCellTrigger != null)
            {
                if (client.LastMap.Cells[client.GuessCellTrigger.Value].MapChangeData > 0)
                    return;

                var trigger = new TeleportTriggerRecord
                                  {
                                      MapId = client.LastMap.Id,
                                      CellId = (short) client.GuessCellTrigger.Value,
                                      TriggerType = CellTriggerType.END_MOVE_ON,
                                      Condition = string.Empty,
                                      DestinationCellId = client.Disposition.cellId,
                                      DestinationMapId = client.CurrentMap.Id,
                                  };

                client.GuessCellTrigger = null;

                ExecuteIOTask(() =>
                                  {
                                      if (CellTriggerManager.Instance.GetOneCellTrigger(entry => entry is TeleportTriggerRecord &&
                                                                                                 (entry as TeleportTriggerRecord).MapId == trigger.MapId &&
                                                                                                 (entry as TeleportTriggerRecord).CellId == trigger.CellId &&
                                                                                                 (entry as TeleportTriggerRecord).DestinationCellId == trigger.DestinationCellId &&
                                                                                                 (entry as TeleportTriggerRecord).DestinationMapId == trigger.DestinationMapId &&
                                                                                                 (entry as TeleportTriggerRecord).TriggerType == trigger.TriggerType) != null)
                                          return;

                                      CellTriggerManager.Instance.AddCellTrigger(trigger);

                                      client.SendChatMessage("Cell trigger added");
                                  });
            }
        }

        public static void BuildActionNpcQuestion(WorldClient client, NpcDialogQuestionMessage dialogQuestionMessage, NpcMessage currentMessage)
        {
            if (!client.GuessAction)
                return;

            if (client.GuessNpcReply != null)
            {
                int replyId = client.GuessNpcReply.replyId;
                client.GuessNpcReply = null; // clear it as fast as possible

                var npcReply = new ContinueDialogReply
                                   {
                                       ReplyId = replyId,
                                       Message = client.LastNpcMessage,
                                       NextMessage = currentMessage
                                   };

                ExecuteIOTask(() =>
                                  {
                                      if (client.LastNpcMessage.Replies.Count(entry => entry is ContinueDialogReply &&
                                                                                       (entry as ContinueDialogReply).ReplyId == npcReply.ReplyId &&
                                                                                       (entry as ContinueDialogReply).NextMessage.Id == npcReply.NextMessage.Id) > 0)
                                          return;

                                      npcReply.Save();
                                      client.LastNpcMessage.Replies.Add(npcReply);
                                      client.LastNpcMessage.Save();

                                      client.SendChatMessage("Npc reply added");
                                  });
            }
            else if (client.GuessNpcFirstAction != null)
            {
                int npcId = client.MapNpcs[client.GuessNpcFirstAction.npcId].npcId;
                int actionId = client.GuessNpcFirstAction.npcActionId;
                client.GuessNpcFirstAction = null;

                var action = new NpcTalkAction
                                 {
                                     Npc = NpcManager.Instance.GetNpcTemplate(npcId),
                                     Message = currentMessage
                                 };


                ExecuteIOTask(() =>
                                  {
                                      if (action.Npc.Actions.Count(entry => entry is NpcTalkAction) > 0)
                                          return;

                                      action.Npc.Actions.Add(action);

                                      action.Save();
                                      action.Npc.Save();

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

            var npcReply = new EndDialogReply {ReplyId = replyId, Message = client.LastNpcMessage};


            ExecuteIOTask(() =>
                              {
                                  if (client.LastNpcMessage.Replies.Count(entry => entry is EndDialogReply &&
                                                                                   (entry as EndDialogReply).ReplyId == npcReply.ReplyId) > 0)
                                      return;

                                  npcReply.Save();
                                  client.LastNpcMessage.Replies.Add(npcReply);
                                  client.LastNpcMessage.Save();

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
                                  var action = new NpcBuySellAction
                                                   {
                                                       Npc = NpcManager.Instance.GetNpcTemplate(npcId),
                                                       Items = new List<NpcItem>()
                                                   };

                                  if (action.Npc.Actions.Count(entry => entry is NpcBuySellAction) > 0)
                                      return;

                                  action.Save();

                                  foreach (ObjectItemToSellInNpcShop objInfo in npcShopMessage.objectsInfos)
                                  {
                                      var item = new NpcItem
                                                     {
                                                         Item = ItemManager.Instance.GetTemplate(objInfo.objectGID),
                                                         NpcShop = action,
                                                         BuyCriterion = string.Empty,
                                                         CustomPrice = objInfo.objectPrice
                                                     };
                                      action.Items.Add(item);
                                      item.Save();
                                  }

                                  action.Npc.Actions.Add(action);
                                  //action.Npc.Save();

                                  client.SendChatMessage("Npc shop added");
                              });
        }

        public static void BuildMonsterSpell(WorldClient client, GameFightMonsterInformations monster, GameActionFightSpellCastMessage spell)
        {
            ExecuteIOTask(() =>
                              {
                                  var monsterSpell = new MonsterSpell();
                                  MonsterGrade grade = MonsterManager.Instance.GetMonsterGrade(monster.creatureGenericId, monster.creatureGrade);

                                  if (MonsterManager.Instance.GetOneMonsterSpell(entry =>
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

        public static void BuildMapFightPlacement(WorldClient client, Map map, IEnumerable<short> blueCells, IEnumerable<short> redCells)
        {
            if (map.Record.FightPositions != null)
                return;

            ExecuteIOTask(() =>
                              {
                                  map.Record.FightPositions = new MapFightPositionsRecord
                                                                  {
                                                                      BlueCells = blueCells.ToArray(),
                                                                      RedCells = redCells.ToArray(),
                                                                      Map = map.Record
                                                                  };

                                  map.Record.FightPositions.Save();
                                  map.Record.Save();

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
            HandleMonsterSpawn(client, monsterGroup.mainCreatureGenericId, client.CurrentMap);

            foreach (MonsterInGroupInformations monster in monsterGroup.underlings)
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
                                                      SubArea = client.CurrentMap.SubArea.Record
                                                  };

                                  MonsterManager.Instance.AddMonsterSpawn(spawn);
                                  client.SendChatMessage("Monster spawn added");
                              });
        }

        public static void HandleInteractiveObject(WorldClient client, InteractiveElement interactiveElement)
        {
            var ioSpawn = new InteractiveSpawn
                              {
                                  Template = InteractiveManager.Instance.GetTemplate(interactiveElement.elementTypeId),
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
            Proxy.Instance.IOTaskPool.EnqueueTask(action);
        }
    }
}