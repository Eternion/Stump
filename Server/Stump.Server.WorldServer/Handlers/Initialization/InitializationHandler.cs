using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Initialization
{
    public class InitializationHandler : WorldHandlerContainer
    {
        public static void SendOnConnectionEventMessage(IPacketReceiver client, sbyte eventType)
        {
            client.Send(new OnConnectionEventMessage(eventType));
        }

        public static void SendSetCharacterRestrictionsMessage(IPacketReceiver client, Character character)
        {
            // todo
            client.Send(new SetCharacterRestrictionsMessage(character.Id,
                            new ActorRestrictionsInformations(
                                !character.Map.AllowAggression, // cantBeAgressed
                                !character.Map.AllowChallenge, // cantBeChallenged
                                !character.Map.AllowExchangesBetweenPlayers, // cantTrade
                                false, // cantBeAttackedByMutant
                                false, // cantRun
                                false, // cantMinimize
                                false, // cantMove

                                !character.Map.AllowAggression, // cantAggress
                                false, // cantChallenge
                                false, // cantExchange
                                false, // cantAttack
                                false, // cantChat
                                true, // cantBeMerchant
                                true, // cantUseObject
                                true, // cantUseTaxCollector

                                false, // cantUseInteractive
                                false, // cantSpeakToNPC
                                false, // cantChangeZone
                                false, // cantAttackMonster
                                false // cantWalk8Directions
                                )));
        }
    }
}