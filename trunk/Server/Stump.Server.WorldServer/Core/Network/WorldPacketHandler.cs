using Stump.Core.Threading;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Handlers;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Server.WorldServer.Core.Network
{
    public class WorldPacketHandler : HandlerManager<WorldPacketHandler, WorldHandlerAttribute, WorldHandlerContainer, WorldClient>
    {
        public override void Dispatch(WorldClient client, Message message)
        {
            if (message is BasicPingMessage) // pong immediately
            {
                client.Send(new BasicPongMessage(( message as BasicPingMessage ).quiet));
                return;
            }

            MessageHandler handler;
            if (m_handlers.TryGetValue(message.MessageId, out handler))
            {
                if (!handler.Container.CanHandleMessage(client, message.MessageId))
                {
                    m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                    return;
                }

                IContextHandler context = GetContextHandler(handler.Attribute, client, message);
                if (context != null)
                {
                    context.AddMessage(new HandledMessage<WorldClient>(handler.Action, client, message));
                }
            }
            else
            {
                m_logger.Debug("Received Unknown packet : " + message);
            }
        }


        public IContextHandler GetContextHandler(WorldHandlerAttribute attr, WorldClient client, Message message)
        {
            if (client.Character == null && attr.RequiresLogin)
            {
                m_logger.Warn("Handler id = {0} cannot handle this message because the client {1} is not logged and should be", message, client);
                client.Disconnect();
                return null;
            }

            if (client.Character != null && !attr.RequiresLogin)
            {
                m_logger.Warn("Handler id = {0} cannot handle this message because the client {1} is already logged and should be not", message, client);
                client.Disconnect();
                return null;
            }

            if (!attr.IsGamePacket)
            {
                return WorldServer.Instance.IOTaskPool;
            }

            if (client.Character == null || client.Account == null)
            {
                m_logger.Warn("Client {0} sent {1} before being logged", client, message);
                client.Disconnect();
                return null;
            }

            if (client.Character.Area == null)
            {
                m_logger.Warn("Client {0} sent {1} while not in world", client, message);
                client.Disconnect();
                return null;
            }

            return client.Character.Area;
        }
    }
}