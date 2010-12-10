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
using Stump.BaseCore.Framework.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public sealed class QueueDispatcher
    {
        #region Properties

        private readonly BlockingQueue<Tuple<BaseClient, Message>> m_commonQueue =
            new BlockingQueue<Tuple<BaseClient, Message>>();

        private readonly BlockingQueue<Tuple<BaseClient, Message>> m_fastQueue =
            new BlockingQueue<Tuple<BaseClient, Message>>();

        private bool m_benchmark;
        private uint m_commonMessageTreatedCount;

        private uint m_fastMessageTreatedCount;
        private List<Type> m_priorithizedMessage = new List<Type>();

        public uint FastMessageTreatedCount
        {
            get { return m_fastMessageTreatedCount; }
        }

        public uint CommonMessageTreatedCount
        {
            get { return m_commonMessageTreatedCount; }
        }

        public bool ActiveBenchmark
        {
            get { return m_benchmark; }
            set { m_benchmark = value; }
        }

        public List<Type> PriorithizedMessage
        {
            get { return m_priorithizedMessage; }
            set { m_priorithizedMessage = value; }
        }

        #endregion

        #region Constructor

        public QueueDispatcher(bool benchmark = true)
        {
            m_benchmark = benchmark;
        }

        #endregion

        #region Queuing Management

        internal void Enqueue(Tuple<BaseClient, Message> tuple)
        {
            if (isPriorithized(tuple.Item2))
                m_fastQueue.Enqueue(tuple);
            else
                m_commonQueue.Enqueue(tuple);
        }

        internal void Enqueue(BaseClient client, Message message)
        {
            var tuple = new Tuple<BaseClient, Message>(client, message);

            if (isPriorithized(tuple.Item2))
                m_fastQueue.Enqueue(tuple);
            else
                m_commonQueue.Enqueue(tuple);
        }

        internal Tuple<BaseClient, Message> Dequeue()
        {
            //Problème si FastQueue vide, ça va attendre sur la CommonQueue, puis si un FastQueueMessage arrive, donc propriété Waiting en attendant
            //Y'a quelque chose, on y go ou Y'a rien, mais la Common est déja en attente donc on y go aussi
            if (m_fastQueue.Count > 0 || (m_fastQueue.Count == 0 && m_commonQueue.IsWaiting))
            {
                if (m_benchmark)
                    m_fastMessageTreatedCount++;
                return m_fastQueue.Dequeue();
            }
            if (m_benchmark)
                m_commonMessageTreatedCount++;
            return m_commonQueue.Dequeue();
        }

        private bool isPriorithized(Message message)
        {
            if (m_priorithizedMessage.Contains(typeof (Message)))
                return true;
            else
                return false;
        }

        #endregion

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("****QueueDispatcher*****");

            result.AppendLine("Fast Queue { IsWaiting : " + m_fastQueue.IsWaiting + " }");

            result.AppendLine("Fast Queue { Count : " + m_fastQueue.Count + " }");

            result.AppendLine("Fast Queue { Treated : " + m_fastMessageTreatedCount + " }");

            result.AppendLine("Common Queue { IsWaiting : " + m_commonQueue.IsWaiting + " }");

            result.AppendLine("Common Queue { Count : " + m_commonQueue.Count + " }");

            result.AppendLine("Common Queue { Treated : " + m_commonMessageTreatedCount + " }");

            result.AppendLine("Priorithized Message Count : " + m_priorithizedMessage.Count);

            foreach (Type type in m_priorithizedMessage)
                result.AppendLine(type.ToString());

            return result.ToString();
        }
    }
}