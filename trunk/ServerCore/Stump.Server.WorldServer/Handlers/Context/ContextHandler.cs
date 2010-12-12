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
using System.Linq;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(GameMapChangeOrientationRequestMessage))]
        public static void HandleGameMapChangeOrientationRequestMessage(WorldClient client, GameMapChangeOrientationRequestMessage message)
        {
            client.ActiveCharacter.Direction = (int) message.direction;
            client.ActiveCharacter.Map.CallOnAllCharactersWithoutFighters(charac => SendGameMapChangeOrientationMessage(charac.Client, client.ActiveCharacter));
        }

        [WorldHandler(typeof(GameMapMovementRequestMessage))]
        public static void HandleGameMapMovementRequestMessage(WorldClient client, GameMapMovementRequestMessage message)
        {
            var cellid = (int) (message.keyMovements[message.keyMovements.Count - 1] & 0x0FFF);

            if (client.ActiveCharacter.IsInFight)
            {
                client.ActiveCharacter.CurrentFight.MoveFighter(client.ActiveCharacter, cellid, message.keyMovements);
            }
            else
            {
                client.ActiveCharacter.CellId = cellid;
                client.ActiveCharacter.Map.HandleCharacterMovement(client.ActiveCharacter, message.keyMovements);
            }
        }

        [WorldHandler(typeof(GameMapMovementConfirmMessage))]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementConfirmMessage message)
        {

        }

        [WorldHandler(typeof(GameMapMovementCancelMessage))]
        public static void HandleGameMapMovementCancelMessage(WorldClient client, GameMapMovementCancelMessage message)
        {
            // todo : check if cell is available and if moving

            client.ActiveCharacter.CellId = (int) message.cellId;
        }

        public static void SendGameContextCreateMessage(WorldClient client, byte context)
        {
            client.Send(new GameContextCreateMessage(context));
        }

        public static void SendGameContextDestroyMessage(WorldClient client)
        {
            client.Send(new GameContextDestroyMessage());
        }

        public static void SendGameMapChangeOrientationMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameMapChangeOrientationMessage(new ActorOrientation((int) entity.Id, (uint) entity.Direction)));
        }

        public static void SendGameContextRemoveElementMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameContextRemoveElementMessage((int) entity.Id));
        }

        public static void SendGameContextRefreshEntityLookMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameContextRefreshEntityLookMessage((int) entity.Id, entity.ToNetworkEntityLook()));
        }

        public static void SendGameMapMovementMessage(WorldClient client, List<uint> keymovements, Entity entity)
        {
            client.Send(new GameMapMovementMessage(keymovements, (int) entity.Id));
        }
    }
}