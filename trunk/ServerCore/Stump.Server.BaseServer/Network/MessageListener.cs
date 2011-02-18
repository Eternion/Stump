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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Pool;

namespace Stump.Server.BaseServer.Network
{
    public sealed class MessageListener
    {
        /// <summary>
        /// Current server adress
        /// </summary>
        [Variable]
        public static string Host = "localhost";

        /// <summary>
        /// Server port
        /// </summary>
        [Variable]
        public static int Port = 443;

        /// <summary>
        /// Max number of clients connected
        /// </summary>
        [Variable]
        public static int MaxConcurrentConnections = 2000;

        /// <summary>
        /// Max number of clients waiting for a connection
        /// </summary>
        [Variable]
        public static int MaxPendingConnections = 100;

        /// <summary>
        /// Max number of clients connected on the same IP or NULL for unlimited
        /// </summary>
        [Variable]
        public static int? MaxIPConnexions = 10;

        /// <summary>
        /// Min interval between two received message or NULL for unlimited ( in ms )
        /// </summary>
        [Variable]
        public static int? MinMessageInterval = 1;

        /// <summary>
        /// Buffer size /!\ Advanced users only /!\
        /// </summary>
        [Variable(DefinableByConfig = false, DefinableRunning = false)]
        public static int BufferSize = 8192;

        public event Action<MessageListener, BaseClient> ClientConnected;

        public event Action<MessageListener, BaseClient> ClientDisconnected;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs();
        private readonly List<BaseClient> m_clientList;
        private readonly SemaphoreSlim m_clientSemaphore;
        private readonly Func<Socket, BaseClient> m_delegateCreateClient;


        private readonly IPEndPoint m_ipEndPoint;

        private readonly Socket m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                            ProtocolType.Tcp);


        private readonly int m_maxConcurrentConnexion;
        private readonly QueueDispatcher m_queueDispatcher;

        private readonly SocketAsyncEventArgsPool m_readAsyncEventArgsPool;
        private readonly BufferManager m_readBufferManager;
        private readonly int m_readBufferSize;
        private readonly SocketAsyncEventArgsPool m_writeAsyncEventArgsPool;

        public MessageListener(QueueDispatcher queueDispatcher, Func<Socket, BaseClient> delegateCreateClient, string address, int port)
        {
            m_ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(address).First(ip => ip.AddressFamily == AddressFamily.InterNetwork), port);
            m_readBufferSize = BufferSize;
            m_maxConcurrentConnexion = MaxConcurrentConnections;

            m_clientList = new List<BaseClient>(m_maxConcurrentConnexion);

            m_readBufferManager = new BufferManager(m_maxConcurrentConnexion * m_readBufferSize, m_readBufferSize);

