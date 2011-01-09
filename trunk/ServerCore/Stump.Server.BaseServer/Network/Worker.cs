// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Handler;

namespace Stump.Server.BaseServer.Network
{
    public sealed class Worker
    {
        #region Properties

        private readonly HandlerManager m_handlerManager;
        private readonly QueueDispatcher m_queueDispatcher;
        private readonly List<Tuple<Message, double>> m_treatedMessage = new List<Tuple<Message, double>>();
        private bool m_benchmark;
        private bool m_paused;
        private DateTime m_startDate;
        private Thread m_thread;
        private bool m_wantToStop;

        public QueueDispatcher QueueDispatcher
        {
            get { return m_queueDispatcher; }
        }

        public HandlerManager HandlerManager
        {
            get { return m_handlerManager; }
        }

        public bool ActiveBenchmark
        {
            get { return m_benchmark; }
            set { m_benchmark = value; }
        }

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
            get { return TreatedMessage.Count; }
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

        public List<Tuple<Message, double>> TreatedMessage
        {
            get { return m_treatedMessage; }
        }

        #endregion

        public Worker(QueueDispatcher queueDispatcher, HandlerManager handlerManager, bool benchmark = true)
        {
            m_benchmark = benchmark;
            m_queueDispatcher = queueDispatcher;
            m_handlerManager = handlerManager;
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
                    Tuple<BaseClient, Message> tuple = m_queueDispatcher.Dequeue();

                    if (m_benchmark)
                    {
                        DateTime start = DateTime.Now;

                        m_handlerManager.Dispatch(tuple.Item1, tuple.Item2);

                        TreatedMessage.Add(new Tuple<Message, double>(tuple.Item2, DateTime.Now.Subtract(start).TotalMilliseconds));
                    }
                    else
                    {
                        m_handlerManager.Dispatch(tuple.Item1, tuple.Item2);
                    }
                }
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
            result.AppendLine("Treated Message : " + TreatedMessage.Count);
            if (TreatedMessage.Count != 0)
            {
                result.AppendLine("Average treatment : " + TreatedMessageAverageTime + " ms");
                foreach (Tuple<Message, double> message in TreatedMessage)
                    result.AppendLine(message.Item1.GetType().Name + " => " + message.Item2);
            }
            return result.ToString();
        }
    }
}