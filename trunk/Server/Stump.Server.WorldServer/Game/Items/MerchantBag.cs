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

        public void MoveItemToInventory(MerchantItem Item)
        {
            //Todo
        }
    }
}