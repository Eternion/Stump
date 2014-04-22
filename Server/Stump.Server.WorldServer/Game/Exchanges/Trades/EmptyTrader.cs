namespace Stump.Server.WorldServer.Game.Exchanges.Trades
{
    public class EmptyTrader : Trader
    {
        private int m_id;

        public EmptyTrader(int id, ITrade trade)
            : base(trade)
        {
            m_id = id;
        }

        public override int Id
        {
            get { return m_id; }
        }

        public override bool MoveItem(int id, int quantity)
        {
            return false;
        }
    }
}