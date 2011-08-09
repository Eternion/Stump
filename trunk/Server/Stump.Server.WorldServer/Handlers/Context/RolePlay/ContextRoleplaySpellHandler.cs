using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        public static void SendSpellForgottenMessage(WorldClient client)
        {
            client.Send(new SpellForgottenMessage(new List<short>(), 0));
        }
    }
}