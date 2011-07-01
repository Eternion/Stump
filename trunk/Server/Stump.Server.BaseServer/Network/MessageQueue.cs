using System;
using System.Text;
using Stump.Core.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public sealed class MessageQueue
    {
        #region Properties

        private readonly BlockingQueue<Tuple<BaseClient, Message>> m_commonQueue = new BlockingQueue<Tuple<BaseClient, Message>>(200);

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

        public void Enqueue(Tuple<BaseClient, Message> tuple)
        {
            m_commonQueue.Enqueue(tuple);
        }

        public void Enqueue(BaseClient client, Message message)
        {
            var tuple = new Tuple<BaseClient, Message>(client, message);

            m_commonQueue.Enqueue(tuple);
        }

        internal Tuple<BaseClient, Message> Dequeue()
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