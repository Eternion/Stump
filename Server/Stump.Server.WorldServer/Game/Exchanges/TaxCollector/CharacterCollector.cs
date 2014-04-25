using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;

namespace Stump.Server.WorldServer.Game.Exchanges.TaxCollector
{
    public class CharacterCollector : Exchanger
    {
        public CharacterCollector(TaxCollectorNpc taxCollector, Character character, TaxCollectorExchange taxCollectorTrade)
            : base(taxCollectorTrade)
        {
            TaxCollector = taxCollector;
            Character = character;
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override bool MoveItem(int id, int quantity)
        {
            if (quantity >= 0)
            {
                Character.SendSystemMessage(7, false); // Action invalide
                return false;
            }

            quantity = -quantity;

            var taxCollectorItem = TaxCollector.Bag.TryGetItem(id);
            if (taxCollectorItem == null)
                return false;

            if (TaxCollector.Bag.MoveToInventory(taxCollectorItem, Character, (uint)quantity))
                Character.Client.Send(new StorageObjectRemoveMessage(id));

            return true;
        }

        public override bool SetKamas(int amount)
        {
            if (amount < 0)
                return false;

            if (TaxCollector.GatheredKamas <= 0)
                amount = 0;

            if (TaxCollector.GatheredKamas < amount)
                amount = TaxCollector.GatheredKamas;

            TaxCollector.GatheredKamas -= amount;
            Character.Inventory.AddKamas(amount);
            Character.Client.Send(new StorageKamasUpdateMessage(TaxCollector.GatheredKamas));

            return true;
        }
    }
}