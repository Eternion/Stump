namespace Stump.Server.WorldServer.Game.Exchanges.Trades
{
    public interface ITrade : IExchange
    {
        Trader FirstTrader
        {
            get;
        }

        Trader SecondTrader
        {
            get;
        }
    }
}