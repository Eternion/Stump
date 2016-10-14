using System;
using System.Collections.Generic;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Message = Stump.DofusProtocol.Messages.Message;

namespace FakeClient.Handlers
{
    public class FakeClientPacketHandler : HandlerManager<FakeClientPacketHandler, FakeHandlerAttribute, FakeClientHandlerContainer, FakeClient>
    {
        private SelfRunningTaskPool m_taskPool;

        public FakeClientPacketHandler()
        {
            m_taskPool = new SelfRunningTaskPool(100, "Fake client task pool");
            m_taskPool.Start();
        }

        public override void Dispatch(FakeClient client, Message message)
        {
            List<MessageHandler> handlers;
            if (m_handlers.TryGetValue(message.MessageId, out handlers))
            {
                try
                {
                    foreach (var handler in handlers)
                    {
                        if (!handler.Container.CanHandleMessage(client, message.MessageId))
                        {
                            m_logger.Warn(client + " tried to send " + message + " but predicate didn't success");
                            return;
                        }

                        m_taskPool.AddMessage(new BenchmarkingMessage(new HandledMessage<FakeClient>(handler.Action, client, message)));
                    }
                }
                catch (Exception ex)
                {
                    m_logger.Error(string.Format("[Handler : {0}] Force disconnection of client {1} : {2}", message, client, ex));
                    client.Disconnect();
                }
            }
        }
    }
}