using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Guilds;

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

        public static void SendTaxCollectorListMessage(IPacketReceiver client, Guild guild)
        {
            client.Send(new TaxCollectorListMessage(guild.MaxTaxCollectors, guild.HireCost, guild.TaxCollectors.Select(x => x.GetNetworkTaxCollector()), new TaxCollectorFightersInformation[0]));
        }
    }
}
