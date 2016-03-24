using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class BonusSetCriterion : Criterion
    {
        public const string Identifier = "Pk";

        public int BonusCount;

        public override void Build()
        {
            byte count;

            if (!byte.TryParse(Literal, out count))
                throw new Exception(string.Format("Cannot build BonusSetCriterion, {0} is not a valid integer", Literal));

            BonusCount = count;
        }

        public override bool Eval(Character character)
        {
            // sum the number of bonus given by itemset
            // if you have only 1 item from the set it gives 0 bonus else you have (count-1) bonuses
            var setItems = character.Inventory.GetEquipedItems().Select(x => x.Template.ItemSet).Where(x => x != null);
            var bonus = setItems.Count() - setItems.Distinct().Count();

            return Compare(bonus, BonusCount);
        }
    }
}
