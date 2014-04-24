using System.Linq;
using Stump.DofusProtocol.Messages;
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

                if (item == null)
                    return false;

                return Character.Bank.StoreItem(item, (uint)quantity);
            }
            else if (quantity < 0)
            {
                var item = Character.Bank.TryGetItem(id);
               
                return Character.Bank.TakeItemBack(item, (uint)-quantity);
            }

            return false;
        }

        public override bool SetKamas(int amount)
        {            
            if (amount > 0)
            {
                return Character.Bank.StoreKamas(amount);
            }
            else if (amount < 0)
            {
                return Character.Bank.TakeKamas(-amount);
            }
            return false;
        }
    }
}