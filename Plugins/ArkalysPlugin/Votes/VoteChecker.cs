using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using System;
using System.Linq;

namespace ArkalysPlugin.Votes
{
    public static class VoteChecker
    {
        [Variable]
        static int VoteTimer = 30;

        [Initialization(InitializationPass.Last, Silent = true)]
        public static void Initialize()
        {
            WorldServer.Instance.IOTaskPool.CallPeriodically((VoteTimer * 1000), CheckVotes);
        }

        static void CheckVotes()
        {
            foreach (var character in World.Instance.GetCharacters(character => character.UserGroup.Role == RoleEnum.Player && (character.Account.LastVote == null || character.Account.LastVote < (DateTime.Now - TimeSpan.FromHours(3)))))
            {
                if (World.Instance.GetCharacters(x => x != character && x.Account.LastClientKey == character.Account.LastClientKey
                        && x.Account.LastVote >= (DateTime.Now - TimeSpan.FromHours(3))).Any())
                    continue;

                IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(new AccountRequestMessage { Id = character.Account.Id },
                    msg => WorldServer.Instance.IOTaskPool.AddMessage(() => OnAccountReceived(msg, character.Client)));

                character.DisplayNotification(
                    "Plus de 3H se sont écoulées depuis votre dernier vote, vous pouvez à nouveau voter pour gagner des jetons en cliquant <u><b><a href='http://www.arkalys.com/vote' target='_blank'><font color='#0000FF'>ICI</font></a></b></u>",
                    NotificationEnum.ERREUR);
            }
        }

        static void OnAccountReceived(AccountAnswerMessage message, WorldClient client)
        {
            client.Account.LastVote = message.Account.LastVote;
        }
    }
}
