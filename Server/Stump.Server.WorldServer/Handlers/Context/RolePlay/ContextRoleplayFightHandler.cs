using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Fights;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(GameRolePlayAttackMonsterRequestMessage.Id)]
        public static void HandleGameRolePlayAttackMonsterRequestMessage(WorldClient client, GameRolePlayAttackMonsterRequestMessage message)
        {
            var map = client.Character.Map;
            var monster = map.GetActor<MonsterGroup>(entry => entry.Id == message.monsterGroupId);

            if (monster != null && monster.Position.Cell == client.Character.Position.Cell)
                monster.FightWith(client.Character);
        }

        [WorldHandler(GameFightPlacementSwapPositionsRequestMessage.Id)]
        public static void HandleGameFightPlacementSwapPositionsRequestMessage(WorldClient client, GameFightPlacementSwapPositionsRequestMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            if (client.Character.Fighter.Position.Cell.Id != message.cellId)
            {
                var cell = client.Character.Fight.Map.Cells[message.cellId];
                var target = client.Character.Fighter.Team.GetOneFighter(cell) as CharacterFighter;

                if (target == null)
                    return;

                if (client.Character.Fighter.IsTeamLeader())
                    client.Character.Fighter.SwapPrePlacement(target);
                else
                {
                    var swapRequest = new SwapRequest(client.Character, target.Character);
                    swapRequest.Open();
                }
            }
        }

        [WorldHandler(GameFightPlacementSwapPositionsAcceptMessage.Id)]
        public static void HandleGameFightPlacementSwapPositionsAcceptMessage(WorldClient client, GameFightPlacementSwapPositionsAcceptMessage message)
        {
            if (!client.Character.IsInRequest() || !(client.Character.RequestBox is SwapRequest))
                return;

            if (message.requestId == client.Character.RequestBox.Source.Id)
                client.Character.RequestBox.Accept();
        }

        [WorldHandler(GameFightPlacementSwapPositionsCancelMessage.Id)]
        public static void HandleGameFightPlacementSwapPositionsCancelMessage(WorldClient client, GameFightPlacementSwapPositionsCancelMessage message)
        {
            if (!client.Character.IsInRequest() || !(client.Character.RequestBox is SwapRequest))
                return;

            if (message.requestId == client.Character.RequestBox.Source.Id)
            {
                if (client.Character == client.Character.RequestBox.Source)
                    client.Character.RequestBox.Cancel();
                else
                    client.Character.RequestBox.Deny();
            }
        }

        public static void SendGameRolePlayPlayerFightFriendlyAnsweredMessage(IPacketReceiver client, Character replier,
                                                                              Character source, Character target,
                                                                              bool accepted)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(replier.Id,
                                                                           source.Id,
                                                                           target.Id,
                                                                           accepted));
        }

        public static void SendGameRolePlayPlayerFightFriendlyRequestedMessage(IPacketReceiver client, Character requester,
                                                                               Character source,
                                                                               Character target)
        {
            client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(requester.Id, source.Id,
                                                                            target.Id));
        }

        public static void SendGameRolePlayArenaUpdatePlayerInfosMessage(IPacketReceiver client, Character character)
        {
            client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage(new ArenaRankInfos((short) character.ArenaRank, 
                (short) character.ArenaMaxRank, (short) character.ArenaDailyMatchsWon, (short) character.ArenaDailyMatchsCount)));
        }

        public static void SendGameRolePlayAggressionMessage(IPacketReceiver client, Character challenger, Character defender)
        {
            client.Send(new GameRolePlayAggressionMessage(challenger.Id, defender.Id));
        }

        public static void SendGameFightPlacementSwapPositionsMessage(IPacketReceiver client, IEnumerable<ContextActor> actors)
        {
            client.Send(new GameFightPlacementSwapPositionsMessage(actors.Select(entry => entry.GetIdentifiedEntityDispositionInformations())));
        }

        public static void SendGameFightPlacementSwapPositionsOfferMessage(IPacketReceiver client, Character source, Character target)
        {
            client.Send(new GameFightPlacementSwapPositionsOfferMessage(source.Id, source.Fighter.Id, source.Cell.Id, target.Fighter.Id, target.Cell.Id));
        }

        public static void SendGameFightPlacementSwapPositionsCancelledMessage(IPacketReceiver client, Character source, Character canceller)
        {
            client.Send(new GameFightPlacementSwapPositionsCancelledMessage(source.Fighter.Id, canceller.Fighter.Id));
        }
    }
}