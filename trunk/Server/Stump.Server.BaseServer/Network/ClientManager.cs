using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Pool;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Network
{
    public class ClientManager : Singleton<ClientManager>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Config Variables
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
        [Variable]
        public static int BufferSize = 8192; 
        #endregion

        #region Events
        public event Action<BaseClient> ClientConnected;

        private void NotifyClientConnected(BaseClient client)
        {
            Action<BaseClient> handler = ClientConnected;
            if (handler != null) handler(client);
        }

        public event Action<BaseClient> ClientDisconnected;

        private void NotifyClientDisconnected(BaseClient client)
        {
            Action<BaseClient> handler = ClientDisconnected;
            if (handler != null) handler(client);
        } 
        #endregion

        public delegate BaseClient CreateClientHandler(Socket clientSocket);

        private CreateClientHandler m_createClientDelegate;

        private readonly Socket m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);

        private readonly List<BaseClient> m_clients = new List<BaseClient>();

        private BufferManager m_bufferManager; // allocate memory dedicated to a client to avoid memory alloc on each send/recv
        private SocketAsyncEventArgsPool m_readAsyncEventArgsPool; // pool of SocketAsyncEventArgs
        private SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs(); // async arg used on client connection
        private SemaphoreSlim m_semaphore; // limit the number of threads accessing to a ressource

        private AutoResetEvent m_resumeEvent = new AutoResetEvent(false);

        /// <summary>
        /// List of connected Clients
        /// </summary>
        public ReadOnlyCollection<BaseClient> Clients
        {
            get { return m_clients.AsReadOnly(); }
        }

        public int Count
        {
            get
            {
                return m_clients.Count;
            }
        }

        public bool Paused
        {
            get;
            private set;
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public bool Started
        {
            get;
            private set;
        }

        public string Host
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public bool IsFull
        {
            get;
            private set;
        }

        public void Initialize(CreateClientHandler createClientHandler)
        {
            if (IsInitialized)
                throw new Exception("ClientManager already initialized");

            m_createClientDelegate = createClientHandler;

            // init semaphore
            m_semaphore = new SemaphoreSlim(MaxConcurrentConnections, MaxConcurrentConnections);

            // init buffer manager
            m_bufferManager = new BufferManager(MaxConcurrentConnections * BufferSize, BufferSize);
            m_bufferManager.InitializeBuffer();

            // initialize read pool
            m_readAsyncEventArgsPool = new SocketAsyncEventArgsPool(MaxConcurrentConnections);
            for (var i = 0; i < MaxConcurrentConnections; i++)
            {
                var args = new SocketAsyncEventArgs();

                m_bufferManager.SetBuffer(args);
                args.Completed += OnReceiveCompleted;
                m_readAsyncEventArgsPool.Push(args);
            }

            m_acceptArgs.Completed += (sender, e) => ProcessAccept(e);

            IsInitialized = true;
        }

        /// <summary>
        /// Start to listen client connections
        /// </summary>
        public void Start(string address, int port)
        {
            if (!IsInitialized)
                throw new Exception("Attempt to start ClientManager before initializing it. Call Initialize()");

            if (Started)
                throw new Exception("ClientManager already started");

            Host = address;
            Port = port;

            var ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(Host).First(ip => ip.AddressFamily == AddressFamily.InterNetwork), Port);
            m_listenSocket.Bind(ipEndPoint);
            m_listenSocket.Listen(MaxPendingConnections);

            Started = true;

            StartAccept();
        }

        /// <summary>
        /// Pause the listener and reject all new connections
        /// </summary>
        public void Pause()
        {
            Paused = true;
        }

        /// <summary>
        /// Resume the actual pause
        /// </summary>
        public void Resume()
        {
            Paused = false;

            m_resumeEvent.Set();
        }

        /// <summary>
        /// Close the listener and dispose ressources
        /// </summary>
        public void Close()
        {
            // interrupt accept process
            Paused = true;

            m_listenSocket.Close();
            m_listenSocket.Dispose();

            m_bufferManager.Dispose();

            m_readAsyncEventArgsPool.Dispose();
        }

        private void StartAccept()
        {
            m_acceptArgs.AcceptSocket = null;

            if (m_semaphore.CurrentCount == 0)
            {
                logger.Warn("Connected clients limits reached ! ({0}) Waiting for a disconnection ...", Count);
                IsFull = true;
            }

            // thread block if the max connections limit is reached
            m_semaphore.Wait();

            if (IsFull)
            {
                IsFull = false;
                logger.Warn("A client get disconnected, connection allowed", m_semaphore.CurrentCount);
            }

            // raise or not the event depending on AcceptAsync return
            if (!m_listenSocket.AcceptAsync(m_acceptArgs))
            {
                ProcessAccept(m_acceptArgs);
            }
        }

        /// <summary>
        /// Called when a new client is connecting
        /// </summary>
        /// <param name="e"></param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs readAsyncEventArgs = null;
            try
            {
                // do not accept connections while pausing
                if (Paused)
                {
                    logger.Warn("Pause state. Connection pending ...", m_semaphore.CurrentCount);
                    // if paused wait until Resume() is called
                    m_resumeEvent.WaitOne();
                }

                if (MaxIPConnexions.HasValue &&
                    CountClientWithSameIp(( (IPEndPoint)e.AcceptSocket.RemoteEndPoint ).Address) > MaxIPConnexions.Value)
                {
                    logger.Error("Client {0} try to connect more then {1} times", e.AcceptSocket.RemoteEndPoint.ToString(), MaxIPConnexions.Value);
                    m_semaphore.Release();

                    StartAccept();
                    return;
                }

                // use a async arg from the pool avoid to re-allocate memory on each connection
                readAsyncEventArgs = PopReadSocketAsyncArgs();

                // create the client instance
                var client = m_createClientDelegate(e.AcceptSocket);
                readAsyncEventArgs.UserToken = client;

                lock (m_clients)
                    m_clients.Add(client);

                NotifyClientConnected(client);

                // if the event is not raised we first check new connections before parsing message that can blocks the connection queue
                if (!client.Socket.ReceiveAsync(readAsyncEventArgs))
                {
                    StartAccept();
                    ProcessReceive(readAsyncEventArgs);
                }
                else
                {
                    StartAccept();
                }

            }
            catch (Exception ex)
            {
                // if an error occurs we do our possible to reset all possible allocated ressources
                logger.Error("Cannot accept a connection from {0}. Exception : {1}", e.RemoteEndPoint, ex);

                m_semaphore.Release();

                if (readAsyncEventArgs != null)
                    PushReadSocketAsyncArgs(readAsyncEventArgs);

                if (e.AcceptSocket != null)
                    e.AcceptSocket.Disconnect(false);


                StartAccept();
            }
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
                // theoretically it shouldn't go up to there.
                logger.Error("Last chance exception on receiving ! : " + exception);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
            {
                CloseClientSocket(e);
            }
            else
            {
                var client = e.UserToken as BaseClient;

                if (client == null)
                {
                    CloseClientSocket(e);
                }
                else
                {
                    client.ProcessReceive(e.Buffer, e.Offset, e.BytesTransferred);

                    if (client.Socket == null)
                    {
                        CloseClientSocket(e);
                    }
                    else
                    {
                        // just continue to receive
                        bool willRaiseEvent = client.Socket.ReceiveAsync(e);

                        if (!willRaiseEvent)
                        {
                            ProcessReceive(e);
                        }
                    }
                }
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var client = e.UserToken as BaseClient;

            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    logger.Error("Last catch. Disconnection of {0} failed : {1}", client, ex);
                }
                finally
                {
                    bool removed;
                    lock (m_clients)
                        removed = m_clients.Remove(client);

                    if (removed)
                    {
                        NotifyClientDisconnected(client);

                        m_semaphore.Release();
                    }

                    // free the SocketAsyncEventArg so it can be reused by another client
                    PushReadSocketAsyncArgs(e);
                }
            }
            else
            {
                // free the SocketAsyncEventArg so it can be reused by another client
                PushReadSocketAsyncArgs(e);
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            // free an async arg on the pool when a send is completed
            PushWriteSocketAsyncArgs(e);
        }

        public SocketAsyncEventArgs PopWriteSocketAsyncArgs()
        {
            return new SocketAsyncEventArgs();
        }

        public void PushWriteSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            // not used anymore
        }

        public SocketAsyncEventArgs PopReadSocketAsyncArgs()
        {
            if (m_readAsyncEventArgsPool.Count <= 0)
                throw new Exception("The reader async argument pool is empty");

            return m_readAsyncEventArgsPool.Pop();
        }

        public void PushReadSocketAsyncArgs(SocketAsyncEventArgs args)
        {
            m_readAsyncEventArgsPool.Push(args);
        }

        public BaseClient[] FindAll(Predicate<BaseClient> predicate)
        {
            lock (m_clients)
            {
                return m_clients.Where(entry => predicate(entry)).ToArray();
            }
        }

        public T[] FindAll<T>(Predicate<T> predicate)
        {
            lock (m_clients)
            {
                return m_clients.OfType<T>().Where(entry => predicate(entry)).ToArray();
            }
        }

        public T[] FindAll<T>()
        {
            lock (m_clients)
            {
                return m_clients.OfType<T>().ToArray();
            }
        }

        public int CountClientWithSameIp(IPAddress ipAddress)
        {
            lock (m_clients)
            {
                return
                    m_clients.Count(
                        t =>
                        t.Socket != null && t.Socket.Connected &&
                        ((IPEndPoint) t.Socket.RemoteEndPoint).Address.Equals(ipAddress));
            }
        }
    }
}