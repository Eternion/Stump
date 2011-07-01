
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Reflection;
using Stump.Core.Attributes;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Network
{
    public sealed class WorkerManager : Singleton<WorkerManager>
    {
        /// <summary>
        ///   Define worker numbers by processor count.
        /// </summary>
        [Variable]
        public static bool AutoWorkerNumber = true;

        /// <summary>
        ///   Number of workers only if AutoWorkerNumber is false
        /// </summary>
        [Variable]
        public static int WorkerThreadNumber = 2;

        private readonly object m_syncRoot = new object();
        private readonly List<Worker> m_workerList = new List<Worker>();


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerManager"/> class.
        /// </summary>
        private WorkerManager()
        {
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
            lock (m_syncRoot)
            {
                for (int i = 0; i < nbr; i++)
                    m_workerList.Add(new Worker());
            }
        }

        /// <summary>
        ///   Remove a Worker
        /// </summary>
        public void RemoveWorker(int nbr)
        {
            lock (m_syncRoot)
            {
                for (int i = 0; i < nbr; i++)
                {
                    m_workerList[m_workerList.Count].WantToStop = true;
                    m_workerList.RemoveAt(m_workerList.Count - 1);
                }
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
            // note : we create more worker than core count 
            // because a worker can fall in sleep status cause of IO methods
            // these methods have to be executed in the IO thread

            if (Environment.ProcessorCount >= 4)
                SetWorkerNumber(2 * Environment.ProcessorCount);
            else
                SetWorkerNumber(4);
        }

        /// <summary>
        ///   Stop Sleeped worker and return number of stopped Workers
        /// </summary>
        /// <returns>number of stopped Worker</returns>
        public int RemoveSleepedWorker()
        {
            lock (m_syncRoot)
            {
                return m_workerList.RemoveAll(entry => entry.State == ThreadState.Suspended);
            }
        }


        public string GetDetailedMessageTypes(bool orderByTime)
        {
            var result = new StringBuilder();

            result.AppendLine("");

            var treatedMessage = m_workerList.SelectMany(w => w.TreatedMessage);

            var groupMessage = from g in treatedMessage
                               group g by g.Item1.MessageId
                                   into gr
                                   select gr;

            if (orderByTime)
                groupMessage = groupMessage.OrderByDescending(g => g.Average(t => t.Item2));

            foreach (var message in groupMessage)
            {
                double average = message.Average(t => t.Item2);
                var ecartType = Math.Sqrt((message.Sum(t => Math.Pow(average - t.Item2, 2)))) / message.Count();
                result.AppendLine("");
                result.AppendLine(string.Format("\tMessage :{0}", message.First().Item1.GetType().Name));
                result.AppendLine(string.Format("\tAverage Time : {0}ms", average));
                result.AppendLine(string.Format("\tMin Time : {0}ms", message.Min(t => t.Item2)));
                result.AppendLine(string.Format("\tMax Time : {0}ms", message.Max(t => t.Item2)));
                result.AppendLine(string.Format("\tEcart Type : {0} ({1}%)", ecartType, Math.Ceiling(ecartType * 100)));
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