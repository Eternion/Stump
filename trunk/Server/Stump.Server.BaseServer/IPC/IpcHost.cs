using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using NLog;

namespace Stump.Server.BaseServer.IPC
{
    public class IpcHost
    {
        private readonly Type m_hostType;
        private readonly Type m_serviceType;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ServiceHost m_host;

        public IpcHost(Type hostType, Type serviceType, string ipcAddress)
        {
            m_hostType = hostType;
            m_serviceType = serviceType;
            IpcAddress = ipcAddress;
        }

        public bool IsOpen
        {
            get { return m_host != null && m_host.State == CommunicationState.Opened; }
        }

        public string IpcAddress
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            private set;
        }

        public void Open()
        {
            if (IsOpen)
                Stop();

            m_host = new ServiceHost(m_hostType);

            var binding = new NetTcpBinding
                              {
                                  Security =
                                      {
                                          Mode = SecurityMode.None,
                                      },
                                  ReceiveTimeout = TimeSpan.FromSeconds(10),
                              };

            ServiceEndpoint endPoint = m_host.AddServiceEndpoint(
                m_serviceType,
                binding,
                IpcAddress);

            m_host.Open();

            string addr = m_host.Description.Endpoints[0].ListenUri.AbsoluteUri;

            Running = true;
            logger.Info("IPC Service started ({0})", addr);
        }

        public void Stop()
        {
            if (m_host != null && m_host.State != CommunicationState.Closed && m_host.State != CommunicationState.Faulted)
            {
                try
                {
                    m_host.Close();
                }
                catch (Exception)
                {
                    // close it anyway
                }
            }

            Running = false;
            logger.Info("IPC Service get stopped");
            m_host = null;
        }
    }
}