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
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        // todo : manage amounts
        [WorldHandler(typeof(ObjectSetPositionMessage))]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if (!Enum.IsDefined(typeof(CharacterInventoryPositionEnum), (int)message.position))
            {
                return;
            }

            client.ActiveCharacter.Inventory.MoveItem(message.objectUID, (CharacterInventoryPositionEnum)message.position);
            SendInventoryWeightMessage(client);
        }

        [WorldHandler(typeof(ObjectDeleteMessage))]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            client.ActiveCharacter.Inventory.DeleteItem(message.objectUID, message.quantity);

            SendInventoryWeightMessage(client);
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(
                new InventoryContentMessage(
                    client.ActiveCharacter.Inventory.Items.Select(entry => entry.ToNetworkItem()).ToList(),
                    (uint) client.ActiveCharacter.Kamas));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage(client.ActiveCharacter.Inventory.Weight,
                                                   client.ActiveCharacter.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(WorldClient client, bool remote, uint kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(WorldClient client, Item addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.ToNetworkItem()));
        }

        public static void SendObjectsAddedMessage(WorldClient client, IEnumerable<Item> addeditems)
        {
            client.Send(new ObjectsAddedMessage(addeditems.Select(entry => entry.ToNetworkItem()).ToList()));
        }

        public static void SendObjectDeletedMessage(WorldClient client, long guid)
        {
            client.Send(new ObjectDeletedMessage((uint) guid));
        }

        public static void SendObjectsDeletedMessage(WorldClient client, IEnumerable<long> guids)
        {
            client.Send(new ObjectsDeletedMessage(guids.Cast<uint>().ToList()));
        }

        public static void SendObjectModifiedMessage(WorldClient client, Item item)
        {
            client.Send(new ObjectModifiedMessage(item.ToNetworkItem()));
        }

        public static void SendObjectMovementMessage(WorldClient client, Item movedItem)
        {
            client.Send(new ObjectMovementMessage((uint) movedItem.Guid, (uint) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(WorldClient client, Item modifieditem)
        {
            client.Send(new ObjectQuantityMessage((uint) modifieditem.Guid, modifieditem.Stack));
        }

        public static void SendObjectErrorMessage(WorldClient client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((byte)error));
        }
    }
}