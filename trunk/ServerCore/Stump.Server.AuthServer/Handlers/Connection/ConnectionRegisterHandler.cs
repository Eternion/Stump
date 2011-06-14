
using System.Text.RegularExpressions;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.AuthServer.Handlers
{
    public partial class ConnectionHandler
    {

        [AuthHandler(typeof(NicknameChoiceRequestMessage))]
        public static void HandleNicknameChoiceRequestMessage(AuthClient client, NicknameChoiceRequestMessage message)
        {
            string nickname = message.nickname;

            /* Check the Username */
            if (!CheckNickName(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.INVALID_NICK));
                return;
            }

            /* Same as Login */
            if (nickname == client.Account.Login)
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.SAME_AS_LOGIN));
                return;
            }

            /* Look like Login */
            if (client.Account.Login.Contains(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.TOO_SIMILAR_TO_LOGIN));
                return;
            }

            /* Already Used */
            if (AccountRecord.NicknameExist(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.ALREADY_USED));
                return;
            }

            /* Ok, it's good */
            client.Account.Nickname = nickname;
            client.Save();

            client.Send(new NicknameAcceptedMessage());
            SendIdentificationSuccessMessage(client, false);
            SendServersListMessage(client);
        }

        public static bool CheckNickName(string nickName)
        {
            return Regex.IsMatch(nickName, @"^[a-zA-Z\-]{3,29}$", RegexOptions.Compiled);
        }

    }
}