            m_readAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion);
            m_writeAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion * 2);

            m_clientSemaphore = new SemaphoreSlim(m_maxConcurrentConnexion, m_maxConcurrentConnexion);

            m_acceptArgs.Completed += OnAcceptCompleted;
            m_delegateCreateClient = delegateCreateClient;
            m_queueDispatcher = queueDispatcher;

            BaseClient.Initialize(ref m_writeAsyncEventArgsPool, ref m_queueDispatcher);
        }

        public MessageListener(QueueDispatcher queueDispatcher, Func<Socket, BaseClient> delegateCreateClient)
        {
            m_ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(Host).First(ip => ip.AddressFamily == AddressFamily.InterNetwork), Port);
            m_readBufferSize = BufferSize;
            m_maxConcurrentConnexion = MaxConcurrentConnections;

            m_clientList = new List<BaseClient>(m_maxConcurrentConnexion);

            m_readBufferManager = new BufferManager(m_maxConcurrentConnexion * m_readBufferSize, m_readBufferSize);

            m_readAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion);
            m_writeAsyncEventArgsPool = new SocketAsyncEventArgsPool(m_maxConcurrentConnexion * 2);

            m_clientSemaphore = new SemaphoreSlim(m_maxConcurrentConnexion, m_maxConcurrentConnexion);

            m_acceptArgs.Completed += OnAcceptCompleted;
            m_delegateCreateClient = delegateCreateClient;
            m_queueDispatcher = queueDispatcher;

            BaseClient.Initialize(ref m_writeAsyncEventArgsPool, ref m_queueDispatcher);
        }

        public IEnumerable<BaseClient> ClientList
        {
            get { return m_clientList; }
        }

        public void Initialize()
        {
            m_readBufferManager.InitBuffer();

            SocketAsyncEventArgs args;

            // initialize read pool
            for (var i = 0; i < m_maxConcurrentConnexion; i++)
            {
                args = new SocketAsyncEventArgs();

                m_readBufferManager.SetBuffer(args);
                args.Completed += OnReceiveCompleted;
                m_readAsyncEventArgsPool.Push(args);
            }

            // initialize write pool
            for (var i = 0; i < m_maxConcurrentConnexion * 2; i++)
            {
                args = new SocketAsyncEventArgs();

                args.Completed += OnSendCompleted;
                m_writeAsyncEventArgsPool.Push(args);
            }
        }

        public void Start()
        {
            m_listenSocket.Bind(m_ipEndPoint);
            m_listenSocket.Listen(MaxPendingConnections);

            StartAccept();
        }

        public void Stop()
        {
            m_listenSocket.Close();
        }

        private void StartAccept()
        {
            m_acceptArgs.AcceptSocket = null;

            m_clientSemaphore.Wait();

            if (!m_listenSocket.AcceptAsync(m_acceptArgs))
            {
                ProcessAccept(m_acceptArgs);
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (MaxIPConnexions.HasValue && GetSameIpNumber((e.AcceptSocket.RemoteEndPoint as IPEndPoint).Address) > MaxIPConnexions.Value)
            {
                logger.Error("Client {0} try to connect more {1} time", e.AcceptSocket.RemoteEndPoint.ToString(), MaxIPConnexions.Value);
                m_clientSemaphore.Release();
                StartAccept();
                return;
            }

            SocketAsyncEventArgs readAsyncEventArgs = m_readAsyncEventArgsPool.Pop();

            var client = m_delegateCreateClient(e.AcceptSocket);
            readAsyncEventArgs.UserToken = client;

            m_clientList.Add(client);

            if (ClientConnected != null) ClientConnected(this, client);

            if (!client.Socket.ReceiveAsync(readAsyncEventArgs))
            {
                ProcessReceive(readAsyncEventArgs);
            }

            StartAccept();
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e);
                        break;
                    case SocketAsyncOperation.Disconnect:
                        CloseClientSocket(e);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                logger.Error("Last chance exception on receiving ! : " + exception);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                var client = e.UserToken as BaseClient;

                if (client != null)
                {
                    client.ProcessReceive(e.Buffer, e.Offset, e.BytesTransferred);

                    if (client.Socket != null)
                    {
                        bool willRaiseEvent = client.Socket.ReceiveAsync(e);

                        if (!willRaiseEvent)
                        {
                            ProcessReceive(e);
                        }
                    }
                    else
                    {
                        CloseClientSocket(e);
                    }
                }
                else
                {
                    CloseClientSocket(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            m_writeAsyncEventArgsPool.Push(e);
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var client = e.UserToken as BaseClient;

            if (client != null)
            {
                client.Disconnect();
                m_clientList.Remove(client);

                if (ClientDisconnected != null) ClientDisconnected(this, client);
            }
            m_clientSemaphore.Release();

            // Free the SocketAsyncEventArg so they can be reused by another client
            m_readAsyncEventArgsPool.Push(e);
        }

        /// <summary>
        /// Gets the number of same client .
        /// </summary>
        /// <param name="ip">The IP.</param>
        /// <returns></returns>
        private int GetSameIpNumber(IPAddress ip)
        {
            return m_clientList.Count(t => t.Socket != null && (t.Socket.RemoteEndPoint as IPEndPoint).Address == ip);
        }
    }
}