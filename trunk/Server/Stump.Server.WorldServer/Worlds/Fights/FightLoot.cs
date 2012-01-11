using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightLoot
    {
        private readonly Dictionary<short, DroppedItem> m_items = new Dictionary<short, DroppedItem>();

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

        // todo : give look to a inventory owner
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GiveLoot(Character character)
        {
            character.Inventory.AddKamas(Kamas);

            foreach (var item in m_items.Values)
            {
                character.Inventory.AddItem(item.ItemId, item.Amount);
            }
        }

        public DofusProtocol.Types.FightLoot GetFightLoot()
        {
            return new DofusProtocol.Types.FightLoot(m_items.Values.SelectMany(entry => new[] { entry.ItemId, (short)entry.Amount }), Kamas);
        }
    }
}