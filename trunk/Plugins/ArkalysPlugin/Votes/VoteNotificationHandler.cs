
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Game;

namespace ArkalysPlugin.Votes
{
    public static class VoteNotificationHandler
    {
        [Initialization(InitializationPass.Last, Silent=true)]
        public static void Initialize()
        {
            IPCAccessor.Instance.AddMessageHandler(typeof (VoteNotificationMessage), HandleMessage);
        }

        private static void HandleMessage(IPCMessage message)
        {
            var msg = (VoteNotificationMessage) message;

            foreach (var character in World.Instance.GetCharacters(x => msg.AccountsToNotify.Contains(x.Account.Id)))
            {
                character.DisplayNotification(
                    "Plus de 3H se sont écoulées depuis votre dernier vote, vous pouvez à nouveau voter pour gagner des jetons depuis le site www.arkalys.com/vote",
                    NotificationEnum.ERREUR);
            }
        }
    }
}