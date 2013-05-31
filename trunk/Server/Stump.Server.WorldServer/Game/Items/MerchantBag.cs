using System;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items
{
    public class MerchantBag : ItemsCollection<MerchantItem>
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