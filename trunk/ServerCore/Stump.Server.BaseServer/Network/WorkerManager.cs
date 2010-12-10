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
using System.Threading;
using Stump.BaseCore.Framework.Attributes;
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
        ///   Number of workers if <see cref = "AutoWorkerNumber" /> is false
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