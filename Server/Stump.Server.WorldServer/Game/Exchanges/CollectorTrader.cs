using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items.TaxCollector;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class CollectorTrader : Trader
    {
        public CollectorTrader(TaxCollectorNpc taxCollector, Character character, TaxCollectorTrade taxCollectorTrade)
            : base(taxCollectorTrade)
        {
            TaxCollector = taxCollector;
            Character = character;
        }

        public override int Id
        {
            get { return Character.Id; }
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

        public override bool SetKamas(uint amount)
        {
            if (TaxCollector.GatheredKamas <= 0)
                amount = 0;

            if (TaxCollector.GatheredKamas < amount)
                amount = (uint)TaxCollector.GatheredKamas;

            TaxCollector.GatheredKamas -= (int)amount;
            Character.Inventory.AddKamas((int)amount);
            Character.Client.Send(new StorageKamasUpdateMessage(TaxCollector.GatheredKamas));

            return true;
        }
    }
}