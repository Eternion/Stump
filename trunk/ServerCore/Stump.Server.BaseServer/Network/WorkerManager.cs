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
using System.Reflection;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Handler;

namespace Stump.Server.BaseServer.Network
{
    public sealed class WorkerManager
    {
        /// <summary>
        ///   Define worker numbers by processor count.
        /// </summary>
        [Variable]
        public static bool AutoWorkerNumber = true;

        /// <summary>
        ///   Number of workers if AutoWorkerNumber is false
        /// </summary>
        [Variable]
        public static int WorkerThreadNumber = 2;

        private readonly HandlerManager m_handlerManager;
        private readonly QueueDispatcher m_queueDispatcher;

        private readonly object m_syncRoot = new object();
        private readonly List<Worker> m_workerList = new List<Worker>();


        /// <summary>
        ///   New instance of WorkerManager
        /// </summary>
        /// <param name = "workerNbr"></param>
        public WorkerManager(QueueDispatcher queueDispatcher, HandlerManager handlerManager)
        {
            m_queueDispatcher = queueDispatcher;
            m_handlerManager = handlerManager;

            if (AutoWorkerNumber)
                AdaptWorkerNumberWithProcessor();
            else
                AddWorker(WorkerThreadNumber);
        }

        /// <summary>
        ///   List of Active Worker
        /// </summary>
        public IEnumerable<Worker> WorkerList
        {
            get { return m_workerList; }
        }

        /// <summary>
        ///   Add a Worker
        /// </summary>
        public void AddWorker(int nbr)
        {
            for (int i = 0; i < nbr; i++)
                m_workerList.Add(new Worker(m_queueDispatcher, m_handlerManager));
        }

        /// <summary>
        ///   Remove a Worker
        /// </summary>
        public void RemoveWorker(int nbr)
        {
            for (int i = 0; i < nbr; i++)
            {
                m_workerList[m_workerList.Count].WantToStop = true;
                m_workerList.RemoveAt(m_workerList.Count);
            }
        }

        /// <summary>
        ///   Define Worker number
        /// </summary>
        /// <param name = "number">number of Worker to set</param>
        public void SetWorkerNumber(int number)
        {
            if (m_workerList.Count == number)
                return;
            if (m_workerList.Count > number)
                RemoveWorker(m_workerList.Count - number);
            else
                AddWorker(number - m_workerList.Count);
        }

        /// <summary>
        ///   AutoAdapt Worker number with number of processor cores
        /// </summary>
        public void AdaptWorkerNumberWithProcessor()
        {
            if (Environment.ProcessorCount >= 4)
                SetWorkerNumber(Environment.ProcessorCount - 2);
            else
                SetWorkerNumber(2);
        }

        /// <summary>
        ///   Stop Sleeped worker and return number of stopped Workers
        /// </summary>
        /// <returns>number of stopped Worker</returns>
        public int RemoveSleepedWorker()
        {
            int count = 0;
            foreach (Worker worker in m_workerList)
            {
                if (worker.State == ThreadState.Suspended)
                {
                    worker.WantToStop = true;
                    m_workerList.Remove(worker);
                    count++;
                }
            }
            return count;
        }


        public string GetDetailedMessageTypes(bool orderByTime)
        {
            var result = new StringBuilder();

            result.AppendLine("");

            var treatedMessage = m_workerList.SelectMany(w => w.TreatedMessage);

            var groupMessage = from g in treatedMessage
                               group g by g.Item1.getMessageId()
                                   into gr
                                   select gr;

            if (orderByTime)
                groupMessage = groupMessage.OrderByDescending(g => g.Average(t => t.Item2));

            foreach (var message in groupMessage)
            {
                double average = message.Average(t => t.Item2);
                var ecartType = Math.Sqrt((message.Sum(t => Math.Pow(average - t.Item2, 2))))/message.Count();
                result.AppendLine("");
                result.AppendLine(string.Format("\tMessage :{0}", message.First().Item1.GetType().Name));
                result.AppendLine(string.Format("\tAverage Time : {0}ms", average));
                result.AppendLine(string.Format("\tMin Time : {0}ms", message.Min(t => t.Item2)));
                result.AppendLine(string.Format("\tMax Time : {0}ms", message.Max(t => t.Item2)));
                result.AppendLine(string.Format("\tEcart Type : {0} ({1}%)", ecartType, Math.Ceiling(ecartType*100)));
                result.AppendLine(string.Format("\tMessage count : {0}", message.Count()));
            }

            return result.ToString();
        }

        public string GetDetailedMessages(string typeName, bool orderByTime)
        {
            var result = new StringBuilder();

            result.AppendLine("");

            var treatedMessage = m_workerList.SelectMany(w => w.TreatedMessage).Where(t => t.Item1.GetType().Name == typeName);

            if (orderByTime)
                treatedMessage = treatedMessage.OrderByDescending(t => t.Item2);

            foreach (var tuple in treatedMessage)
            {
                result.AppendLine(string.Format("\tId :{0} | Time : {1}ms", Math.Abs(tuple.GetHashCode()), tuple.Item2));
            }

            return result.ToString();
        }

        public string GetDetailedMessage(string typeName, int id)
        {
            var result = new StringBuilder();

            result.AppendLine("");

            var treatedMessage = m_workerList.SelectMany(w => w.TreatedMessage).Where(t => t.Item1.GetType().Name == typeName).First(t => Math.Abs(t.GetHashCode()) == id).Item1;

            var fields = treatedMessage.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                result.AppendLine(string.Format("\t{0} : {1}", field.Name, field.GetValue(treatedMessage)));
            }

            return result.ToString();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("****WorkerManager*****");

            result.AppendLine("Worker Count : " + m_workerList.Count);

            foreach (Worker worker in m_workerList)
            {
                result.AppendLine("___________________");
                result.AppendLine(worker.ToString());
                result.AppendLine("___________________");
            }

            return result.ToString();
        }
    }
}