
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class InitializationHandler : WorldHandlerContainer
    {
        public static void SendOnConnectionEventMessage(WorldClient client, uint eventType)
        {
            client.Send(new OnConnectionEventMessage(eventType));
        }

        public static void SendSetCharacterRestrictionsMessage(WorldClient client)
        {
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