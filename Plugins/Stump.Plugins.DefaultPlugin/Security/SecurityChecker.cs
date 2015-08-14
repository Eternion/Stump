using System;
using System.Collections.Generic;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Cryptography;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Plugins.DefaultPlugin.Security
{
    public static class SecurityCheckerRegister
    {
        [Initialization(InitializationPass.First)]
        public static void Initialize()
        {
            WorldServer.Instance.IOTaskPool.CallPeriodically(5000, SecurityChecker.TimeoutCheck);
            WorldServer.Instance.HandlerManager.Register(typeof (SecurityChecker));
        }
    }

    public class SecurityChecker : WorldHandlerContainer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// In seconds
        /// </summary>
        [Variable]
        public static int KickTimeout = 15;

        private static readonly List<Tuple<string, string, int>> filesToCheck = new List<Tuple<string, string, int>>
        {
            new Tuple<string, string, int>(Cryptography.GetMD5Hash("data/common/Notifications.d2o"), "data/common/Notifications.d2o", 1583)
        };

        private static readonly Dictionary<WorldClient, DateTime> m_pendingClients = new Dictionary<WorldClient, DateTime>();

        [WorldHandler(AuthenticationTicketMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            PerformCheck(client);
        }

        [WorldHandler(CheckFileMessage.Id, ShouldBeLogged = false, IsGamePacket = false, IgnorePredicate = true)]
        public static void HandleCheckFileMessage(WorldClient client, CheckFileMessage message)
        {
            var file = filesToCheck.Find(x => x.Item1 == message.filenameHash);
            if (file == null)
                return;

            int value;
            int.TryParse(message.value, out value);

            if (value == file.Item3)
                m_pendingClients.Remove(client);
        }

        public static void PerformCheck(WorldClient client)
        {
            if (client.UserGroup.IsGameMaster)
                return;

            if (m_pendingClients.ContainsKey(client))
                return;

            foreach (var file in filesToCheck)
                client.Send(new CheckFileRequestMessage(file.Item2, 0));

            m_pendingClients.Add(client, DateTime.Now);
        }

        public static void TimeoutCheck()
        {
            var toRemove = new List<WorldClient>();
            foreach (var keyPair in m_pendingClients)
            {
                if (keyPair.Key == null)
                {
                    toRemove.Add(keyPair.Key);
                    continue;
                }

                if (DateTime.Now - keyPair.Value <= TimeSpan.FromSeconds(KickTimeout))
                    continue;

                logger.Warn("Client {0} isn't legit ! -> KICKED", keyPair.Key);
                keyPair.Key.Disconnect();
                toRemove.Add(keyPair.Key);
            }

            foreach (var client in toRemove)
                m_pendingClients.Remove(client);
        }
    }
}