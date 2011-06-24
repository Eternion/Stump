
using System;
using System.Collections.Generic;
using System.Text;
using Stump.Core.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public sealed class QueueDispatcher
    {
        #region Properties

        private readonly BlockingQueue<Tuple<BaseClient, Message>> m_commonQueue = new BlockingQueue<Tuple<BaseClient, Message>>(200);

  //      private readonly BlockingQueue<Tuple<BaseClient, Message>> m_fastQueue = new BlockingQueue<Tuple<BaseClient, Message>>();

        private bool m_benchmark;
        private uint m_commonMessageTreatedCount;

 //       private uint m_fastMessageTreatedCount;
   //     private List<Type> m_priorithizedMessage = new List<Type>();

        //public uint FastMessageTreatedCount
        //{
        //    get { return m_fastMessageTreatedCount; }
        //}

        public uint CommonMessageTreatedCount
        {
            get { return m_commonMessageTreatedCount; }
        }

        public bool ActiveBenchmark
        {
            get { return m_benchmark; }
            set { m_benchmark = value; }
        }

        //public List<Type> PriorithizedMessage
        //{
        //    get { return m_priorithizedMessage; }
        //    set { m_priorithizedMessage = value; }
        //}

        #endregion

        #region Constructor

        public QueueDispatcher(bool benchmark = true)
        {
            m_benchmark = benchmark;
        }

        #endregion

        #region Queuing Management

        public void Enqueue(Tuple<BaseClient, Message> tuple)
        {
      //      if (IsPriorithized(tuple.Item2))
       //         m_fastQueue.Enqueue(tuple);
       //     else
                m_commonQueue.Enqueue(tuple);
        }

        public void Enqueue(BaseClient client, Message message)
        {
            var tuple = new Tuple<BaseClient, Message>(client, message);

            //if (IsPriorithized(tuple.Item2))
            //    m_fastQueue.Enqueue(tuple);
            //else
                m_commonQueue.Enqueue(tuple);
        }

        internal Tuple<BaseClient, Message> Dequeue()
        {
            //if (m_fastQueue.Count > 0 || (m_fastQueue.Count == 0 && m_commonQueue.IsWaiting))
            //{
            //    if (m_benchmark)
            //        m_fastMessageTreatedCount++;

            //    return m_fastQueue.Dequeue();
            //}

            if (m_benchmark)
                m_commonMessageTreatedCount++;

            return m_commonQueue.Dequeue();
        }

        //private bool IsPriorithized(Message message)
        //{
        //    return m_priorithizedMessage.Contains(message.GetType());
        //}

        #endregion

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("****QueueDispatcher*****");

     //       result.AppendLine("Fast Queue { IsWaiting : " + m_fastQueue.IsWaiting + " }");

    //        result.AppendLine("Fast Queue { Count : " + m_fastQueue.Count + " }");

    //        result.AppendLine("Fast Queue { Treated : " + m_fastMessageTreatedCount + " }");

            result.AppendLine("Common Queue { IsWaiting : " + m_commonQueue.IsWaiting + " }");

            result.AppendLine("Common Queue { Count : " + m_commonQueue.Count + " }");

            result.AppendLine("Common Queue { Treated : " + m_commonMessageTreatedCount + " }");

    //        result.AppendLine("Priorithized Message Count : " + m_priorithizedMessage.Count);

     //       foreach (Type type in m_priorithizedMessage)
      //          result.AppendLine(type.ToString());

            return result.ToString();
        }
    }
}