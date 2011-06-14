
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class ModerationHandler : WorldHandlerContainer
    {
        public static void SendPopupWarningMessage(WorldClient client, uint duration, string author, string content)
        {
            client.Send(new PopupWarningMessage(duration, author, content));
        }
    }
}