
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Handler;
using ThreadState = System.Threading.ThreadState;

namespace Stump.Server.BaseServer.Network
{
    public sealed class Worker
    {
        #region Properties

        private readonly List<Tuple<Message, long>> m_treatedMessage = new List<Tuple<Message, long>>(1000);


        private bool m_paused;
        private DateTime m_startDate;
        private Thread m_thread;
        private bool m_wantToStop;
        private Stopwatch m_stopWatch = new Stopwatch();

        public bool WantToStop
        {
            get { return m_wantToStop; }
            set { m_wantToStop = value; }
        }

        public bool Paused
        {
            get { return m_paused; }
            set { m_paused = value; }
        }

        public TimeSpan UpTime
        {
            get { return DateTime.Now.Subtract(m_startDate); }
        }

        public DateTime StartDate
        {
            get { return m_startDate; }
        }

        public int TreatedMessageCount
        {
            get { return m_treatedMessage.Count; }
        }

        public double TreatedMessageAverageTime
        {
            get
            {
                return TreatedMessage.Average(tuple => tuple.Item2);
            }
        }

        public int UniqueId
        {
            get { return m_thread.ManagedThreadId; }
        }

        public ThreadState State
        {
            get { return m_thread.ThreadState; }
        }

        public IEnumerable<Tuple<Message, long>> TreatedMessage
        {
            get { return m_treatedMessage; }
        }

        #endregion

        public Worker()
        {
            Task.Factory.StartNew(Loop, TaskCreationOptions.LongRunning);
        }

        private void Loop()
        {
            m_thread = Thread.CurrentThread;
            m_startDate = DateTime.Now;
            m_thread.Name = "Worker #" + UniqueId;

            while (!m_wantToStop)
            {
                if (!m_paused)
                {
                    ClientMessage mess = ClientManager.Instance.MessageQueue.Dequeue();

                    if (Settings.EnableBenchmarking)
                    {
                        m_stopWatch.Restart();

                        HandlerManager.Instance.Dispatch(mess.Client, mess.Message);

                        m_stopWatch.Stop();

                        m_treatedMessage.Add(new Tuple<Message, long>(mess.Message,m_stopWatch.ElapsedMilliseconds));
                    }
                    else
                    {
                        HandlerManager.Instance.Dispatch(mess.Client, mess.Message);
                    }
                }
                else
                    Thread.Sleep(1);
            }
        }


        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("*******Worker********");

            result.AppendLine("Worker n°" + m_thread.ManagedThreadId);
            result.AppendLine("State : " + m_thread.ThreadState);
            result.AppendLine("Start Date : " + m_startDate.ToLongTimeString());
            result.AppendLine("UpTime : " + UpTime);
            result.AppendLine("Treated Message : " + m_treatedMessage.Count);
            if (m_treatedMessage.Count != 0)
            {
                result.AppendLine("Average treatment : " + TreatedMessageAverageTime + " ms");

                foreach (var message in m_treatedMessage)
                    result.AppendLine(message.Item1.GetType().Name + " => " + message.Item2);
            }
            return result.ToString();
        }
    }
}