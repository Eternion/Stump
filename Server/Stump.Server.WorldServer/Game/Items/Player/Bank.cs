using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class Bank : ItemsStorage<BankItem>
    {
        //1K per default
        [Variable]
        private const int PricePerItem = 1;

        public Bank(Character character)
        {
            Owner = character;
        }

        public void LoadRecord()
        {
            WorldServer.Instance.IOTaskPool.EnsureContext();

            Items = WorldServer.Instance.DBAccessor.Database.Query<BankItemRecord>(string.Format(BankItemRelator.FetchByOwner,
                    Owner.Account.Id)).ToDictionary(x => x.Id, x => new BankItem(Owner, x));
        }

        public Character Owner
        {
            get;
        }

        public override int Kamas
        {
            get { return Owner.Client.WorldAccount.BankKamas; }
            protected set { Owner.Client.WorldAccount.BankKamas = value; }
        }

        public bool StoreItem(BasePlayerItem item, int amount)
        {
            if (!Owner.Inventory.HasItem(item) || amount <= 0)
                return false;

            if (item.IsLinkedToPlayer())
                return false;

            if (item.IsEquiped())
                return false;

            if (amount > item.Stack)
                amount = (int)item.Stack;

            Owner.Inventory.RemoveItem(item, amount);

            var bankItem = ItemManager.Instance.CreateBankItem(Owner, item, amount);
            AddItem(bankItem);

            return true;
        }

        public bool StoreItems(IEnumerable<int> guids, bool all, bool existing)
        {
            foreach (var item in Owner.Inventory.Where(x => guids.Contains(x.Guid) || (existing && Items.Values.Any(y => y.Template.Id == x.Template.Id)) || all).ToArray())
                StoreItem(item, (int)item.Stack);

            //TODO: Avoid client lag
            //InventoryHandler.SendObjectsDeletedMessage(Owner.Client, guids);

            return true;
        }

        public bool StoreKamas(int kamas)
        {
            if (kamas < 0)
                return false;

            if (Owner.Inventory.Kamas < kamas)
                kamas = Owner.Inventory.Kamas;

            Owner.Inventory.SetKamas(Owner.Inventory.Kamas - kamas);
            AddKamas(kamas);

            return true;
        }

        public bool TakeItemBack(BankItem item, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (item == null)
                return false;

            if (!HasItem(item))
                return false;

            if (amount > item.Stack)
                amount = (int)item.Stack;

            RemoveItem(item, amount);

            var playerItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, amount, new List<EffectBase>(item.Effects));
            Owner.Inventory.AddItem(playerItem);

            return true;
        }

        public bool TakeItemsBack(IEnumerable<int> guids, bool all, bool existing)
        {
            foreach (var item in Items.Values.Where(x => guids.Contains(x.Guid) || (existing && Owner.Inventory.Any(y => y.Template.Id == x.Template.Id)) || all).ToArray())
                TakeItemBack(item, (int)item.Stack);

            //TODO: Avoid client lag
            //InventoryHandler.SendObjectsDeletedMessage(Owner.Client, guids);

            return true;
        }

        public bool TakeKamas(int kamas)
        {
            if (kamas < 0)
                return false;

            if (kamas > Kamas)
                kamas = Kamas;

            SubKamas(kamas);
            Owner.Inventory.AddKamas(kamas);

            return true;
        }

        public int GetAccessPrice() => (Items.Count * PricePerItem);

        protected override void OnItemAdded(BankItem item)
        {
            InventoryHandler.SendStorageObjectUpdateMessage(Owner.Client, item);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(BankItem item)
        {            
            InventoryHandler.SendStorageObjectRemoveMessage(Owner.Client, item);

            base.OnItemRemoved(item);
        }

        protected override void OnItemStackChanged(BankItem item, int difference)
        {            
            InventoryHandler.SendStorageObjectUpdateMessage(Owner.Client, item);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendStorageKamasUpdateMessage(Owner.Client, Kamas);

            base.OnKamasAmountChanged(amount);
        }
    }
}