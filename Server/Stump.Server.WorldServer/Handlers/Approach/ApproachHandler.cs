using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;

namespace Stump.Server.WorldServer.Handlers.Approach
{
    public class ApproachHandler : WorldHandlerContainer
    {
        public static SynchronizedCollection<WorldClient> ConnectionQueue = new SynchronizedCollection<WorldClient>();

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static Task m_queueRefresherTask;

        [Initialization(InitializationPass.First)]
        private static void Initialize()
        {
            m_queueRefresherTask = Task.Factory.StartNewDelayed(3000, RefreshQueue);
        }

        private static void RefreshQueue()
        {
            try
            {
                var toRemove = new List<WorldClient>();
                var count = 0;
                lock (ConnectionQueue.SyncRoot)
                {
                    foreach (var worldClient in ConnectionQueue)
                    {
                        count++;

                        if (!worldClient.Connected)
                        {
                            toRemove.Add(worldClient);
                        }

                        if (DateTime.Now - worldClient.InQueueUntil <= TimeSpan.FromSeconds(3))
                            continue;

                        SendQueueStatusMessage(worldClient, (ushort)count, (ushort)ConnectionQueue.Count);
                        worldClient.QueueShowed = true;
                    }

                    foreach (var worldClient in toRemove)
                    {
                        ConnectionQueue.Remove(worldClient);
                    }
                }
            }
            finally 
            {
                m_queueRefresherTask = Task.Factory.StartNewDelayed(3000, RefreshQueue);
            }
        }

        [WorldHandler(AuthenticationTicketMessage.Id, ShouldBeLogged = false, IsGamePacket = false)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            if (!IPCAccessor.Instance.IsConnected)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            logger.Debug("Client request ticket {0}", message.ticket);
            IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(new AccountRequestMessage { Ticket = message.ticket }, 
                msg => WorldServer.Instance.IOTaskPool.AddMessage(() => OnAccountReceived(msg, client)), error => client.Disconnect());
        }

        private static void OnAccountReceived(AccountAnswerMessage message, WorldClient client)
        {
            Character dummy;
            if (AccountManager.Instance.IsAccountBlocked(message.Account.Id, out dummy))
            {
                logger.Error("Account blocked, connection unallowed");
                client.Disconnect();
            }

            lock (ConnectionQueue.SyncRoot)
                ConnectionQueue.Remove(client);

            if (client.QueueShowed)
                SendQueueStatusMessage(client, 0, 0); // close the popup

            var ticketAccount = message.Account;

            /* Check null ticket */
            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            var clients = WorldServer.Instance.FindClients(x => x.Account != null && x.Account.Id == ticketAccount.Id).ToArray();
            clients.ForEach(x => x.Disconnect());

            /* Bind WorldAccount if exist */
            var account = AccountManager.Instance.FindById(ticketAccount.Id);
            if (account != null)
            {
                client.WorldAccount = account;

                if (client.WorldAccount.ConnectedCharacter != null)
                {
                    var character = World.Instance.GetCharacter(client.WorldAccount.ConnectedCharacter.Value);

                    if (character != null)
                        character.LogOut();
                }
            }

            /* Bind Account & Characters */
            client.SetCurrentAccount(ticketAccount);

            /* Ok */
            client.Send(new AuthenticationTicketAcceptedMessage());
            SendServerOptionalFeaturesMessage(client, OptionalFeaturesEnum.PvpArena);
            SendAccountCapabilitiesMessage(client);

            client.Send(new TrustStatusMessage(true)); // Restrict actions if account is not trust

            /* Just to get console AutoCompletion */
            if (client.UserGroup.IsGameMaster)
                SendConsoleCommandsListMessage(client, CommandManager.Instance.AvailableCommands.Where(x => client.UserGroup.IsCommandAvailable(x)));


        }
        public static void SendStartupActionsListMessage(IPacketReceiver client)
        {
            client.Send(new StartupActionsListMessage());
        }

        public static void SendServerOptionalFeaturesMessage(IPacketReceiver client, params OptionalFeaturesEnum[] features)
        {
            client.Send(new ServerOptionalFeaturesMessage(features.Select(x => (short)x)));
        }

        public static void SendAccountCapabilitiesMessage(WorldClient client)
        {
            client.Send(new AccountCapabilitiesMessage(
                            client.Account.Id,
                            false,
                            (short)client.Account.BreedFlags,
                            (short)BreedManager.Instance.AvailableBreedsFlags,
                            (sbyte) client.UserGroup.Role));
        }

        public static void SendConsoleCommandsListMessage(IPacketReceiver client, IEnumerable<CommandBase> commands)
        {
            var commandsInfos = (from command in commands
                                 let aliases = command.GetFullAliases() 
                                 let usage = command.GetSafeUsage() 
                                 from alias in aliases select Tuple.Create(alias, usage, command.Description ?? string.Empty)).ToList();

            client.Send(
                new ConsoleCommandsListMessage(
                    commandsInfos.Select(x => x.Item1),
                    commandsInfos.Select(x => x.Item2),
                    commandsInfos.Select(x => x.Item3)));
        }

        public static void SendQueueStatusMessage(IPacketReceiver client, ushort position, ushort total)
        {
            client.Send(new QueueStatusMessage(position, total));
        }
    }
}