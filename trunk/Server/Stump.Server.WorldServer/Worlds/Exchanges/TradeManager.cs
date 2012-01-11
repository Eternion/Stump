using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;

namespace Stump.Server.WorldServer.Worlds.Exchanges
{
    public class TradeManager : EntityManager<TradeManager, PlayerTrade>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public PlayerTrade Create()
        {
            var trade = new PlayerTrade(m_idProvider.Pop());

            AddEntity(trade.Id, trade);

            return trade;
        }

        public void Remove(PlayerTrade trade)
        {
            RemoveEntity(trade.Id);

            m_idProvider.Push(trade.Id);
        }

        public PlayerTrade GetTrade(int id)
        {
            return GetEntityOrDefault(id);
        }
    }
}