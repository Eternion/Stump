using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using NpcSpawn = Stump.Server.WorldServer.Database.Npcs.NpcSpawn;
using NpcTemplate = Stump.Server.WorldServer.Database.Npcs.NpcTemplate;

namespace Stump.Plugins.EditorPlugin.Commands
{
    public class NpcEditorCommands : SubCommandContainer
    {
        public NpcEditorCommands()
        {
            Aliases = new[] { "nedit", "npcedit" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Npc editor";
        }
    }

    public class NpcSpawnCommand : SubCommand
    {
        public NpcSpawnCommand()
        {
            Aliases = new[] { "spawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Spawn a npc";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
            AddParameter("map", "map", "Map id", isOptional: true, converter: ParametersConverter.MapConverter);
            AddParameter<short>("cell", "cell", "Cell id", isOptional: true);
            AddParameter("direction", "dir", "Direction", isOptional: true, converter: ParametersConverter.GetEnumConverter<DirectionsEnum>());
        }

        public override void Execute(TriggerBase trigger)
        {
            var template = trigger.Get<NpcTemplate>("npc");
            ObjectPosition position = null;

            if (trigger.IsArgumentDefined("map") && trigger.IsArgumentDefined("cell") && trigger.IsArgumentDefined("direction"))
            {
                var map = trigger.Get<Map>("map");
                var cell = trigger.Get<short>("cell");
                var direction = trigger.Get<DirectionsEnum>("direction");

                position = new ObjectPosition(map, cell, direction);
            }
            else if (trigger is GameTrigger)
            {
                position = (trigger as GameTrigger).Character.Position;
            }

            if (position == null)
            {
                trigger.ReplyError("Position of npc is not defined");
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    var spawn = new NpcSpawn()
                                    {
                                        Template = template,
                                        MapId = position.Map.Id,
                                        CellId = position.Cell.Id,
                                        Direction = position.Direction,
                                        Look = template.Look
                                    };

                    NpcManager.Instance.AddNpcSpawn(spawn);
                    position.Map.Area.ExecuteInContext(() =>
                    {
                        var npc = position.Map.SpawnNpc(spawn);

                        trigger.Reply("Npc {0} spawned", npc.Id);
                    });
                });
        }
    }

    public class NpcUnSpawnCommand : SubCommand
    {
        public NpcUnSpawnCommand()
        {
            Aliases = new[] { "unspawn" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Unspawn the npc by the given contextual id";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter<sbyte>("npcid", "npc", "Npc Contextual id");
            AddParameter("map", "map", "Npc Map", isOptional: true, converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var npcId = trigger.Get<sbyte>("npcid");
            Npc npc;

            if (trigger.IsArgumentDefined("map"))
            {
                npc = trigger.Get<Map>("map").GetActor<Npc>(npcId);
            }
            else if (trigger is GameTrigger)
            {
                npc = (trigger as GameTrigger).Character.Map.GetActor<Npc>(npcId);
            }
            else
            {
                trigger.ReplyError("Npc Map must be defined !");
                return;
            }


            if (npc.Spawn == null)
            {
                trigger.ReplyError("This npc is not saved in database");
            }

            npc.Map.UnSpawnNpc(npc);

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    NpcManager.Instance.RemoveNpcSpawn(npc.Spawn);
                    trigger.Reply("Npc {0} unspawned", npcId);
                });
        }
    }

