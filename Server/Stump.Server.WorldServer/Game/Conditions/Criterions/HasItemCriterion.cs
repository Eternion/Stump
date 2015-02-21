using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class HasItemCriterion : Criterion
    {
        public const string Identifier = "PO";

        public int Item
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            if (Operator == ComparaisonOperatorEnum.EQUALS)
                return character.Inventory.Any(entry => entry.Template.Id == Item);

             return Operator == ComparaisonOperatorEnum.INEQUALS && !character.Inventory.Any(entry => entry.Template.Id == Item && entry.IsEquiped());
        }

        public override void Build()
        {
            int itemId;

            if (!int.TryParse(Literal, out itemId))
                throw new Exception(string.Format("Cannot build HasItemCriterion, {0} is not a valid item id", Literal));

            Item = itemId;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }

    public class DeleteItemCriterion : Criterion
    {
        public const string Identifier = "PO2";

        public Dictionary<int, int> Items
        {
            get;
            set;
        }

        public override bool Eval(Character character)
        {
            if (Operator != ComparaisonOperatorEnum.EQUALS)
                return false;

            var itemsToDelete = new Dictionary<BasePlayerItem, int>();

            foreach (var requireItem in Items)
            {
                var itemId = requireItem.Key;
                var amount = requireItem.Value;

                var template = ItemManager.Instance.TryGetTemplate(itemId);
                if (template == null)
                    return false;

                var item = character.Inventory.TryGetItem(template);

                if (item.Stack < amount)
                    return false;

                itemsToDelete.Add(item, amount);   
            }

            foreach (var itemToDelete in itemsToDelete)
            {
                character.Inventory.RemoveItem(itemToDelete.Key, itemToDelete.Value);
            }

            return true;
        }

        public override void Build()
        {
            var itemsList = Literal.Split('|');

            foreach (var itemCut in itemsList.Select(item => item.Split('_')))
            {
                int itemId;
                int amount;

                if (!int.TryParse(itemCut[0], out itemId))
                    throw new Exception(string.Format("Cannot build HasItemCriterion, {0} is not a valid item id", itemCut[0]));

                if (!int.TryParse(itemCut[1], out amount))
                    throw new Exception(string.Format("Cannot build HasItemCriterion, itemId {0} has not a valid item amount {1}", itemCut[0], itemCut[1]));

                Items.Add(itemId, amount);
            }
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}