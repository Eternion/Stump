using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Network;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public partial class ConnectionHandler
    {

        [AuthHandler(AcquaintanceSearchMessage.Id)]
        public static void HandleAcquaintanceSearchMessage(AuthClient client, AcquaintanceSearchMessage message)
        {
            var account = Account.FindAccountByNickname(message.nickname);

            if (account == null)
            {
                SendAcquaintanceSearchErrorMessage(client, AcquaintanceErrorEnum.NO_RESULT);
                return;
            }

            SendAcquaintanceSearchServerListMessage(client, account.Characters.Select(wcr => (short)wcr.World.Id).Distinct());
        }

        public static void SendAcquaintanceSearchServerListMessage(AuthClient client, IEnumerable<short> serverIds)
        {
            client.Send(new AcquaintanceServerListMessage(serverIds));
        }

        public static void SendAcquaintanceSearchErrorMessage(AuthClient client, AcquaintanceErrorEnum reason)
        {
            client.Send(new AcquaintanceSearchErrorMessage((sbyte) reason));
        }

    }
}