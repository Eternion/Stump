
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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Reflection;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer.IPC
{
    public class IpcServer : Singleton<IpcServer>
    {
        /// <summary>
        /// IPC port
        /// </summary>
        [Variable]
        public static int IpcPort = 9100;

        /// <summary>
        /// Secret key to use to confirm World Server access
        /// </summary>
        [Variable]
        public static string IpcSecretKey = "000stump000";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConcurrentDictionary<WorldServerInformation, IRemoteOperationsWorld> m_ipcclients =
            new ConcurrentDictionary<WorldServerInformation, IRemoteOperationsWorld>();

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

        public void RegisterTcpClient(WorldServerInformation wsi, int channelPort)
        {
            string ipcadress = string.Format("tcp://{0}:{1}/", wsi.Address, channelPort);

            var remoteobject =
                (IRemoteOperationsWorld) Activator.GetObject(typeof (IRemoteOperationsWorld), ipcadress + "Remoting");

            m_ipcclients.TryAdd(wsi, remoteobject);
        }

        public void UnRegisterTcpClient(WorldServerInformation wsi)
        {
            IEnumerable<KeyValuePair<WorldServerInformation, IRemoteOperationsWorld>> clients =
                m_ipcclients.Where(entry => entry.Key.Id == wsi.Id);

            IRemoteOperationsWorld value;
            foreach (var client in clients)
                m_ipcclients.TryRemove(client.Key, out value);
        }

        public IRemoteOperationsWorld GetIpcClient(int id)
        {
            IEnumerable<KeyValuePair<WorldServerInformation, IRemoteOperationsWorld>> client =
                m_ipcclients.Where(entry => entry.Key.Id == id);

            if (client.Count() <= 0)
                return null;

            return client.First().Value;
        }

        public IRemoteOperationsWorld[] GetIpcClients()
        {
            return m_ipcclients.Values.ToArray();
        }
    }
}