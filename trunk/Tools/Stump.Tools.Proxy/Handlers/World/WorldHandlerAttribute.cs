
using System;
using Stump.Server.BaseServer.Handler;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class WorldHandlerAttribute : HandlerAttribute
    {
        public WorldHandlerAttribute(uint message)
            : base(message)
        {
        }
    }
}