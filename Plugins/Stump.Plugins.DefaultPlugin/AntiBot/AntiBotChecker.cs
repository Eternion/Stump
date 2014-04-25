using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Messages.Custom;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Plugins.DefaultPlugin.AntiBot
{
    public static class AntiBotCheckerRegister
    {
        [Initialization(InitializationPass.First)]
        public static void Initialize()
        {
            WorldServer.Instance.HandlerManager.Register(typeof (AntiBotChecker));
        }
    }

    public class AntiBotChecker : WorldHandlerContainer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static string SwfPatchPath = "AntiBot.swf";

        /// <summary>
        /// In seconds
        /// </summary>
        [Variable]
        public static int KickTimeout = 5;

        private static byte[] m_patchBuffer;

        private static Dictionary<WorldClient, DateTime> m_pendingClients = new Dictionary<WorldClient, DateTime>();
        private static TimerEntry m_timeoutTimer;
        
        [WorldHandler(AuthenticationTicketMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            var patch = GetSWFPatch();

            if (patch != null)
            {
                client.Send(new RawDataMessageFixed(patch));
                m_pendingClients.Add(client, DateTime.Now);

                if (m_timeoutTimer == null)
                {
                    m_timeoutTimer = new TimerEntry(1000, 1000, TimeoutCheck);
                    WorldServer.Instance.IOTaskPool.AddTimer(m_timeoutTimer);
                }
            }
        }

        [WorldHandler(ClientKeyMessage.Id, ShouldBeLogged = false, IsGamePacket = false, IgnorePredicate = true)]
        public static void HandleClientKeyMessage(WorldClient client, ClientKeyMessage message)
        {
            /* Connected:       Yes
             * Connected to:    localhost:3467
             * Raw parser:      [object MessageReceiver]
             * Message handler: [object Worker]
             * Output buffer:   0 message(s)
             * Input buffer:    0 byte(s)*/

            if (!message.key.StartsWith("Connected"))
                return;

            var lines = message.key.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var hostAndPort = lines[1].Remove(0, "Connected to:".Length).Trim();

            var colonIndex = hostAndPort.IndexOf(":");

            if (colonIndex == -1)
                logger.Error("Cannot parse {0}", hostAndPort);
            else
            {
                var host = hostAndPort.Remove(colonIndex);

                if (host != WorldServer.ServerInformation.Address)
                {
                    logger.Warn("Client {0} is probably a BOT ! -> KICKED (Connected to {1} instead of {2})", client, host,
                        WorldServer.ServerInformation.Address);
                    client.Disconnect();
                }
            }

            m_pendingClients.Remove(client);
        }

        public static void TimeoutCheck(int dt)
        {
            var toRemove = new List<WorldClient>();
            foreach (var keyPair in m_pendingClients)
            {
                if (DateTime.Now - keyPair.Value > TimeSpan.FromSeconds(KickTimeout))
                {
                    logger.Warn("Client {0} is probably a BOT ! -> KICKED He did not send his host", keyPair.Key);
                    keyPair.Key.Disconnect();
                    toRemove.Add(keyPair.Key);
                }
            }

            foreach (var client in toRemove)
                m_pendingClients.Remove(client);
        }

        private static string GetSWFPatchPath()
        {
            return Path.Combine(Plugin.CurrentPlugin.GetPluginDirectory(), SwfPatchPath);
        }

        private static byte[] GetSWFPatch()
        {
            if (m_patchBuffer != null)
                return m_patchBuffer;


            var path = GetSWFPatchPath();
            if (File.Exists(path))
                return m_patchBuffer = File.ReadAllBytes(path);

            logger.Warn("SWF Patch not found ({0})", path);
            return null;
        }
    }
}