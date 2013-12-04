using System;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemsStorage<T> : ItemsCollection<T>
        where T : IItem
    {
        [Variable(true)]
        private const int MaxInventoryKamas = 150000000;

        public event Action<ItemsStorage<T>, int> KamasAmountChanged;

        private void NotifyKamasAmountChanged(int kamas)
        {
            OnKamasAmountChanged(kamas);

            var handler = KamasAmountChanged;
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
            if (amount >= MaxInventoryKamas)
                Kamas = MaxInventoryKamas;
                //Todo: TEXT_INFORMATION_MESSAGE		344		Vous avez atteint le seuil maximum de kamas dans votre inventaire.

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