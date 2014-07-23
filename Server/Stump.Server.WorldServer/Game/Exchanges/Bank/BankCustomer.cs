using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Exchanges.Bank
{
    public class BankCustomer : Exchanger
    {
        public BankCustomer(Character character, BankDialog dialog)
            : base(dialog)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override bool MoveItem(int id, int quantity)
        {
            if (quantity > 0)
            {
                var item = Character.Inventory.TryGetItem(id);

                return item != null && Character.Bank.StoreItem(item, quantity);
            }

            if (quantity >= 0)
                return false;

            var deleteItem = Character.Bank.TryGetItem(id);

            return Character.Bank.TakeItemBack(deleteItem, -quantity);
        }

        public override bool SetKamas(int amount)
        {            
            if (amount > 0)
            {
                return Character.Bank.StoreKamas(amount);
            }

            return amount < 0 && Character.Bank.TakeKamas(-amount);
        }
    }
}