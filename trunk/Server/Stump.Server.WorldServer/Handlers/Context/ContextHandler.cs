using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;

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
            SendGameContextDestroyMessage(client);
            SendGameContextCreateMessage(client, 1);

            /*CharacterHandler.SendCharacterStatsListMessage(client);
            CharacterHandler.SendLifePointsRegenBeginMessage(client, 60);*/

            RolePlay.ContextHandler.SendCurrentMapMessage(client, client.ActiveCharacter.Map.Id);
            BasicHandler.SendBasicTimeMessage(client);

            //World.Instance.SendMessageOfTheDay(client.ActiveCharacter);
        }

        [WorldHandler(GameMapChangeOrientationRequestMessage.Id)]
        public static void HandleGameMapChangeOrientationRequestMessage(WorldClient client,
                                                                        GameMapChangeOrientationRequestMessage message)
        {
            client.ActiveCharacter.Direction = (DirectionsEnum) message.direction;
            client.ActiveCharacter.Map.Do(
                charac => SendGameMapChangeOrientationMessage(charac.Client, client.ActiveCharacter));
        }

        // todo : get and check whole path
        [WorldHandler(GameMapMovementRequestMessage.Id)]
        public static void HandleGameMapMovementRequestMessage(WorldClient client, GameMapMovementRequestMessage message)
        {
            /*if (!client.ActiveCharacter.CanMove())
                return;

            var movementPath = new MovementPath(client.ActiveCharacter.Map, message.keyMovements);

            if (client.ActiveCharacter.IsInFight)
            {
                client.ActiveCharacter.Fighter.Move(movementPath);
            }
            else
            {
                client.ActiveCharacter.Move(movementPath);
            }*/
        }

        [WorldHandler(GameMapMovementConfirmMessage.Id)]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementConfirmMessage message)
        {
            //client.ActiveCharacter.MovementEnded();
        }

        [WorldHandler(GameMapMovementCancelMessage.Id)]
        public static void HandleGameMapMovementCancelMessage(WorldClient client, GameMapMovementCancelMessage message)
        {
            // todo : check if cell is available and if moving

            /*client.ActiveCharacter.StopMove(new ObjectPosition(client.ActiveCharacter.Map, (ushort) message.cellId,
                                                               client.ActiveCharacter.Position.Direction));*/
        }

        public static void SendGameContextCreateMessage(WorldClient client, byte context)
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
                                                                         (byte) actor.Position.Direction)));
        }

        public static void SendGameContextRemoveElementMessage(WorldClient client, ContextActor actor)
        {
            client.Send(new GameContextRemoveElementMessage(actor.Id));
        }

        public static void SendGameContextRefreshEntityLookMessage(WorldClient client, ContextActor actor)
        {
            client.Send(new GameContextRefreshEntityLookMessage(actor.Id, actor.Look));
        }

        public static void SendGameMapMovementMessage(WorldClient client, IEnumerable<short> movementsKey, RolePlayActor actor)
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