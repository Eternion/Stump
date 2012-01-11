using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Initialization
{
    public class InitializationHandler : WorldHandlerContainer
    {
        public static void SendOnConnectionEventMessage(IPacketReceiver client, sbyte eventType)
        {
            client.Send(new OnConnectionEventMessage(eventType));
        }

        public static void SendSetCharacterRestrictionsMessage(IPacketReceiver client)
        {
            // todo
            client.Send(new SetCharacterRestrictionsMessage(
                            new ActorRestrictionsInformations(
                                false, // cantBeAgressed
                                false, // cantBeChallenged
                                false, // cantTrade
                                false, // cantBeAttackedByMutant
                                false, // cantRun
                                false, // forceSlowWalk
                                false, // cantMinimize
                                false, // cantMove

                                true, // cantAggress
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