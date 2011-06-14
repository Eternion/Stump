
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class ScriptHandler : WorldHandlerContainer
    {
        public static void SendCinematicMessage(WorldClient client, uint cinematicId)
        {
            client.Send(new CinematicMessage(cinematicId));
        }
    }
}