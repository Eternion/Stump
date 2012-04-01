
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Handlers.Actions
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        [WorldHandler(GameActionAcknowledgementMessage.Id)]
        public static void HandleGameActionAcknowledgementMessage(WorldClient client,
                                                                  GameActionAcknowledgementMessage message)
        {
            // valid == true anyway
            if (message.valid && client.Character.IsFighting() && client.Character.Fighter.IsFighterTurn())
            {
                client.Character.Fighter.Fight.AcknowledgeAction();
            }
        }
    }
}