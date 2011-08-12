using System;

namespace Stump.Server.WorldServer.Worlds.Items
{
    public class ItemsStorage : ItemsCollection
    {
        public event Action<ItemsStorage, int> KamasAmountChanged;

        private void NotifyKamasAmountChanged(int kamas)
        {
            OnKamasAmountChanged(kamas);

            Action<ItemsStorage, int> handler = KamasAmountChanged;
            if (handler != null) handler(this, kamas);
        }

        protected virtual void OnKamasAmountChanged(int amount)
        {

        }

        public virtual void AddKamas(int amount)
        {
            SetKamas(Kamas + amount);
        }

        public virtual void SubKamas(int amount)
        {
            SetKamas(Kamas - amount);
        }

        public virtual void SetKamas(int amount)
        {
            Kamas = amount;

            NotifyKamasAmountChanged(amount);
        }

        public virtual int Kamas
        {
            get;
            protected set;
        }
    }
}