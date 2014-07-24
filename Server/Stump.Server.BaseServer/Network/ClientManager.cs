using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
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
            var handler = ClientConnected;
            if (handler != null) handler(client);
        }

        public event Action<BaseClient> ClientDisconnected;

        private void NotifyClientDisconnected(BaseClient client)
        {
            var handler = ClientDisconnected;
            if (handler != null) handler(client);
        } 

        #endregion

        public delegate BaseClient CreateClientHandler(Socket clientSocket);

        private CreateClientHandler m_createClientDelegate;

        private readonly Socket m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                                    ProtocolType.Tcp);

        private readonly List<BaseClient> m_clients = new List<BaseClient>();

        private readonly SocketAsyncEventArgs m_acceptArgs = new SocketAsyncEventArgs(); // async arg used on client connection
        private SemaphoreSlim m_semaphore; // limit the number of threads accessing to a ressource

        private readonly AutoResetEvent m_resumeEvent = new AutoResetEvent(false);

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

            if (!ObjectPoolMgr.ContainsType<SocketAsyncEventArgs>())
            {
                ObjectPoolMgr.RegisterType(() => new SocketAsyncEventArgs());
                ObjectPoolMgr.SetMinimumSize<SocketAsyncEventArgs>(100);
            }

            m_createClientDelegate = createClientHandler;

            // init semaphore
            m_semaphore = new SemaphoreSlim(MaxConcurrentConnections, MaxConcurrentConnections);
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
            m_listenSocket.NoDelay = true;
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

                try
                {
                    var IP = ((IPEndPoint)e.AcceptSocket.RemoteEndPoint).Address;

                    if (MaxIPConnexions.HasValue && CountClientWithSameIp(IP) > MaxIPConnexions.Value)
                    {
                        logger.Error("Client {0} try to connect more then {1} times", e.AcceptSocket.RemoteEndPoint.ToString(), MaxIPConnexions.Value);
                        m_semaphore.Release();

                        StartAccept();
                        return;
                    }
                }
                catch(Exception ex)
                {
                    logger.Error("Invalid remote end-point. Exception : {0}", ex);
                    m_semaphore.Release();

                    StartAccept();
                    return;
                }

                // use a async arg from the pool avoid to re-allocate memory on each connection
                readAsyncEventArgs = PopSocketArg();

                // create the client instance
                var client = m_createClientDelegate(e.AcceptSocket);
                readAsyncEventArgs.UserToken = client;

                lock (m_clients)
                    m_clients.Add(client);

                NotifyClientConnected(client);

                client.BeginReceive();

                StartAccept();
            }
            catch (Exception ex)
            {
                // if an error occurs we do our possible to reset all possible allocated ressources
                logger.Error("Cannot accept a connection from {0}. Exception : {1}", e.RemoteEndPoint, ex);

                m_semaphore.Release();

                if (readAsyncEventArgs != null)
                    PushSocketArg(readAsyncEventArgs);

                if (e.AcceptSocket != null)
                {
                    if (e.AcceptSocket.Connected)
                        e.AcceptSocket.Disconnect(false);
                }


                StartAccept();
            }
        }

        public void OnClientDisconnected(BaseClient client)
        {
            bool removed;
            lock (m_clients)
                removed = m_clients.Remove(client);

            if (!removed)
                return;

            NotifyClientDisconnected(client);

            m_semaphore.Release();
        }

        public SocketAsyncEventArgs PopSocketArg()
        {
            var arg = ObjectPoolMgr.ObtainObject<SocketAsyncEventArgs>();
            return arg;
        }

        public void PushSocketArg(SocketAsyncEventArgs args)
        {
            ObjectPoolMgr.ReleaseObject(args);
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