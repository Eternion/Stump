using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightLoot
    {
        private readonly Dictionary<int, DroppedItem> m_items = new Dictionary<int, DroppedItem>();

        public FightLoot()
        {
            
        }
        
        public IReadOnlyDictionary<int, DroppedItem> Items
        {
            get { return new ReadOnlyDictionary<int, DroppedItem>(m_items); }
        }

        public int Kamas
        {
            get;
            set;
        }

        public void AddItem(int itemId)
        {
            AddItem(itemId, 1);
        }

        public void AddItem(int itemId, uint amount)
        {
            if (m_items.ContainsKey(itemId))
                m_items[itemId].Amount++;
            else
                m_items.Add(itemId, new DroppedItem(itemId, amount));
        }

        public void AddItem(DroppedItem item)
        {
            if (m_items.ContainsKey(item.ItemId))
                m_items[item.ItemId].Amount += item.Amount;
            else
                m_items.Add(item.ItemId, new DroppedItem(item.ItemId, item.Amount));
        }

        public DofusProtocol.Types.FightLoot GetFightLoot()
        {
            return new DofusProtocol.Types.FightLoot(m_items.Values.SelectMany(entry => new[] { (short)entry.ItemId, (short)entry.Amount }), Kamas);
        }

        public string FightItemsString()
        {
            return string.Join("|", m_items.Select(item => item.Value.ItemId + "_" + item.Value.Amount).ToList());
        }
    }
}