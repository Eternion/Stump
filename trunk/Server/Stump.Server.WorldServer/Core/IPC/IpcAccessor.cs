using System;
using System.Net.Sockets;
using System.ServiceModel;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Timers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IpcAccessor : Singleton<IpcAccessor>, IDisposable
    {
        /// <summary>
        ///   Delay in seconds where we should retry connecting to remote server. (in seconds)
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int ReconnectDelay = 10;

        /// <summary>
        /// Delay between two server update (in seconds)
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int UpdateInterval = 5;

        /// <summary>
        /// IPC server adress
        /// </summary>
        [Variable]
        public static string IpcAuthAddress = "net.tcp://localhost:9100";

        /// <summary>
        /// IPC world port
        /// </summary>
        [Variable]
        public static string IpcWorldAddress = "net.tcp://localhost:9101";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Properties

        private TimerEntry m_maintainConnectionTimer;
        private AuthClientAdapter m_proxyObject;
        private bool m_running;

        private bool m_isConnected;

        public bool IsConnected
        {
            get { return m_isConnected && m_proxyObject.State == CommunicationState.Opened; }
            private set { m_isConnected = value; }
        }

        public AuthClientAdapter ProxyObject
        {
            get
            {
                if (!IsConnected)
                    throw new Exception("Attempt to call the authentification server by ipc whithout beeing registered");

                return m_proxyObject;
            }
            private set { m_proxyObject = value; }
        }

        public IpcHost IpcHost
        {
            get;
            private set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            IpcHost.Stop();
            IsConnected = false;
            ProxyObject = null;
            GC.SuppressFinalize(this);
        }

        #endregion

        public event Action<IpcAccessor> Connected;

        protected virtual void OnConnected()
        {
            IsConnected = true;

            Action<IpcAccessor> handler = Connected;
            if (handler != null)
                handler(this);
        }

        public event Action<IpcAccessor> Disconnected;

        protected virtual void OnDisconnected()
        {
            IsConnected = false;
            ProxyObject = null;

            Action<IpcAccessor> handler = Disconnected;
            if (handler != null)
                handler(this);
        }

        public void Initialize()
        {
            IsConnected = false;
            IpcHost = new IpcHost(typeof(IpcOperations), typeof(IRemoteWorldOperations), IpcWorldAddress);
            IpcHost.Open();
        }

        /// <summary>
        ///   Actually attempt to connect to remote server and retrieve a
        ///   proxy object.
        /// </summary>
        private bool Connect()
        {
            var binding = new NetTcpBinding {Security = {Mode = SecurityMode.None}};
            var proxyobject = new AuthClientAdapter(binding, new EndpointAddress(IpcAuthAddress));
            proxyobject.Error += OnOperationError;

            try
            {
                proxyobject.Open();

                var result = proxyobject.RegisterWorld(WorldServer.ServerInformation, IpcWorldAddress);

                if (result == RegisterResultEnum.IpNotAllowed ||
                    result == RegisterResultEnum.PropertiesMismatch)
                {
                    logger.Error("[IPC] The authentication server has denied the access of this server.");
                    WorldServer.Instance.Shutdown();
                }
                else if (result == RegisterResultEnum.OK)
                {
                    ProxyObject = proxyobject;

                    logger.Info("[IPC] Server connected to the authentification server");
                    OnConnected();
                }
                else if (result == RegisterResultEnum.AuthServerUnreachable)
                {
                    // do not notify anything because it calls OnOperationError
                }
                else
                {
                    logger.Warn("Cannot connect IPC service, error = {0}", result);
                }

                return true;
            }
            catch (EndpointNotFoundException)
            {
                logger.Warn("{0} not found", IpcAuthAddress);

                return false;
            }
                // actually this is not an communication problem but we don't
                // considered server as connected for a security reason.
            catch (Exception ex)
            {
                throw new Exception("[IPC] Ping connection throw an exception : " + ex.Message);
            }
        }

        private bool Disconnect()
        {
            // check whenever the connection is not already closed or closing
            if (m_proxyObject != null &&
                m_proxyObject.State != CommunicationState.Closed &&
                m_proxyObject.State != CommunicationState.Closing)
            {
                try
                {
                    if (m_proxyObject.State == CommunicationState.Opened)
                        m_proxyObject.UnRegisterWorld();

                    m_proxyObject.Close();
                }
                catch (Exception)
                {

                }
                finally
                {
                    m_proxyObject = null;
                }

                OnDisconnected();
                return true;
            }

            return false;
        }

        private void OnOperationError(Exception ex)
        {
            if (ex is CommunicationException)
            {
                // Connection got interrupted
                logger.Warn("[IPC] Lost connection to AuthServer. Scheduling reconnection attempt...");
            }
            else
            {
                logger.Error("[IPC] Exception occurs on IPC method access : {0} \nScheduling reconnection attempt...", ex);
            } 
            
            Disconnect();
        }

        /// <summary>
        ///   Actually start to connect to remote server.
        /// </summary>
        public void Start()
        {
            m_running = true;

            if (m_maintainConnectionTimer == null || !m_maintainConnectionTimer.IsRunning)
            {
                m_maintainConnectionTimer = new TimerEntry(0, UpdateInterval * 1000, MaintainConnection);
                WorldServer.Instance.IOTaskPool.AddTimer(m_maintainConnectionTimer);
                m_maintainConnectionTimer.Start();
            }
        }

        public void Stop()
        {
            m_running = false;
        }

        /// <summary>
        ///   Running on his own context, we ping regularly here remote server.
        /// </summary>
        private void MaintainConnection(int dt)
        {
            if (!m_running)
            {
                if (IsConnected)
                {
                    Disconnect();
                    logger.Info("[IPC] Server disconnected from the authentification server");
                }
            }
            else if (!IsConnected)
            {
                if (WorldServer.Instance.Running)
                {
                    Connect();
                }
            }

            else
            {
                m_proxyObject.UpdateConnectedChars(World.Instance.CharacterCount);
            }
        }
    }
}