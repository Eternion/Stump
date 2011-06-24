
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        public static void SendSpellForgottenMessage(WorldClient client)
        {
            client.Send(new SpellForgottenMessage(new List<uint>(), 0));
        }
    }
}