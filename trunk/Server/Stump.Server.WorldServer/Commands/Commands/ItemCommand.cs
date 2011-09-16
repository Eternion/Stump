using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Items;

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

    public class ItemAddCommand : SubCommand
    {
        public ItemAddCommand()
        {
            Aliases = new[] {"add", "new"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Add an item to the targeted character";
            ParentCommand = typeof (ItemCommand);

            AddParameter("template", "item", "Item to add", converter:ParametersConverter.ItemTemplateConverter);
            AddParameter("target", "t", "Character who will receive the item", isOptional:true, converter:ParametersConverter.CharacterConverter);
            AddParameter("amount", "amount", "Amount of items to add", 1u);
            AddParameter<bool>("max", "max", "Set item's effect to maximal values", isOptional:true);
            
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemTemplate = trigger.Get<ItemTemplate>("template");
            Character target;

            if (!trigger.IsArgumentDefined("target") && trigger is GameTrigger)
                target = (trigger as GameTrigger).Character;
            else
                target = trigger.Get<Character>("target");

            Item addedItem =
                target.Inventory.AddItem(
                    itemTemplate,
                    trigger.Get<uint>("amount"));

            if (addedItem == null)
                trigger.Reply("Item '{0}'({1}) can't be add for an unknown reason", itemTemplate.Name, itemTemplate.Id);
            else if (trigger is GameTrigger && (trigger as GameTrigger).Character.Id == target.Id)
                trigger.Reply("Added '{0}'({1}) to your inventory.", itemTemplate.Name, itemTemplate.Id);
            else
                trigger.Reply("Added '{0}'({1}) to '{2}' inventory.", itemTemplate.Name, itemTemplate.Id, target.Name);
        }
    }

    public class ItemListCommand : SubCommand
    {
        [Variable]
        public static readonly int LimitItemList = 20;

        public ItemListCommand()
        {
            Aliases = new[] { "list", "ls" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Lists loaded items or items from an inventory with a search pattern";
            ParentCommand = typeof(ItemCommand);
            AddParameter("pattern", "p", "Search pattern (see docs)", "*");
            AddParameter("target", "t", "Where items will be search", converter:ParametersConverter.CharacterConverter, isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if(trigger.IsArgumentDefined("target"))
            {
                var target = trigger.Get<Character>("target");

                var items = ItemManager.Instance.GetItemsByPattern(trigger.Get<string>("pattern"), target.Inventory.Items);

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1}) Amount:{2} Guid:{3}", item.Template.Name, item.Template.Id, item.Stack, item.Guid);
                }
            }
            else
            {
                var items = ItemManager.Instance.GetItemsByPattern(trigger.Get<string>("pattern"));

                int counter = 0;
                foreach (var item in items)
                {
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
}