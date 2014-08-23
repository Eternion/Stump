using System;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

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
}