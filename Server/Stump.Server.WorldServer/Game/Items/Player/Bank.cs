using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class Bank : ItemsStorage<BankItem>
    {
        //1K per default
        [Variable] private const int PricePerItem = 1;

        public Bank(Character character)
        {
            Owner = character;
            IsLoaded = false;
        }

        public void LoadRecord()
        {
            if (IsLoaded)
                return;

            WorldServer.Instance.IOTaskPool.EnsureContext();

            Items = WorldServer.Instance.DBAccessor.Database.Query<BankItemRecord>(string.Format(BankItemRelator.FetchByOwner,
                    Owner.Account.Id)).ToDictionary(x => x.Id, x => new BankItem(Owner, x));
            IsLoaded = true;
        }

        public bool IsLoaded
        {
            get;
            private set;
        }

        public Character Owner
        {
            get;
            private set;
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

            if (amount > item.Stack)
                amount = (int)item.Stack;

            var bankItem = ItemManager.Instance.CreateBankItem(Owner, item, amount);
            AddItem(bankItem);

            Owner.Inventory.RemoveItem(item, amount);

            return true;
        }

        public bool StoreKamas(int kamas)
        {
            if (kamas < 0)
                return false;

            if (Owner.Inventory.Kamas < kamas)
                kamas = Owner.Inventory.Kamas;

            AddKamas(kamas);
            Owner.Inventory.SetKamas(Owner.Inventory.Kamas - kamas);


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

            var playerItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, amount, new List<EffectBase>(item.Effects));
            Owner.Inventory.AddItem(playerItem);

            RemoveItem(item, amount);

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

        public int GetAccessPrice()
        {
            return (Items.Count * PricePerItem);
        }

        protected override void OnItemAdded(BankItem item, bool addItemMsg)
        {
            InventoryHandler.SendStorageObjectUpdateMessage(Owner.Client, item);

            base.OnItemAdded(item, false);
        }

        protected override void OnItemRemoved(BankItem item, bool removeItemMsg)
        {            
            InventoryHandler.SendStorageObjectRemoveMessage(Owner.Client, item);

            base.OnItemRemoved(item, removeItemMsg);
        }

        protected override void OnItemStackChanged(BankItem item, int difference, bool removeMsg = true)
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