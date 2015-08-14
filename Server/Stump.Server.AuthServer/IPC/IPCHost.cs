#region License GNU GPL
// IPCHost.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCHost
    {
        /// <summary>
        /// Number of maximum managed servers
        /// </summary>
        [Variable]
        public static readonly int ServersMaxCount = 10;

        [Variable]
        public static int BufferSize = 8192;

        #region Events
        public event Action<IPCHost, IPCClient> ClientConnected;

        private void NotifyClientConnected(IPCClient client)
        {
            var handler = ClientConnected;
            if (handler != null) handler(this, client);
        }

        public event Action<IPCHost, IPCClient> ClientDisconnected;

        private void NotifyClientDisconnected(IPCClient client)
        {
            var handler = ClientDisconnected;
            if (handler != null) handler(this, client);
        }
        #endregion

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string m_host;
        private readonly int m_port;

        private readonly Socket m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);

        private SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs(); // async arg used on client connection
        private readonly SemaphoreSlim m_semaphore; // limit the number of threads accessing to a ressource
        private readonly List<IPCClient> m_clients = new List<IPCClient>();

        private bool m_paused;

        public IPCHost(string host, int port)
        {
            m_host = host;
            m_port = port;
            m_semaphore = new SemaphoreSlim(ServersMaxCount, ServersMaxCount);
        }

        public bool IsPaused
        {
            get { return m_paused; }
        }

        public bool Started
        {
            get;
            private set;
        }

        public string Host
        {
            get { return m_host; }
        }

        public int Port
        {
            get { return m_port; }
        }


        public Socket Socket
        {
            get { return m_listenSocket; }
        }

        public void Start()
        {
            if (IsPaused)
            {
                m_paused = false;

                StartAccept();
            }
            else
            {
                if (Started)
                    throw new Exception("IPCHost already started");

                var ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(Host).First(ip => ip.AddressFamily == AddressFamily.InterNetwork), Port);
                m_listenSocket.Bind(ipEndPoint);
                m_listenSocket.Listen(1);

                Started = true;

                StartAccept();
            }
        }


        public void Stop()
        {
            if (!Started)
                throw new Exception("IPCHost not started yet");

            m_paused = true;
        }


        private void StartAccept()
        {
            m_acceptArgs = new SocketAsyncEventArgs {AcceptSocket = null};
            m_acceptArgs.Completed += (sender, e) => ProcessAccept(e);

            if (m_semaphore.CurrentCount == 0)
            {
                logger.Warn("Connected servers limits reached ! ({0}) Waiting for a disconnection ...", m_clients.Count);
            }

            // thread block if the max connections limit is reached
            m_semaphore.Wait();

            // raise or not the event depending on AcceptAsync return
            if (!m_listenSocket.AcceptAsync(m_acceptArgs))
            {
                ProcessAccept(m_acceptArgs);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            IPCClient client = null;
            try
            {

                // do not accept connections while pausing
                if (IsPaused)
                {
                    logger.Warn("Pause state. Connection cancelled ...", m_semaphore.CurrentCount);
                    e.AcceptSocket.Disconnect(false);
                    m_semaphore.Release();
                    return;
                }

                client = new IPCClient(e.AcceptSocket);
                client.Disconnected += OnClientDisconnected;

                m_clients.Add(client);

                NotifyClientConnected(client);
                client.BeginReceive();

                StartAccept();
            }
            catch (Exception ex)
            {
                logger.Error("Cannot accept a connection from {0}. Exception : {1}", e.RemoteEndPoint, ex);

                if (client != null)
                    client.Disconnect();
                else
                    m_semaphore.Release();

                StartAccept();
            }
        }

        private void OnClientDisconnected(IPCClient obj)
        {
            m_clients.Remove(obj);
            m_semaphore.Release();
        }
    }
}