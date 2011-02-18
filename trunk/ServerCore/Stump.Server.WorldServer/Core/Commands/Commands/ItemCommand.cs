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
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.World.Actors.Character;

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
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<Character>("target", "t", "Character who will receive the item", converter:ParametersConverter.CharacterConverter),
                    new CommandParameter<ItemTemplate>("template", "item", "Item to add", converter:ParametersConverter.ItemTemplateConverter),
                    new CommandParameter<uint>("amount", "amount", "Amount of items to add", true, 1),
                    new CommandParameter<bool>("max", "max", "Set item's effect to maximal values", true, false)
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var itemTemplate = trigger.GetArgument<ItemTemplate>("template");
            var target = trigger.GetArgument<Character>("target");

            Item addedItem =
                target.Inventory.AddItem(
                    itemTemplate,
                    trigger.GetArgument<uint>("amount"));

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
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("pattern", "p", "Search pattern (see docs)", true, "*"),
                    new CommandParameter<Character>("target", "t", "Character who will receive the item", true, converter:ParametersConverter.CharacterConverter),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            if(trigger.ArgumentExists("target"))
            {
                var target = trigger.GetArgument<Character>("target");

                var items = ItemManager.GetItemsByPattern(trigger.GetArgument<string>("pattern"), target.Inventory.Items);

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1}) Amount:{2} Guid:{3}", item.Template.Name, item.Template.Id, item.Stack, item.Guid);
                }
            }
            else
            {
                var items = ItemManager.GetItemsByPattern(trigger.GetArgument<string>("pattern"));

                foreach (var item in items)
                {
                    trigger.Reply("'{0}'({1})", item.Name, item.Id);
                }
            }
        }
    }
}