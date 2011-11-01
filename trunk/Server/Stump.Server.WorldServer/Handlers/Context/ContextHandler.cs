using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        private ContextHandler()
        {
            
        }

        [WorldHandler(GameContextCreateRequestMessage.Id)]
        public static void HandleGameContextCreateRequestMessage(WorldClient client,
                                                                 GameContextCreateRequestMessage message)
        {
            if (client.ActiveCharacter.InWorld)
                return;

            SendGameContextDestroyMessage(client);
            SendGameContextCreateMessage(client, 1);

            CharacterHandler.SendCharacterStatsListMessage(client);

            client.ActiveCharacter.LogIn();
            client.ActiveCharacter.StartRegen();
        }

        [WorldHandler(GameMapChangeOrientationRequestMessage.Id)]
        public static void HandleGameMapChangeOrientationRequestMessage(WorldClient client,
                                                                        GameMapChangeOrientationRequestMessage message)
        {
            client.ActiveCharacter.Direction = (DirectionsEnum) message.direction;
            client.ActiveCharacter.Map.ForEach(
                charac => SendGameMapChangeOrientationMessage(charac.Client, client.ActiveCharacter));
        }

        // todo : get and check whole path
        [WorldHandler(GameMapMovementRequestMessage.Id)]
        public static void HandleGameMapMovementRequestMessage(WorldClient client, GameMapMovementRequestMessage message)
        {
            if (!client.ActiveCharacter.CanMove())
                return;

            var movementPath = Path.BuildFromCompressedPath(client.ActiveCharacter.Map, message.keyMovements);

            client.ActiveCharacter.StartMove(movementPath);
        }

        [WorldHandler(GameMapMovementConfirmMessage.Id)]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementConfirmMessage message)
        {
            client.ActiveCharacter.StopMove();
        }

        [WorldHandler(GameMapMovementCancelMessage.Id)]
        public static void HandleGameMapMovementCancelMessage(WorldClient client, GameMapMovementCancelMessage message)
        {
            client.ActiveCharacter.StopMove(new ObjectPosition(client.ActiveCharacter.Map, message.cellId,
                                                               client.ActiveCharacter.Position.Direction));
        }

        [WorldHandler(ShowCellRequestMessage.Id)]
        public static void HandleShowCellRequestMessage(WorldClient client, ShowCellRequestMessage message)
        {
            if (!client.ActiveCharacter.IsFighting())
                return;

            client.ActiveCharacter.Fighter.ShowCell(client.ActiveCharacter.Map.Cells[message.cellId]);
        }

        public static void SendGameContextCreateMessage(WorldClient client, sbyte context)
        {
            client.Send(new GameContextCreateMessage(context));
        }

        public static void SendGameContextDestroyMessage(WorldClient client)
        {
            client.Send(new GameContextDestroyMessage());
        }

        public static void SendGameMapChangeOrientationMessage(WorldClient client, ContextActor actor)
        {
            client.Send(
                new GameMapChangeOrientationMessage(new ActorOrientation(actor.Id,
                                                                         (sbyte) actor.Position.Direction)));
        }

        public static void SendGameContextRemoveElementMessage(WorldClient client, ContextActor actor)
        {
            client.Send(new GameContextRemoveElementMessage(actor.Id));
        }

        public static void SendShowCellMessage(WorldClient client, ContextActor source, Cell cell)
        {
            client.Send(new ShowCellMessage(source.Id, cell.Id));
        }

        public static void SendGameContextRefreshEntityLookMessage(WorldClient client, ContextActor actor)
        {
            client.Send(new GameContextRefreshEntityLookMessage(actor.Id, actor.Look));
        }

        public static void SendGameMapMovementMessage(WorldClient client, IEnumerable<short> movementsKey, ContextActor actor)
        {
            client.Send(new GameMapMovementMessage(movementsKey, actor.Id));
        }

        public static void SendGameEntitiesDispositionMessage(WorldClient client,
                                                              IEnumerable<ContextActor> actors)
        {
            client.Send(
                new GameEntitiesDispositionMessage(
                    actors.Select(entry => entry.GetIdentifiedEntityDispositionInformations())));
        }
    }
}