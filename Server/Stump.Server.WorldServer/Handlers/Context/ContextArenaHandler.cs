using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Arena;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        [WorldHandler(GameRolePlayArenaFightAnswerMessage.Id)]
        public static void HandleGameRolePlayArenaFightAnswerMessage(WorldClient client, GameRolePlayArenaFightAnswerMessage message)
        {
            var popup = client.Character.ArenaPopup;

            if (popup == null)
                return;

            if (message.accept)
                popup.Accept();
            else
                popup.Deny();
        }

        [WorldHandler(GameRolePlayArenaRegisterMessage.Id)]
        public static void HandleGameRolePlayArenaRegisterMessage(WorldClient client, GameRolePlayArenaRegisterMessage message)
        {
            // todo error messages
            // 3VS3 only ?
            if (client.Character.ArenaParty != null)
            {
                if (client.Character.IsPartyLeader(client.Character.ArenaParty.Id))
                    ArenaManager.Instance.AddToQueue(client.Character.ArenaParty);
            }
            else
                ArenaManager.Instance.AddToQueue(client.Character);
        }

        [WorldHandler(GameRolePlayArenaUnregisterMessage.Id)]
        public static void HandleGameRolePlayArenaUnregisterMessage(WorldClient client, GameRolePlayArenaUnregisterMessage message)
        {
            ArenaManager.Instance.RemoveFromQueue(client.Character);
        }   
        
        public static void SendGameRolePlayArenaFightPropositionMessage(IPacketReceiver client, ArenaPopup popup, int delay)
        {
            client.Send(new GameRolePlayArenaFightPropositionMessage(popup.Team.Fight.Id, popup.Team.GetAlliesInQueue().Select(x => x.Id), (short)delay));
        }

        public static void SendGameRolePlayArenaFighterStatusMessage(IPacketReceiver client)
        {
            client.Send(new GameRolePlayArenaFighterStatusMessage());
        }

        public static void SendGameRolePlayArenaRegistrationStatusMessage(IPacketReceiver client, bool registred, PvpArenaStepEnum step, PvpArenaTypeEnum type)
        {
            client.Send(new GameRolePlayArenaRegistrationStatusMessage(registred, (sbyte)step, (sbyte)type));
        }
    }
}