using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using NLog;

namespace Stump.Server.WorldServer.Core.Network
{
    public class WorldClientCollection : IPacketReceiver, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private object m_lock = new object();
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
                lock (m_lock)
                {
                    if (m_underlyingList.Count == 0)
                        return;

                    var disconnectedClients = new List<WorldClient>();
                    SegmentStream stream = BufferManager.Default.CheckOutStream();
                    try
                    {
                        var writer = new BigEndianWriter(stream);
                        message.Pack(writer);
                        stream.Segment.Uses = m_underlyingList.Count(x => x != null && x.Connected);
                        
                        foreach (WorldClient worldClient in m_underlyingList)
                        {
                            if (worldClient != null)
                            {
                                worldClient.Send(stream);
                                worldClient.OnMessageSent(message);
                            }

                            if (worldClient == null || !worldClient.Connected)
                            {
                                disconnectedClients.Add(worldClient);
                            }
                        }
                    }
                    finally
                    {
                        
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
            lock (m_lock)
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
            lock (m_lock)
            {
                if (m_singleClient == client)
                    m_singleClient = null;
                else
                    m_underlyingList.Remove(client);
            }
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