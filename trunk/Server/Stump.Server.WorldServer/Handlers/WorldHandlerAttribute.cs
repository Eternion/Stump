using System;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Handler;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers
{
    public class WorldHandlerAttribute : HandlerAttribute
    {
        public WorldHandlerAttribute(uint messageId)
            : base(messageId)
        {
            IsGamePacket = true;
            RequiresLogin = true;
        }

        public WorldHandlerAttribute(uint messageId, bool isGamePacket, bool requiresLogin)
            : base(messageId)
        {
            IsGamePacket = isGamePacket;
            RequiresLogin = requiresLogin;
        }

        public bool RequiresLogin
        {
            get;
            set;
        }

        public bool IsGamePacket
        {
            get;
            set;
        }
    }
}