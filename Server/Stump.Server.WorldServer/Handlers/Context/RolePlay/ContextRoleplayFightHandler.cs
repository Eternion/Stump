using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(GameRolePlayAttackMonsterRequestMessage.Id)]
        public static void HandleGameRolePlayAttackMonsterRequestMessage(WorldClient client, GameRolePlayAttackMonsterRequestMessage message)
        {
            var map = client.Character.Map;
            var monster = map.GetActor<MonsterGroup>(entry => entry.Id == message.monsterGroupId);

            if (monster != null)
                monster.FightWith(client.Character);
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
            client.Send(new GameRolePlayArenaUpdatePlayerInfosMessage((short) character.ArenaRank, 
                (short) character.ArenaDailyMaxRank, (short) character.ArenaMaxRank, 
                (short) character.ArenaDailyMatchsWon, (short) character.ArenaDailyMatchsCount));
        }

        public static void SendGameRolePlayAggressionMessage(IPacketReceiver client, Character challenger, Character defender)
        {
            client.Send(new GameRolePlayAggressionMessage(challenger.Id, defender.Id));
        }
    }
}