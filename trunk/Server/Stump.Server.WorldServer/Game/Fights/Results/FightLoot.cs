using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Fights.Loots
{
    public class FightLoot
    {
        private readonly Dictionary<short, DroppedItem> m_items = new Dictionary<short, DroppedItem>();

        public IReadOnlyDictionary<short, DroppedItem> Items
        {
            get { return new ReadOnlyDictionary<short, DroppedItem>(m_items); }
        }

        public int Kamas
        {
            get;
            set;
        }

        public void AddItem(short itemId)
        {
            if (m_items.ContainsKey(itemId))
                m_items[itemId].Amount++;
            else
                m_items.Add(itemId, new DroppedItem(itemId, 1));
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
            return new DofusProtocol.Types.FightLoot(m_items.Values.SelectMany(entry => new[] { entry.ItemId, (short)entry.Amount }), Kamas);
        }

        public string FightItemsString()
        {
            return string.Join("|", m_items.Select(item => item.Value.ItemId + "_" + item.Value.Amount).ToList());
        }
    }
}