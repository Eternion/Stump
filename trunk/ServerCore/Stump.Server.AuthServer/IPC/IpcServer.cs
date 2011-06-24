
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Principal;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.AuthServer.IPC
{
    public class IpcServer : Singleton<IpcServer>
    {
        /// <summary>
        /// IPC port
        /// </summary>
        [Variable]
        public static int IpcPort = 9100;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConcurrentDictionary<int, IRemoteOperationsWorld> m_ipcclients =
            new ConcurrentDictionary<int, IRemoteOperationsWorld>();

        public void Initialize()
        {
            try
            {
                var channel = new TcpChannel(IpcPort);

                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof (IpcOperations),
                    "Remoting",
                    WellKnownObjectMode.Singleton);

                logger.Info("IPC Started!");
            }
            catch (RemotingException re)
            {
                logger.Error("[IPC] An Exception occurred : " + re.Message);
            }
        }

        public void RegisterTcpClient(WorldServerData wsi, int channelPort)
        {
            string ipcadress = string.Format("tcp://{0}:{1}/", wsi.Address, channelPort);

            var remoteobject =
                (IRemoteOperationsWorld) Activator.GetObject(typeof (IRemoteOperationsWorld), ipcadress + "Remoting");

            if (!m_ipcclients.TryAdd(wsi.Id, remoteobject))
                throw new Exception(string.Format("Server already registred with id '{0}'", wsi.Id));
        }

        public void UnRegisterTcpClient(WorldServerData wsi)
        {
            UnRegisterTcpClient(wsi.Id);
        }

        public void UnRegisterTcpClient(int id)
        {
            IRemoteOperationsWorld value;
            if (!m_ipcclients.TryRemove(id, out value))
                throw new Exception(string.Format("Cannot remove server with id '{0}', maybe it doesn't exist more", id));
        }

        public IRemoteOperationsWorld GetIpcClient(int id)
        {
            IRemoteOperationsWorld client;
            m_ipcclients.TryGetValue(id, out client);

            return client;
        }

        public IRemoteOperationsWorld[] GetIpcClients()
        {
            return m_ipcclients.Values.ToArray();
        }
    }
}