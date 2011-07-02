using System;
using System.Text;
using Stump.Core.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public sealed class MessageQueue
    {
        #region Properties

        private readonly BlockingQueue<ClientMessage> m_commonQueue = new BlockingQueue<ClientMessage>(200);

        private bool m_benchmark;
        private uint m_commonMessageTreatedCount;

        public uint CommonMessageTreatedCount
        {
            get { return m_commonMessageTreatedCount; }
        }

        public bool ActiveBenchmark
        {
            get { return m_benchmark; }
            set { m_benchmark = value; }
        }

        #endregion

        #region Constructor

        public MessageQueue(bool benchmark = true)
        {
            m_benchmark = benchmark;
        }

        #endregion

        #region Queuing Management

        public void Enqueue(ClientMessage mess)
        {
            m_commonQueue.Enqueue(mess);
        }

        public void Enqueue(BaseClient client, Message message)
        {
            var mess = new ClientMessage(client, message);

            m_commonQueue.Enqueue(mess);
        }

        internal ClientMessage Dequeue()
        {
            if (m_benchmark)
                m_commonMessageTreatedCount++;

            return m_commonQueue.Dequeue();
        }

        #endregion

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("****QueueDispatcher*****");

            result.AppendLine("Common Queue { IsWaiting : " + m_commonQueue.IsWaiting + " }");
            result.AppendLine("Common Queue { Count : " + m_commonQueue.Count + " }");
            result.AppendLine("Common Queue { Treated : " + m_commonMessageTreatedCount + " }");

            return result.ToString();
        }
    }
}