using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class ItemCommand : SubCommandContainer
    {
        public ItemCommand()
        {
            Aliases = new[] {"item"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Provides many commands to manage items";
        }
    }

    public class ItemAddCommand : TargetSubCommand
    {
        public ItemAddCommand()
        {
            Aliases = new[] {"add", "new"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Add an item to the targeted character";
            ParentCommand = typeof (ItemCommand);

            AddParameter("template", "item", "Item to add", converter:ParametersConverter.ItemTemplateConverter);
            AddTargetParameter(true, "Character who will receive the item");
            AddParameter("amount", "amount", "Amount of items to add", 1u);
            AddParameter<bool>("max", "max", "Set item's effect to maximal values", isOptional:true);
            
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemTemplate = trigger.Get<ItemTemplate>("template");
            var target = GetTarget(trigger);

            var item = ItemManager.Instance.CreatePlayerItem(target, itemTemplate, trigger.Get<uint>("amount"), trigger.IsArgumentDefined("max"));

            target.Inventory.AddItem(item);

            if (item == null)
                trigger.ReplyError("Item '{0}'({1}) can't be add for an unknown reason", itemTemplate.Name, itemTemplate.Id);
            else if (trigger is GameTrigger && (trigger as GameTrigger).Character.Id == target.Id)
                trigger.Reply("Added '{0}'({1}) to your inventory.", itemTemplate.Name, itemTemplate.Id);
            else
                trigger.Reply("Added '{0}'({1}) to '{2}' inventory.", itemTemplate.Name, itemTemplate.Id, target.Name);
        }
    }

    public class ItemRemoveCommand : TargetSubCommand
    {
        public ItemRemoveCommand()
        {
            Aliases = new[] { "remove", "delete" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Delete an item from the target";
            ParentCommand = typeof(ItemCommand);

            AddParameter("template", "item", "Item to remove", converter: ParametersConverter.ItemTemplateConverter);
            AddTargetParameter(true, "Character who will lose the item");
            AddParameter<uint>("amount", "amount", "Amount of items to remove", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemTemplate = trigger.Get<ItemTemplate>("template");
            var target = GetTarget(trigger);

            var item = target.Inventory.TryGetItem(itemTemplate);

            if (item != null)
            {
                if (trigger.IsArgumentDefined("amount"))
                {
                    target.Inventory.RemoveItem(item, trigger.Get<uint>("amount"));
                    trigger.ReplyBold("'{0}'x{1} removed from {1}'s inventory", itemTemplate.Name,
                                      trigger.Get<uint>("amount"), target);
                }
                else
                {
                    target.Inventory.RemoveItem(item);
                    trigger.ReplyBold("Item {0} removed from {1}'s inventory", itemTemplate.Name, target);
                }
            }
            else
            {
                trigger.ReplyError("{0} hasn't item {1}");
            }
        }
    }

    public class ItemListCommand : SubCommand
    {
        [Variable]
        public static readonly int LimitItemList = 50;

        public ItemListCommand()
        {
            Aliases = new[] { "list", "ls" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Lists loaded items or items from an inventory with a search pattern";
            ParentCommand = typeof(ItemCommand);
            AddParameter("pattern", "p", "Search pattern (see docs)", "*");
            AddParameter("target", "t", "Where items will be search", converter:ParametersConverter.CharacterConverter, isOptional:true);
            AddParameter("page", "page", "Page number of the list (starts at 0)", 0, isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if(trigger.IsArgumentDefined("target"))
            {
                var target = trigger.Get<Character>("target");

                var items = ItemManager.Instance.GetItemsByPattern(trigger.Get<string>("pattern"), target.Inventory);

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1}) Amount:{2} Guid:{3}", item.Template.Name, item.Template.Id, item.Stack, item.Guid);
                }
            }
            else
            {
                var items = ItemManager.Instance.GetItemsByPattern(trigger.Get<string>("pattern"));
                var startIndex = trigger.Get<int>("page") * LimitItemList;

                int counter = 0;
                var enumerator = items.GetEnumerator();

                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    if (i < startIndex)
                        continue;

                    var item = enumerator.Current;
                    if (counter >= LimitItemList)
                    {
                        trigger.Reply("... (limit reached : {0})", LimitItemList);
                        break;
                    }

                    trigger.Reply("'{0}'({1})", item.Name, item.Id);
                    counter++;
                }

                if (counter == 0)
                    trigger.Reply("No results");
            }
        }
    }

    public class ItemShowInvCommand : SubCommand
    {
        public ItemShowInvCommand()
        {
            Aliases = new[] { "showinv" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Show items of the target into your inventory";
            ParentCommand = typeof(ItemCommand);
            AddParameter("target", "t", "Where items will be search", converter: ParametersConverter.CharacterConverter, isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.IsArgumentDefined("target"))
            {
                var target = trigger.Get<Character>("target");
                var source = ((GameTrigger)trigger).Character.Client;

                source.Send(
                    new InventoryContentMessage(
                        target.Inventory.Select(entry => entry.GetObjectItem()),
                        target.Inventory.Kamas));
            }
            else
            {
                trigger.ReplyError("Please define a target");
            }
        }
    }

    public class ItemAddSetCommand : TargetSubCommand
    {
        public ItemAddSetCommand()
        {
            Aliases = new[] {"addset"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Add the entire itemset to the targeted character";
            ParentCommand = typeof (ItemCommand);

            AddParameter("template", "itemset", "Itemset to add", converter: ParametersConverter.ItemSetTemplateConverter);
            AddTargetParameter(true, "Character who will receive the itemset");
            AddParameter<bool>("max", "max", "Set item's effect to maximal values", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemSet = trigger.Get<ItemSetTemplate>("template");
            var target = GetTarget(trigger);

            foreach (ItemTemplate template in itemSet.Items)
            {

                var item = ItemManager.Instance.CreatePlayerItem(target, template, 1, trigger.IsArgumentDefined("max"));

                target.Inventory.AddItem(item);

                if (item == null)
                    trigger.Reply("Item '{0}'({1}) can't be add for an unknown reason", template.Name, template.Id);
                else if (trigger is GameTrigger && (trigger as GameTrigger).Character.Id == target.Id)
                    trigger.Reply("Added '{0}'({1}) to your inventory.", template.Name, template.Id);
                else
                    trigger.Reply("Added '{0}'({1}) to '{2}' inventory.", template.Name, template.Id, target.Name);
            }
        }
    }

    public class ItemAddTypeCommand : TargetSubCommand
    {
        public ItemAddTypeCommand()
        {
            Aliases = new[] {"addtype"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Add all the items match with typeId.";
            ParentCommand = typeof (ItemCommand);

            AddParameter<int>("typeid", "type", "TypeId to add");
            AddTargetParameter(true, "Character who will receive the items");
        }

        public override void Execute(TriggerBase trigger)
        {
            var typeId = trigger.Get<int>("typeid");
            var target = GetTarget(trigger);

            var items = ItemManager.Instance.GetTemplates();

            foreach (var item in items)
            {
                if (item.TypeId != typeId)
                    continue;

                var cItem = ItemManager.Instance.CreatePlayerItem(target, item, 1);
                target.Inventory.AddItem(cItem);

                if (cItem == null)
                    trigger.Reply("Item '{0}'({1}) can't be add for an unknown reason", item.Name, item.Id);
                else if (trigger is GameTrigger && (trigger as GameTrigger).Character.Id == target.Id)
                    trigger.Reply("Added '{0}'({1}) to your inventory.", item.Name, item.Id);
                else
                    trigger.Reply("Added '{0}'({1}) to '{2}' inventory.", item.Name, item.Id, target.Name);
            }
        }
    }

    public class ItemDelTypeCommand : TargetSubCommand
    {
        public ItemDelTypeCommand()
        {
            Aliases = new[] { "deltype" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Remove all the items match with typeId.";
            ParentCommand = typeof(ItemCommand);

            AddParameter<int>("typeid", "type", "TypeId to remove");
            AddTargetParameter(true, "Character who will remove the items");
        }

        public override void Execute(TriggerBase trigger)
        {
            var typeId = trigger.Get<int>("typeid");
            var target = GetTarget(trigger);

            var itemsToDelete = target.Inventory.Where(x => x.Template.TypeId == typeId).ToArray();

            foreach (var item in itemsToDelete)
            {
                if (item.Template.TypeId == typeId)
                {
                    target.Inventory.RemoveItem(item);
                    trigger.ReplyBold("Item {0} removed from {1}'s inventory", item.Template.Name, target);
                }
            }
        }
    }
}