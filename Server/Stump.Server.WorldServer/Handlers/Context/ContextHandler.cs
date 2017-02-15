using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        private ContextHandler()
        {
            
        }

        [WorldHandler(GameContextCreateRequestMessage.Id)]
        public static void HandleGameContextCreateRequestMessage(WorldClient client, GameContextCreateRequestMessage message)
        {
            if (client.Character.IsInWorld)
            {
                client.Character.SendServerMessage("You are already Logged !");
                return;
            }

            client.Character.LogIn();
        }

        [WorldHandler(GameContextReadyMessage.Id)]
        public static void HandleGameContextReadyMessage(WorldClient client, GameContextReadyMessage message)
        {
            client.Character.OnCharacterContextReady(message.mapId);
        }

        [WorldHandler(GameMapChangeOrientationRequestMessage.Id)]
        public static void HandleGameMapChangeOrientationRequestMessage(WorldClient client, GameMapChangeOrientationRequestMessage message)
        {
            if (client.Character.IsInFight())
                return;

            client.Character.Direction = (DirectionsEnum) message.direction;
            SendGameMapChangeOrientationMessage(client.Character.CharacterContainer.Clients, client.Character);
        }

        [WorldHandler(GameCautiousMapMovementRequestMessage.Id)]
        [WorldHandler(GameMapMovementRequestMessage.Id)]
        public static void HandleGameMapMovementRequestMessage(WorldClient client, GameMapMovementRequestMessage message)
        {
            if (!client.Character.CanMove())
            {
                SendGameMapNoMovementMessage(client, (short)client.Character.Position.Point.X, (short)client.Character.Position.Point.Y);
                return;
            }

            var movementPath = Path.BuildFromCompressedPath(client.Character.Map, message.keyMovements);

            if (message is GameCautiousMapMovementRequestMessage)
                movementPath.SetWalk();

            if (!client.Character.StartMove(movementPath))
                SendGameMapNoMovementMessage(client, (short)client.Character.Position.Point.X, (short)client.Character.Position.Point.Y);
        }

        [WorldHandler(GameMapMovementConfirmMessage.Id)]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementConfirmMessage message)
        {
            client.Character.StopMove();
        }

        [WorldHandler(GameMapMovementCancelMessage.Id)]
        public static void HandleGameMapMovementCancelMessage(WorldClient client, GameMapMovementCancelMessage message)
        {
            client.Character.StopMove(new ObjectPosition(client.Character.Map, message.cellId,
                                                               client.Character.Position.Direction));
        }

        [WorldHandler(ShowCellRequestMessage.Id)]
        public static void HandleShowCellRequestMessage(WorldClient client, ShowCellRequestMessage message)
        {
            if (client.Character.IsFighting())
                client.Character.Fighter.ShowCell(client.Character.Map.Cells[message.cellId]);
            else if (client.Character.IsSpectator())
                client.Character.Spectator.ShowCell(client.Character.Map.Cells[message.cellId]);
        }

        public static void SendGameMapNoMovementMessage(IPacketReceiver client, short cellX, short cellY)
        {
            client.Send(new GameMapNoMovementMessage(cellX, cellY));
        }

        public static void SendGameContextCreateMessage(IPacketReceiver client, sbyte context)
        {
            client.Send(new GameContextCreateMessage(context));
        }

        public static void SendGameContextDestroyMessage(IPacketReceiver client)
        {
            client.Send(new GameContextDestroyMessage());
        }

        public static void SendGameMapChangeOrientationMessage(IPacketReceiver client, ContextActor actor)
        {
            client.Send(new GameMapChangeOrientationMessage(new ActorOrientation(actor.Id,
                                                                         (sbyte) actor.Position.Direction)));
        }

        public static void SendGameContextRemoveElementMessage(IPacketReceiver client, ContextActor actor)
        {
            client.Send(new GameContextRemoveElementMessage(actor.Id));
        }

        public static void SendShowCellSpectatorMessage(IPacketReceiver client, FightSpectator spectator, Cell cell)
        {
            client.Send(new ShowCellSpectatorMessage(spectator.Character.Id, cell.Id, spectator.Character.Name));
        }

        public static void SendShowCellMessage(IPacketReceiver client, ContextActor source, Cell cell)
        {
            client.Send(new ShowCellMessage(source.Id, cell.Id));
        }

        public static void SendGameContextRefreshEntityLookMessage(IPacketReceiver client, ContextActor actor)
        {
            client.Send(new GameContextRefreshEntityLookMessage(actor.Id, actor.Look.GetEntityLook()));
        }

        public static void SendGameMapMovementMessage(IPacketReceiver client, IEnumerable<short> movementsKey, ContextActor actor)
        {
            client.Send(new GameMapMovementMessage(movementsKey, actor.Id));
        }

        public static void SendGameCautiousMapMovementMessage(IPacketReceiver client, IEnumerable<short> movementsKey, ContextActor actor)
        {
            client.Send(new GameCautiousMapMovementMessage(movementsKey, actor.Id));
        }

        public static void SendGameEntitiesDispositionMessage(IPacketReceiver client, IEnumerable<ContextActor> actors)
        {
            client.Send(new GameEntitiesDispositionMessage(actors.Select(entry => entry.GetIdentifiedEntityDispositionInformations())));
        }
    }
}