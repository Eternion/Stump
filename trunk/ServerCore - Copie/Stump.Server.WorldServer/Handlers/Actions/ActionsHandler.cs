
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (GameActionAcknowledgementMessage))]
        public static void HandleGameActionAcknowledgementMessage(WorldClient client,
                                                                  GameActionAcknowledgementMessage message)
        {
            // valid == true anyway
            if (message.valid && client.ActiveCharacter.IsInFight)
            {
                client.ActiveCharacter.Fighter.SequenceEndReply(message.actionId);
            }
        }
    }
}