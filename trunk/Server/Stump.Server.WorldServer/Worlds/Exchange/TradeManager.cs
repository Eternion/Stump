using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;

namespace Stump.Server.WorldServer.Worlds.Exchange
{
    public class TradeManager : Singleton<TradeManager>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();
        private readonly Dictionary<int, PlayerTrade> m_trades = new Dictionary<int, PlayerTrade>();

        public PlayerTrade Create()
        {
            var group = new PlayerTrade(m_idProvider.Pop());

            m_trades.Add(group.Id, group);

            return group;
        }

        public void Remove(PlayerTrade party)
        {
            m_trades.Remove(party.Id);

            m_idProvider.Push(party.Id);
        }

        public PlayerTrade GetTrade(int id)
        {
            return m_trades.ContainsKey(id) ? m_trades[id] : null;
        }
    }
}