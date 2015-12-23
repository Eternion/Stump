using Stump.DofusProtocol.Messages;
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
            client.Send(new SetCharacterRestrictionsMessage(character.Id, character.GetActorRestrictionsInformations()));
        }
    }
}