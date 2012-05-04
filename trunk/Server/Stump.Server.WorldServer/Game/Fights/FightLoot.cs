using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Fights
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
        public void GiveLoot(Character character)
        {
            character.Inventory.AddKamas(Kamas);

            foreach (var drop in m_items.Values)
            {
                var template = ItemManager.Instance.TryGetTemplate(drop.ItemId);

                if (template.Effects.Count > 0)
                {
                    for (int i = 0; i < drop.Amount; i++)
                    {
                        var item = ItemManager.Instance.CreatePlayerItem(character, drop.ItemId, drop.Amount);
                        character.Inventory.AddItem(item);
                    }
                }
                else
                {
                    var item = ItemManager.Instance.CreatePlayerItem(character, drop.ItemId, drop.Amount);
                    character.Inventory.AddItem(item);
                }
            }
        }

        public DofusProtocol.Types.FightLoot GetFightLoot()
        {
            return new DofusProtocol.Types.FightLoot(m_items.Values.SelectMany(entry => new[] { entry.ItemId, (short)entry.Amount }), Kamas);
        }
    }
}