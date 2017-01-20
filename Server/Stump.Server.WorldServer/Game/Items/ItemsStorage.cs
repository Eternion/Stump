using System;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemsStorage<T> : PersistantItemsCollection<T>
        where T : IPersistantItem
    {
        public event Action<ItemsStorage<T>, int> KamasAmountChanged;

        private void NotifyKamasAmountChanged(int kamas)
        {
            OnKamasAmountChanged(kamas);
            KamasAmountChanged?.Invoke(this, kamas);
        }

        protected virtual void OnKamasAmountChanged(int amount)
        {
        }

        public int AddKamas(int amount)
        {
            if (amount == 0)
                return 0;

            return SetKamas(Kamas + amount);
        }

        public int SubKamas(int amount)
        {
            if (amount == 0)
                return 0;

            return SetKamas(Kamas - amount);
        }

        public virtual int SetKamas(int amount)
        {
            var oldKamas = Kamas;

            if (amount < 0)
                amount = 0;

            Kamas = amount;
            NotifyKamasAmountChanged(amount - oldKamas);
            return amount - oldKamas;
        }

        public virtual int Kamas
        {
            get;
            protected set;
        }
    }
}