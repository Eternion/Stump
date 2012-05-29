using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Startup;

namespace Stump.Server.WorldServer.Handlers.Approach
{
    public class ApproachHandler : WorldHandlerContainer
    {
        [WorldHandler(AuthenticationTicketMessage.Id, RequiresLogin = false, IsGamePacket = false)]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            if (!IpcAccessor.Instance.IsConnected)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Get Ticket */
            AccountData ticketAccount = AccountManager.Instance.GetAccountByTicket(message.ticket);

            /* Check null ticket */
            if (ticketAccount == null)
            {
                client.Send(new AuthenticationTicketRefusedMessage());
                client.DisconnectLater(1000);
                return;
            }

            /* Bind WorldAccount if exist */
            if (WorldAccount.Exists(ticketAccount.Id))
            {
                client.WorldAccount = WorldAccount.FindById(ticketAccount.Id);

                if (client.WorldAccount.ConnectedCharacterId != null)
                {
                    var character = World.Instance.GetCharacter(client.WorldAccount.ConnectedCharacterId.Value);

                    if (character != null)
                        character.LogOut();
                }
            }

            /* Bind Account & Characters */
            client.Account = ticketAccount;
            client.Characters = CharacterManager.Instance.GetCharactersByAccount(client);

            /* Ok */
            client.Send(new AuthenticationTicketAcceptedMessage());
            SendServerOptionalFeaturesMessage(client, new short[0]);
            SendAccountCapabilitiesMessage(client);

            client.Send(new TrustStatusMessage(true)); // usage -> ?

            /* Just to get console AutoCompletion */
            if (client.Account.Role >= RoleEnum.Moderator)
                SendConsoleCommandsListMessage(client);
        }

        public static void SendStartupActionsListMessage(IPacketReceiver client)
        {
            client.Send(new StartupActionsListMessage());
        }

        public static void SendServerOptionalFeaturesMessage(IPacketReceiver client, IEnumerable<short> features)
        {
            client.Send(new ServerOptionalFeaturesMessage(features));
        }

        public static void SendAccountCapabilitiesMessage(WorldClient client)
        {
            client.Send(new AccountCapabilitiesMessage(
                            (int)client.Account.Id,
                            false,
                            (short)client.Account.BreedFlags,
                            (short)BreedManager.Instance.AvailableBreedsFlags,
                            (sbyte) client.Account.Role));
        }

        public static void SendConsoleCommandsListMessage(IPacketReceiver client)
        {
            client.Send(
                new ConsoleCommandsListMessage(
                    WorldServer.Instance.CommandManager.AvailableCommands.SelectMany(c => c.Aliases),
                    new string[0],  new string[0]));
        }
    }
}