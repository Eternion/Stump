
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Exchange
{
    public class TradeManager : InstanceManager<PlayerTrade>
    {
        public static int CreateTrade(PlayerTrade playerTrade)
        {
            return CreateInstance(playerTrade);
        }

        public static bool RemoveTrade(PlayerTrade playerTrade)
        {
            return RemoveInstance(playerTrade);
        }

        public static PlayerTrade GetTradeById(int id)
        {
            return GetInstanceById(id);
        }
    }
}