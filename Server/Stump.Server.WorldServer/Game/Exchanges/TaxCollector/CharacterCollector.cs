using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.TaxCollector;
using System;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Exchanges.TaxCollector
{
    public class CharacterCollector : Exchanger
    {
        public CharacterCollector(TaxCollectorNpc taxCollector, Character character, TaxCollectorExchange taxCollectorTrade)
            : base(taxCollectorTrade)
        {
            TaxCollector = taxCollector;
            Character = character;
            RecoltedItems = new Dictionary<int, TaxCollectorItem>();
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
        }

        public Character Character
        {
            get;
        }

        public Dictionary<int, TaxCollectorItem> RecoltedItems
        {
            get;
            private set;
        }

        public override bool MoveItem(int id, int quantity)
        {
            if (TaxCollector.IsFighting || !TaxCollector.IsInWorld)
                return false;

            if (quantity >= 0)
            {
                Character.SendSystemMessage(7, false); // Action invalide
                return false;
            }

            quantity = Math.Abs(quantity);

            var taxCollectorItem = TaxCollector.Bag.TryGetItem(id);
            if (taxCollectorItem == null)
                return false;

            if (TaxCollector.Bag.MoveToInventory(taxCollectorItem, Character, quantity))
            {
                if (RecoltedItems.ContainsKey(taxCollectorItem.Guid))
                    RecoltedItems[taxCollectorItem.Guid].Stack += (uint)quantity;
                else
                    RecoltedItems.Add(taxCollectorItem.Guid, ItemManager.Instance.CreateTaxCollectorItem(TaxCollector, taxCollectorItem.Template, quantity));

                if (TaxCollector.Bag.HasItem(taxCollectorItem))
                    Character.Client.Send(new StorageObjectUpdateMessage(taxCollectorItem.GetObjectItem()));
                else
                    Character.Client.Send(new StorageObjectRemoveMessage(id));
            }

            return true;
        }

        public override bool SetKamas(int amount)
        {
            if (TaxCollector.IsFighting || !TaxCollector.IsInWorld)
                return false;

            if (amount > 0)
                return false;

            amount = Math.Abs(amount);

            if (TaxCollector.GatheredKamas <= 0)
                amount = 0;

            if (TaxCollector.GatheredKamas < amount)
                amount = TaxCollector.GatheredKamas;

            TaxCollector.GatheredKamas -= amount;
            Character.Inventory.AddKamas(amount);
            Character.Client.Send(new StorageKamasUpdateMessage(TaxCollector.GatheredKamas));

            if (TaxCollector.Bag.Count == 0 && TaxCollector.GatheredKamas == 0)
                TaxCollector.Delete();

            return true;
        }
    }
}