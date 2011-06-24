
using System.Linq;
using System.Collections.Generic;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.AuthServer.Handlers
{
    public partial class ConnectionHandler
    {

        [AuthHandler(typeof(AcquaintanceSearchMessage))]
        public static void HandleAcquaintanceSearchMessage(AuthClient client, AcquaintanceSearchMessage message)
        {
            var account = AccountRecord.FindAccountByNickname(message.nickname);

            if (account == null)
            {
                SendAcquaintanceSearchErrorMessage(client, AcquaintanceErrorEnum.NO_RESULT);
                return;
            }

            SendAcquaintanceSearchServerListMessage(client, account.Characters.Select(wcr => wcr.World.Id).Distinct().ToList());
        }

        public static void SendAcquaintanceSearchServerListMessage(AuthClient client, List<int> serverIds)
        {
            client.Send(new AcquaintanceServerListMessage(serverIds));
        }

        public static void SendAcquaintanceSearchErrorMessage(AuthClient client, AcquaintanceErrorEnum reason)
        {
            client.Send(new AcquaintanceSearchErrorMessage((uint)reason));
        }

    }
}