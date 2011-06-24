
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Commands
{
    public class ItemCommand : WorldCommand
    {
        public ItemCommand()
        {
            Aliases = new[] {"item"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Provides many commands to manage items";
        }

        public override void Execute(TriggerBase trigger)
        {
            throw new DummyCommandException();
        }
    }

    public class ItemAddCommand : WorldSubCommand
    {
        public ItemAddCommand()
        {
            Aliases = new[] {"add", "new"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Add an item to the targeted character";
            ParentCommand = typeof (ItemCommand);
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<ItemTemplate>("template", "item", "Item to add", converter:ParametersConverter.ItemTemplateConverter),
                    new ParameterDefinition<Character>("target", "t", "Character who will receive the item", true, converter:ParametersConverter.CharacterConverter),
                    new ParameterDefinition<uint>("amount", "amount", "Amount of items to add", true, 1),
                    new ParameterDefinition<bool>("max", "max", "Set item's effect to maximal values", true, false)
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemTemplate = trigger.Get<ItemTemplate>("template");
            Character target;

            if (!trigger.IsArgumentDefined("target") && trigger is IInGameTrigger)
                target = (trigger as IInGameTrigger).Character;
            else
                target = trigger.Get<Character>("target");

            Item addedItem =
                target.Inventory.AddItem(
                    itemTemplate,
                    trigger.Get<uint>("amount"));

            if (addedItem == null)
                trigger.Reply("Item '{0}'({1}) can't be add for an unknown reason", itemTemplate.Name, itemTemplate.Id);
            else if (trigger is IInGameTrigger && (trigger as IInGameTrigger).Character.Id == target.Id)
                trigger.Reply("Added '{0}'({1}) to your inventory.", itemTemplate.Name, itemTemplate.Id);
            else
                trigger.Reply("Added '{0}'({1}) to '{2}' inventory.", itemTemplate.Name, itemTemplate.Id, target.Name);
        }
    }

    public class ItemListCommand : WorldSubCommand
    {
        public ItemListCommand()
        {
            Aliases = new[] { "list", "ls" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Lists loaded items or items from an inventory with a search pattern";
            ParentCommand = typeof(ItemCommand);
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<string>("pattern", "p", "Search pattern (see docs)", true, "*"),
                    new ParameterDefinition<Character>("target", "t", "Where items will be search", true, converter:ParametersConverter.CharacterConverter),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            if(trigger.IsArgumentDefined("target"))
            {
                var target = trigger.Get<Character>("target");

                var items = ItemManager.GetItemsByPattern(trigger.Get<string>("pattern"), target.Inventory.Items);

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1}) Amount:{2} Guid:{3}", item.Template.Name, item.Template.Id, item.Stack, item.Guid);
                }
            }
            else
            {
                var items = ItemManager.GetItemsByPattern(trigger.Get<string>("pattern"));

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1})", item.Name, item.Id);
                }
            }
        }
    }
}