
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        [WorldHandler(GameActionAcknowledgementMessage.Id)]
        public static void HandleGameActionAcknowledgementMessage(WorldClient client,
                                                                  GameActionAcknowledgementMessage message)
        {
            // valid == true anyway
            if (message.valid && client.ActiveCharacter.IsFighting())
            {
                client.ActiveCharacter.Fighter.EndSequenceAction();
            }
        }
    }
}