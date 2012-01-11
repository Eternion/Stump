using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler : WorldHandlerContainer
    {
        public static void SendSpellForgottenMessage(IPacketReceiver client)
        {
            client.Send(new SpellForgottenMessage(new List<short>(), 0));
        }
    }
}