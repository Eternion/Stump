using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Core.Network
{
    public class WorldClientCollection : IPacketReceiver, IEnumerable<WorldClient>, IDisposable
    {
        private WorldClient m_singleClient; // avoid new object allocation
        private readonly List<WorldClient> m_underlyingList = new List<WorldClient>();

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
                lock (this)
                {
                    if (m_underlyingList.Count == 0)
                        return;

                    var disconnectedClients = new List<WorldClient>();
                    SegmentStream stream = BufferManager.Default.CheckOutStream();
                    try
                    {
                        var writer = new BigEndianWriter(stream);
                        message.Pack(writer);
                        stream.Segment.Uses = m_underlyingList.Count;

                        if (stream.Segment.Uses == 0)
                            BufferManager.Default.CheckIn(stream.Segment);

                        foreach (WorldClient worldClient in m_underlyingList)
                        {
                            if (worldClient != null)
                            {
                                worldClient.Send(stream);
                                worldClient.OnMessageSent(message);
                            }

                            if (worldClient == null || !worldClient.Connected)
                                disconnectedClients.Add(worldClient);
                        }
                    }
                    finally
                    {
                        if (stream.Segment.Uses > 0)
                        {
                            stream.Segment.Uses = 0;
                            BufferManager.Default.CheckIn(stream.Segment);
                        }
                    }

                    foreach (var client in disconnectedClients)
                    {
                        Remove(client);
                    }
                }
            }
        }

        public void Add(WorldClient client)
        {
            lock (this)
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
        }

        public void Remove(WorldClient client)
        {
            lock (this)
            {
                if (m_singleClient == client)
                    m_singleClient = null;
                else
                    m_underlyingList.Remove(client);
            }
        }

        public IEnumerator<WorldClient> GetEnumerator()
        {
            // not thread safe
            return m_singleClient != null ? new[] { m_singleClient }.AsEnumerable().GetEnumerator() : m_underlyingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator WorldClientCollection(WorldClient client)
        {
            return new WorldClientCollection(client);
        }

        public void Dispose()
        {
            m_singleClient = null;
            m_underlyingList.Clear();
        }
    }
}