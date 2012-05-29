using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
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
using Stump.Server.WorldServer.Database.World.Maps;
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
                    Direction = (DirectionsEnum)client.Disposition.direction,
                };

                ExecuteIOTask(() =>
                {
                    if (npcReply.Message.Replies.Count(entry => entry is TeleportReply &&
                                                                     ( entry as TeleportReply ).ReplyId == npcReply.ReplyId) > 0)
                        return;

                    npcReply.Save();
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

                var skill = new SkillTeleportRecord
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

                                      if (io.Template != null && io.Template.Skills.Count(entry => entry is SkillTeleportRecord &&
                                                                                                   (entry as SkillTeleportRecord).CellId == skill.CellId &&
                                                                                                   (entry as SkillTeleportRecord).MapId == skill.MapId &&
                                                                                                   (entry as SkillTeleportRecord).Direction == skill.Direction) > 0)
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

                var cell = (short) client.GuessCellTrigger.Value;
                client.GuessCellTrigger = null;

                if (!client.IsCellTriggerValid())
                    return;

                var trigger = new TeleportTriggerRecord
                                  {
                                      MapId = client.LastMap.Id,
                                      CellId = cell,
                                      TriggerType = CellTriggerType.END_MOVE_ON,
                                      Condition = string.Empty,
                                      DestinationCellId = client.Disposition.cellId,
                                      DestinationMapId = client.CurrentMap.Id,
                                  };



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
                                      if (npcReply.Message.Replies.Count(entry => entry is ContinueDialogReply &&
                                                                                       (entry as ContinueDialogReply).ReplyId == npcReply.ReplyId &&
                                                                                       (entry as ContinueDialogReply).NextMessage.Id == npcReply.NextMessage.Id) > 0)
                                          return;

                                      npcReply.Save();
                                      npcReply.Message.Replies.Add(npcReply);

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
                                     NpcId = npcId,
                                     Message = currentMessage
                                 };


                ExecuteIOTask(() =>
                                  {
                                      var npc = NpcManager.Instance.GetNpcTemplate(npcId);

                                      if (npc.Actions.Count(entry => entry is NpcTalkAction) > 0)
                                          return;

                                      action.Save();
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

            var npcReply = new EndDialogReply {ReplyId = replyId, Message = client.LastNpcMessage};


            ExecuteIOTask(() =>
                              {
                                  if (npcReply.Message.Replies.Count(entry => entry is EndDialogReply &&
                                                                                   (entry as EndDialogReply).ReplyId == npcReply.ReplyId) > 0)
                                      return;

                                  npcReply.Save();
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
                                  var npc = NpcManager.Instance.GetNpcTemplate(npcId);

                                  var action = new NpcBuySellAction
                                                   {
                                                       NpcId = npcId,
                                                   };

                                  if (npc.Actions.Count(entry => entry is NpcBuySellAction) > 0)
                                      return;

                                  using (var session = new SessionScope(FlushAction.Never))
                                  {
                                      action.Save();

                                      foreach (ObjectItemToSellInNpcShop objInfo in npcShopMessage.objectsInfos)
                                      {
                                          var item = new NpcItem
                                                         {
                                                             Item = ItemManager.Instance.TryGetTemplate(objInfo.objectGID),
                                                             NpcShopId = (int) action.Id,
                                                             BuyCriterion = string.Empty,
                                                             CustomPrice = objInfo.objectPrice
                                                         };

                                          item.Save();
                                          action.Items.Add(item);
                                      }

                                      npc.Actions.Add(action);
                                      session.Flush();
                                  }

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

        public static void BuildMapFightPlacement(WorldClient client, Map map, IEnumerable<short> blueCells, IEnumerable<short> redCells)
        {
            if (map.Record.BlueFightCells.Length > 0 && map.Record.RedFightCells.Length > 0)
                return;

            ExecuteIOTask(() =>
                              {
                                  map.Record.BlueFightCells = blueCells.ToArray();
                                  map.Record.RedFightCells = redCells.ToArray();

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