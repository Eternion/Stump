using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Handlers.Moderation
{
    public class ModerationHandler : WorldHandlerContainer
    {
        public static void SendPopupWarningMessage(IPacketReceiver client, string content, string author, byte lockDuration)
        {
            client.Send(new PopupWarningMessage(lockDuration, author, content));
        }
    }
}