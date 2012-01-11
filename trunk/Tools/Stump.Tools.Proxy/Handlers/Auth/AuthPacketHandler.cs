using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class AuthPacketHandler : HandlerManager<AuthPacketHandler, AuthHandlerAttribute, AuthHandlerContainer, AuthClient>
    {
        public override void Dispatch(AuthClient client, Message message)
        {
            MessageHandler handler;
            if (m_handlers.TryGetValue(message.MessageId, out handler))
            {
                if (!handler.Container.CanHandleMessage(client, message.MessageId))
                {
                    m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                    return;
                }

                Proxy.Instance.IOTaskPool.AddMessage(new HandledMessage<AuthClient>(handler.Action, client, message));
            }
        }
    }
}