using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors;
using Stump.Server.WorldServer.Worlds.Interactives;
using Stump.Server.WorldServer.Worlds.Interactives.Skills;

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

        public static void SendInteractiveUsedMessage(WorldClient client, ContextActor user, InteractiveObject interactiveObject, Skill skill)
        {
            client.Send(new InteractiveUsedMessage(user.Id, interactiveObject.Id, (short) skill.Id, (short) skill.Template.Duration));
        }
    }
}