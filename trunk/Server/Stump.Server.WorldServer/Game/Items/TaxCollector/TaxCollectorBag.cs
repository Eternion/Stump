using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items.TaxCollector
{
    public class TaxCollectorBag : ItemsCollection<TaxCollectorItem>
    {
        public TaxCollectorBag(TaxCollectorNpc owner)
        {
            Owner = owner;
        }

        public TaxCollectorNpc Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Must be saved 
        /// </summary>
        public bool IsDirty
        {
            get;
            private set;
        }

        protected override void OnItemStackChanged(TaxCollectorItem item, int difference)
        {
            IsDirty = true;

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnItemAdded(TaxCollectorItem item)
        {
            IsDirty = true;

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(TaxCollectorItem item)
        {
            IsDirty = true;

            if (Count == 0)
                Owner.Delete();

            base.OnItemRemoved(item);
        }

        public bool MoveToInventory(TaxCollectorItem item, Character character)
        {
            return MoveToInventory(item, character, item.Stack);
        }

        public bool MoveToInventory(TaxCollectorItem item, Character character, uint quantity)
        {
            if (quantity == 0)
                return false;

            if (quantity > item.Stack)
                quantity = item.Stack;

            RemoveItem(item, quantity);
            BasePlayerItem newItem = ItemManager.Instance.CreatePlayerItem(character, item.Template, quantity,
                                                                       item.Effects);

            character.Inventory.AddItem(newItem);

            return true;
        }

        public void LoadRecord()
        {
            var records = ItemManager.Instance.FindTaxCollectorItems(Owner.Id);
            Items = records.Select(entry => new TaxCollectorItem(entry)).ToDictionary(entry => entry.Guid);
        }

        public override void Save()
        {
            base.Save();

            IsDirty = false;
        }
    }
}