    public class NpcShopCommand : AddRemoveSubCommand
    {
        public NpcShopCommand()
        {
            Aliases = new[] { "shop" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage npc shop";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
            AddParameter("item", "item", converter: ParametersConverter.ItemTemplateConverter);
            AddParameter<float>("customprice", "price", isOptional: true);
            AddParameter<bool>("max", "max", "Get the max stats when sold", isOptional: true);
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var template = trigger.Get<NpcTemplate>("npc");
            var itemTemplate = trigger.Get<ItemTemplate>("item");
            var shop = template.Actions.OfType<NpcBuySellAction>().FirstOrDefault();

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    if (shop == null)
                    {
                        shop = new NpcBuySellAction(new NpcActionRecord
                        {
                            Type = NpcBuySellAction.Discriminator,
                            Template = template
                        });

                        NpcManager.Instance.AddNpcAction(shop);
                        template.Actions.Add(shop);
                    }

                    var item = new NpcItem()
                    {
                        Item = itemTemplate,
                        CustomPrice = trigger.IsArgumentDefined("customprice") ? (float?)trigger.Get<float>("customprice") : null,
                        NpcShopId = (int)shop.Record.Id,
                        BuyCriterion = string.Empty,
                        MaxStats = trigger.IsArgumentDefined("max")
                    };

                    foreach (var actualItem in shop.Items.Where(x => x.ItemId == item.ItemId).ToArray())
                    {
                        WorldServer.Instance.DBAccessor.Database.Delete(actualItem);
                        shop.Items.Remove(actualItem);
                        trigger.Reply("Item '{0}' remove from '{1}'s' shop", itemTemplate.Name, template.Name);
                    }

                    WorldServer.Instance.DBAccessor.Database.Insert(item);
                    shop.Items.Add(item);
                    trigger.Reply("Item '{0}' added to '{1}'s' shop", itemTemplate.Name, template.Name);
                });
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var template = trigger.Get<NpcTemplate>("npc");
            var itemTemplate = trigger.Get<ItemTemplate>("item");
            var shop = template.Actions.OfType<NpcBuySellAction>().FirstOrDefault();

            if (shop == null)
            {
                trigger.ReplyError("Npc {0} has no shop", template);
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    var items = shop.Items.Where(entry => entry.ItemId == itemTemplate.Id).ToArray();

                    foreach (var item in items)
                    {
                        WorldServer.Instance.DBAccessor.Database.Delete(item);
                        shop.Items.Remove(item);
                        trigger.Reply("Item '{0}' removed from '{1}'s' shop", itemTemplate.Name, template.Name);
                    }
                });
        }
    }

    public class NpcShopSetTokenCommand : SubCommand
    {
        public NpcShopSetTokenCommand()
        {
            Aliases = new[] { "shoptoken" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Define the token needed to trade with the npc";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
            AddParameter("token", "token", "Token item", isOptional: true, converter: ParametersConverter.ItemTemplateConverter);
            AddParameter<bool>("notoken", "no", "Reset the shop as standard", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var npc = trigger.Get<NpcTemplate>("npc");
            var shop = npc.Actions.OfType<NpcBuySellAction>().FirstOrDefault();

            if (shop == null)
            {
                trigger.ReplyError("Npc {0} has no shop", npc);
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
               () =>
               {
                   if (trigger.IsArgumentDefined("notoken"))
                   {
                       shop.Token = null;
                       trigger.Reply("Token removed");
                   }
                   else if (trigger.IsArgumentDefined("token"))
                   {
                       var token = trigger.Get<ItemTemplate>("token");
                       shop.Token = token;
                       trigger.Reply("Npc {0} now sells items for tokens {1}", npc, token);
                   }
                   else
                   {
                       trigger.ReplyError("Define token or -notoken");
                   }

                   NpcManager.Instance.RemoveNpcAction(shop);
               });
        }
    }

    public class NpcShopMaxCommand : SubCommand
    {
        public NpcShopMaxCommand()
        {
            Aliases = new[] { "shopmax" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Set all sold items to max stats";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
            AddParameter("active", "active", "Active or not", true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var npc = trigger.Get<NpcTemplate>("npc");
            var shop = npc.Actions.OfType<NpcBuySellAction>().FirstOrDefault();

            if (shop == null)
            {
                trigger.ReplyError("Npc {0} has no shop", npc);
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
               () =>
               {
                   shop.MaxStats = trigger.Get<bool>("active");

                   NpcManager.Instance.RemoveNpcAction(shop);
               });
        }
    }

    public class NpcShopRemoveAll : SubCommand
    {
        public NpcShopRemoveAll()
        {
            Aliases = new[] { "removeall" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Remove all items from NPC";
            ParentCommandType = typeof(NpcEditorCommands);
            AddParameter("npc", "npc", "Npc Template id", converter: ParametersConverter.NpcTemplateConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var npc = trigger.Get<NpcTemplate>("npc");
            var shop = npc.Actions.OfType<NpcBuySellAction>().FirstOrDefault();

            if (shop == null)
            {
                trigger.ReplyError("Npc {0} has no shop", npc);
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(
                () =>
                {
                    var items = shop.Items.ToArray();

                    foreach (var item in items)
                    {
                        WorldServer.Instance.DBAccessor.Database.Delete(item);
                        shop.Items.Remove(item);

                        trigger.Reply("All Items has been removed from '{0}'s' shop", npc.Name);
                    }
                });
        }
    }
}