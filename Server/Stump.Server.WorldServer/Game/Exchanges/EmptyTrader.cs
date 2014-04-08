using Stump.Server.WorldServer.Game.Actors.RolePlay;

namespace Stump.Server.WorldServer.Game.Exchanges
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
    }
}