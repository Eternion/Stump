using Stump.Server.BaseServer.Handler;

namespace Stump.Server.WorldServer.Handlers
{
    public class WorldHandlerAttribute : HandlerAttribute
    {
        public WorldHandlerAttribute(uint messageId)
            : base(messageId)
        {
        }

        public WorldHandlerAttribute(uint messageId, bool handledByIOTask)
            : base(messageId, handledByIOTask)
        {
        }
    }
}