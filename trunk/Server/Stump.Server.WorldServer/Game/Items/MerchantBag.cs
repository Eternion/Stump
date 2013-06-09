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

        public void MoveItemToInventory(MerchantItem Item)
        {
            //Todo
        }
    }
}