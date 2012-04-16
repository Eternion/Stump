using System;
using Stump.Server.WorldServer.Database.Items;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemsStorage<T> : ItemsCollection<T>
        where T : IItem
    {
        public event Action<ItemsStorage<T>, int> KamasAmountChanged;

        private void NotifyKamasAmountChanged(int kamas)
        {
            OnKamasAmountChanged(kamas);

            Action<ItemsStorage<T>, int> handler = KamasAmountChanged;
            if (handler != null) handler(this, kamas);
        }

        protected virtual void OnKamasAmountChanged(int amount)
        {

        }

        public void AddKamas(int amount)
        {
            SetKamas(Kamas + amount);
        }

        public void SubKamas(int amount)
        {
            SetKamas(Kamas - amount);
        }

        public void SetKamas(int amount)
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