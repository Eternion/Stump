using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Handlers.Interactives
{
    public class InteractiveHandler : WorldHandlerContainer
    {
        [WorldHandler(InteractiveUseRequestMessage.Id)]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            client.ActiveCharacter.Map.UseInteractiveObject(client.ActiveCharacter, message.elemId, message.skillInstanceUid);
        }

        [WorldHandler(InteractiveUseEndedMessage.Id)]
        public static void HandleInteractiveUseEndedMessage(WorldClient client, InteractiveUseEndedMessage message)
        {

        }

        public static void SendInteractiveUsedMessage(IPacketReceiver client, ContextActor user, InteractiveObject interactiveObject, Skill skill)
        {
            client.Send(new InteractiveUsedMessage(user.Id, interactiveObject.Id, (short) skill.Id, (short) skill.Template.Duration));
        }
    }
}