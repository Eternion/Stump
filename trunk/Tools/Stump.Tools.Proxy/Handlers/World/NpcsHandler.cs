
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class NpcsHandler : WorldHandlerContainer
    {
        [WorldHandler(NpcDialogQuestionMessage.Id)]
        public static void HandleNpcDialogQuestionMessage(WorldClient client, NpcDialogQuestionMessage message)
        {
            client.Send(message);

            DataFactory.HandleNpcQuestion(client, message);
        }

        [WorldHandler(NpcDialogReplyMessage.Id)]
        public static void HandleNpcDialogReplyMessage(WorldClient client, NpcDialogReplyMessage message)
        {
            client.GuessNpcReply = message;

            client.Server.Send(message);
        }

        [WorldHandler(LeaveDialogMessage.Id)]
        public static void HandleLeaveDialogMessage(WorldClient client, LeaveDialogMessage message)
        {
            client.Send(message);

            DataFactory.BuildActionNpcLeave(client, message);
        }

        [WorldHandler(LeaveDialogRequestMessage.Id)]
        public static void HandleLeaveDialogRequestMessage(WorldClient client, LeaveDialogRequestMessage message)
        {
            client.GuessNpcReply = null;

            client.Server.Send(message);
        }

        [WorldHandler(NpcGenericActionRequestMessage.Id)]
        public static void HandleNpcGenericActionRequestMessage(WorldClient client, NpcGenericActionRequestMessage message)
        {
            client.GuessNpcFirstAction = message;

            client.Server.Send(message);
        }


        [WorldHandler(ExchangeStartOkNpcShopMessage.Id)]
        public static void HandleExchangeStartOkNpcShopMessage(WorldClient client, ExchangeStartOkNpcShopMessage message)
        {
            client.Send(message);

            DataFactory.BuildActionNpcShop(client, message);
        }
    }
}