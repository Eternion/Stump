using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Core.Network
{
    public class WorldClientCollection : IPacketReceiver, IEnumerable<WorldClient>
    {
        private WorldClient m_singleClient; // avoid new object allocation
        private List<WorldClient> m_underlyingList = new List<WorldClient>();

        public WorldClientCollection()
        {
            
        }

        public WorldClientCollection(IEnumerable<WorldClient> clients)
        {
            m_underlyingList = clients.ToList();
        }

        public WorldClientCollection(WorldClient client)
        {
            m_singleClient = client;
        }

        public int Count
        {
            get { return m_singleClient != null ? 1 : m_underlyingList.Count; }
        }

        public void Send(Message message)
        {
            if (m_singleClient != null)
            {
                m_singleClient.Send(message);
            }
            else
            {
                foreach (var worldClient in m_underlyingList)
                {
                    if (worldClient != null)
                        worldClient.Send(message);
                } 
            }
        }

        public void Add(WorldClient client)
        {
            if (m_singleClient != null)
            {
                m_underlyingList.Add(m_singleClient);
                m_underlyingList.Add(client);
                m_singleClient = null;
            }
            else
            {
                m_underlyingList.Add(client);
            }
        }

        public void Remove(WorldClient client)
        {
            if (m_singleClient == client)
                m_singleClient = null;
            else
                m_underlyingList.Remove(client);
        }

        public IEnumerator<WorldClient> GetEnumerator()
        {
            if (m_singleClient != null)
                return new[] { m_singleClient }.AsEnumerable().GetEnumerator();

            // not thread safe
            return m_underlyingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator WorldClientCollection(WorldClient client)
        {
            return new WorldClientCollection(client);
        }
    }
}