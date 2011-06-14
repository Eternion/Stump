
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        private InventoryHandler()
        {
            Predicates = new Dictionary<Type, Predicate<WorldClient>>
                         {
                             {typeof (ExchangeObjectMoveKamaMessage), PredicatesDefinitions.IsTrading},
                             {typeof (ExchangeObjectMoveMessage), PredicatesDefinitions.IsTrading},
                             {typeof (ExchangeReadyMessage), PredicatesDefinitions.IsTrading},
                         };
        }

        public static void SendKamasUpdateMessage(WorldClient client, long kamasAmount)
        {
            client.Send(new KamasUpdateMessage((int) kamasAmount));
        }
    }
}