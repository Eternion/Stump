using Stump.Core.Threading;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.Network;
using Message = Stump.DofusProtocol.Messages.Message;
using WorldClient = Stump.Tools.Proxy.Network.WorldClient;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class WorldPacketHandler : HandlerManager<WorldPacketHandler, WorldHandlerAttribute, WorldHandlerContainer, WorldClient>
    {
        public override void Dispatch(WorldClient client, Message message)
        {
            MessageHandler handler;
            if (m_handlers.TryGetValue(message.MessageId, out handler))
            {
                    if (!handler.Container.CanHandleMessage(client, message.MessageId))
                    {
                        m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                        return;
                    }

                    var context = Proxy.Instance.IOTaskPool;
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
    }
}