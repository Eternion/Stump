using Stump.Server.BaseServer.Handler;

namespace Stump.Server.WorldServer.Handlers
{
    public class WorldHandlerAttribute : HandlerAttribute
    {
        public WorldHandlerAttribute(uint messageId)
            : base(messageId)
        {
            IsGamePacket = true;
            ShouldBeLogged = true;
        }

        public WorldHandlerAttribute(uint messageId, bool isGamePacket, bool requiresLogin)
            : base(messageId)
        {
            IsGamePacket = isGamePacket;
            ShouldBeLogged = requiresLogin;
        }

        public bool ShouldBeLogged
        {
            get;
            set;
        }

        public bool IsGamePacket
        {
            get;
            set;
        }

        public bool IgnorePredicate
        {
            get;
            set;
        }
    }
}