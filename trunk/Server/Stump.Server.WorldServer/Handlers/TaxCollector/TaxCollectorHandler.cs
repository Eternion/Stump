using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;

namespace Stump.Server.WorldServer.Handlers.TaxCollector
{
    public class TaxCollectorHandler : WorldHandlerContainer
    {
        [WorldHandler(TaxCollectorHireRequestMessage.Id)]
        public static void HandleTaxCollectorHireRequestMessage(WorldClient client, TaxCollectorHireRequestMessage message)
        {
            if (client.Character.Guild == null)
                return;

            TaxCollectorManager.Instance.AddTaxCollectorSpawn(client.Character);
        }

        public static void SendTaxCollectorListMessage(WorldClient client)
        {
            client.Send(new TaxCollectorListMessage(client.Character.Guild.MaxTaxCollectors, client.Character.Guild.HireCost, client.Character.Guild.TaxCollectors.Select(x => x.GetNetworkTaxCollector()), new TaxCollectorFightersInformation[0]));
        }
    }
}
