using System;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items
{
    public sealed class MerchantBag : ItemsCollection<MerchantItem>
    {
        public Character Owner
        {
            get;
            private set;
        }

        public MerchantBag(Character owner)
        {
            Owner = owner;
        }

        internal void LoadMerchantBag()
        {
            var records = ItemManager.Instance.FindPlayerMerchantItems(Owner.Id);
            Items = records.Select(entry => new MerchantItem(Owner, entry)).ToDictionary(entry => entry.Guid);
        }

        private void UnLoadMerchantBag()
        {
            Items.Clear();
        }

        public override void Save()
        {
            lock (Locker)
            {
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var item in Items)
                {
                    if (item.Value.Record.IsNew)
                    {
                        database.Insert(item.Value.Record);
                        item.Value.Record.IsNew = false;
                    }
                    else if (item.Value.Record.IsDirty)
                    {
                        database.Update(item.Value.Record);
                    }
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    database.Delete(item.Record);
                }
            }
        }

        public int CalcMerchantTax()
        {
            double resultTax = 0;

            foreach(var Item in Items)
            {
                resultTax += (Item.Value.Price * Item.Value.Stack);
            }

            resultTax = (resultTax * 0.1);

            return (int)resultTax;
        }

        public bool MoveToInventory(MerchantItem item)
        {
            RemoveItem(item);

            var cItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, (uint)item.Stack);

            Owner.Inventory.AddItem(cItem);

            return true;
        }

        public bool ModifyQuantity(MerchantItem item, int quantity)
        {
            if (quantity <= item.Stack)
            {
                int newQuantity = item.Stack - quantity;
                RemoveItem(item, (uint)newQuantity);

                var cItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, (uint)newQuantity);

                if (cItem != null)
                    Owner.Inventory.AddItem(cItem);

                return true;
            }

            return false;
        }
    }
}