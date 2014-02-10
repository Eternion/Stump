using System.Linq;
using Stump.Core.Pool;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Exchanges.Items;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class TaxCollectorTrader : Trader
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public TaxCollectorTrader(TaxCollectorNpc taxCollector, ITrade trade) : base(trade)
        {
            TaxCollector = taxCollector;
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return TaxCollector.Id; }
        }

        public void AddItem(ItemTemplate template, uint amount)
        {
            var item = Items.FirstOrDefault(x => x.Template == template);

            if (item != null)
            {
                item.Stack += amount;
                OnItemMoved(item, true, (int)amount);
            }
            else
            {
                item = new NpcTradeItem(m_idProvider.Pop(), template, amount);
                AddItem(item);
                OnItemMoved(item, false, (int)amount);
            }
        }

        public bool RemoveItem(ItemTemplate template, uint amount)
        {
            var item = Items.FirstOrDefault(x => x.Template == template);

            if (item == null)
                return false;

            var amountRemoved = amount;
            if (item.Stack - amount <= 0)
            {
                RemoveItem(item);
                amountRemoved = item.Stack;
            }
            else
            {
                item.Stack -= amount;
            }

            OnItemMoved(item, item.Stack > 0, -(int)amountRemoved);
            return true;
        }
    }
}
