using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
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
using System.Threading;

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

            message.ticket = Encoding.ASCII.GetString(message.ticket.Split(',').Select(x => (byte)int.Parse(x)).ToArray());

            logger.Debug("Client request ticket {0}", message.ticket);

            IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(new AccountRequestMessage { Ticket = message.ticket }, 
                msg => WorldServer.Instance.IOTaskPool.AddMessage(() => OnAccountReceived(msg, client)), error => client.Disconnect());
        }

        [WorldHandler(ReloginTokenRequestMessage.Id, IsGamePacket = false)]
        public static void HandleReloginTokenRequestMessage(WorldClient client, ReloginTokenRequestMessage message)
        {
            //Need to keep ticket or regen one ta validate Token
            client.Send(new ReloginTokenStatusMessage(false, Encoding.ASCII.GetBytes(client.Account.Ticket).Select(x => (sbyte)x)));
        }

        /*[WorldHandler(HaapiApiKeyRequestMessage.Id)]
        public static void HandleHaapiApiKeyRequestMessage(WorldClient client, HaapiApiKeyRequestMessage message)
        {
            client.Send(new HaapiApiKeyMessage(message.keyType, client.Account.Ticket));
        }*/

        [WorldHandler(KrosmasterAuthTokenRequestMessage.Id)]
        public static void HandleKrosmasterAuthTokenRequestMessage(WorldClient client, KrosmasterAuthTokenRequestMessage message)
        {
            client.Send(new KrosmasterAuthTokenMessage(client.Account.Ticket));
        }

        static void OnAccountReceived(AccountAnswerMessage message, WorldClient client)
        {
            if (AccountManager.Instance.IsAccountBlocked(message.Account.Id, out Character dummy))
            {
                logger.Error($"{client} - Account({message.Account.Id}) blocked, connection unallowed");
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

            // not an expected situation
            if (clients.Length > 0)
            {
                client.Disconnect();
                return;
            }

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
            SendServerSessionConstantsMessage(client,
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.TIME_BEFORE_DISCONNECTION, BaseServer.Settings.InactivityDisconnectionTime*1000 ?? -1),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.KOH_DURATION, 7200000),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.KOH_WINNING_SCORE, 30),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.MINIMAL_TIME_BEFORE_KOH, 86400000),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.TIME_BEFORE_WEIGH_IN_KOH, 60000),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.UNKOWN_6, 10),
                new ServerSessionConstantInteger((short) ServerConstantTypeEnum.UNKNOW_7, 2000));
            SendAccountCapabilitiesMessage(client);

            client.Send(new TrustStatusMessage(true, true)); // Restrict actions if account is not trust

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
            client.Send(new ServerOptionalFeaturesMessage(features.Select(x => (sbyte)x)));
        }

        public static void SendServerSessionConstantsMessage(IPacketReceiver client, params ServerSessionConstant[] constants)
        {
            client.Send(new ServerSessionConstantsMessage(constants));
        }

        public static void SendAccountCapabilitiesMessage(WorldClient client)
        {
            client.Send(new AccountCapabilitiesMessage(false, true,
                            client.Account.Id,
                            (int)BreedManager.Instance.AvailableBreedsFlags,
                            (int)BreedManager.Instance.AvailableBreedsFlags,
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