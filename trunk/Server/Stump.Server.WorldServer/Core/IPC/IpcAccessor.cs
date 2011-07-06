using System;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.WorldServer.Core.IPC
{
    public class IpcAccessor : Singleton<IpcAccessor>, IDisposable
    {
        /// <summary>
        ///   Delay in seconds where we should retry connecting to remote server.
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int ReconnectDelay = 10;

        /// <summary>
        /// Delay for a ping check
        /// </summary>
        [Variable(DefinableRunning = true)]
        public static int PingDelay = 200;

        /// <summary>
        /// IPC server adress
        /// </summary>
        [Variable]
        public static string IpcAuthAdress = "localhost";

        /// <summary>
        /// IPC authentification port
        /// </summary>
        [Variable]
        public static short IpcAuthPort = 9100;

        /// <summary>
        /// IPC world port
        /// </summary>
        [Variable]
        public static short IpcWorldPort = 9101;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Object to sync up when needed.
        /// </summary>
        private readonly object m_synclock = new object();

        /// <summary>
        ///   Indicate if we are disposing or not.
        /// </summary>
        private bool m_disposing;

        public bool IsRegister
        {
            get;
            private set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void Initialize()
        {
            // data to connect to the auth ipc server
            IpcAddress = string.Format("tcp://{0}:{1}/", IpcAuthAdress, IpcAuthPort);
            Connected = false;

            // register world ipc server
            var channel = new TcpChannel(IpcWorldPort); // the used port 

            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof (IpcOperations),
                "Remoting",
                WellKnownObjectMode.Singleton);
        }

        /// <summary>
        ///   Actually attempt to connect to remote server and retrieve a
        ///   proxy object.
        /// </summary>
        private bool Connect()
        {
            var proxyobject =
                (IRemoteOperationsAuth) Activator.GetObject(typeof (IRemoteOperationsAuth), IpcAddress + "Remoting");
            try
            {
                proxyobject.PingConnection(WorldServer.ServerInformation);
                ProxyObject = proxyobject;

                return true;
            }
            catch (SocketException)
            {
                return false;
            }
                // actually this is not an communication problem but we don't
                // considered server as connected for a security reason.
            catch (Exception ex)
            {
                throw new Exception("[IPC] Ping connection throw an exception : " + ex.Message);
            }
        }

        public void RegisterWorld()
        {
            bool haveToSave = string.IsNullOrEmpty(WorldServer.ServerInformation.Password);

            if (m_proxyObject.RegisterWorld(ref WorldServer.ServerInformation, IpcWorldPort))
            {
                IsRegister = true;
                logger.Info("[IPC] Connection from the authentification server granted");

                if (haveToSave)
                {
                    logger.Info("[IPC] Save the new configuration file");

                    WorldServer.Instance.IgnoreNextConfigReload();
                    WorldServer.Instance.Config.Save();
                }

                Task.Factory.StartNew(MaintainConnection);
            }
            else
            {
                logger.Error("[IPC] The authentication server has denied the access of this server.");
                WorldServer.Instance.Shutdown();
            }
        }

        /// <summary>
        ///   Actually start to connect to remote server.
        /// </summary>
        public void Start()
        {
            lock (m_synclock)
            {
                if (!m_disposing)
                {
                    if (Connect())
                    {
                        Connected = true;
                        logger.Info("[IPC] Found Authenfication Server {0}. Wait to be register...", IpcAddress);
                        RegisterWorld();
                    }

                    else if (!WorldServer.Instance.Running)
                    {
                        Connected = false;
                        return;
                    }
                    else
                    {
                        Connected = false;

                        logger.Warn("[IPC] Couldn't connect to : {0} Retrying in : {1} seconds...", IpcAddress,
                                    ReconnectDelay);
                        Task.Factory.StartNewDelayed(ReconnectDelay*1000, Start);
                    }
                }
            }
        }

        /// <summary>
        ///   Running on his own context, we ping regularly here remote server.
        /// </summary>
        private void MaintainConnection()
        {
            while (Connected && WorldServer.Instance.Running)
            {
                try
                {
                    if (!ProxyObject.PingConnection(WorldServer.ServerInformation) && IsRegister)
                    {
                        // No pong. Connection closed (time out)
                        Connected = false;
                        logger.Warn("Lost connection to : {0}.\nReconnecting in {1} seconds...", IpcAddress,
                                    ReconnectDelay);
                        
                        NotifyConnectionLost();
                        Task.Factory.StartNewDelayed(ReconnectDelay * 1000, Start);
                    }
                }
                catch (SocketException)
                {
                    Connected = false;
                    logger.Warn("Lost connection to : {0}.\nReconnecting in {1} seconds...", IpcAddress, ReconnectDelay);
                    
                    NotifyConnectionLost();
                    Task.Factory.StartNewDelayed(ReconnectDelay * 1000, Start);
                }
                catch(Exception e)
                {
                    logger.Error("Exception raised when pinging the auth serveur : " + e);
                }

                Thread.Sleep(PingDelay);
            }
        }

        /// <summary>
        ///   Disconnect this client from remote server.
        /// </summary>
        public void Disconnect()
        {
            Dispose();
        }

        /// <summary>
        ///   Notify to WorldServer instance that we lost connection to remote server.
        /// </summary>
        private void NotifyConnectionLost()
        {
            lock (m_synclock)
            {
                IsRegister = false;
                ProxyObject = null;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_disposing = true;
                ProxyObject = null;
            }
        }

        #region Properties

        private IRemoteOperationsAuth m_proxyObject;

        public string IpcAddress
        {
            get;
            set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public IRemoteOperationsAuth ProxyObject
        {
            get
            {
                if (!IsRegister)
                    throw new Exception("Attempt to call the authentification server by ipc whithout beeing register");

                return m_proxyObject;
            }
            private set { m_proxyObject = value; }
        }

        #endregion
    }
}