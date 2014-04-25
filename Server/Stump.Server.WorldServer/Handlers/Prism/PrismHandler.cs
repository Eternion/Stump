using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Prism
{
    public class PrismHandler : WorldHandlerContainer
    {
        [WorldHandler(PrismBalanceRequestMessage.Id)]
        public static void HandlePrismBalanceRequestMessage(WorldClient client, PrismBalanceRequestMessage message)
        {
            // todo

            client.Send(new PrismBalanceResultMessage(0, 0));
        }

        [WorldHandler(PrismCurrentBonusRequestMessage.Id)]
        public static void HandlePrismCurrentBonusRequestMessage(WorldClient client, PrismCurrentBonusRequestMessage message)
        {
            // todo

            client.Send(new PrismAlignmentBonusResultMessage(new AlignmentBonusInformations(0, 0)));
        }
    }
}