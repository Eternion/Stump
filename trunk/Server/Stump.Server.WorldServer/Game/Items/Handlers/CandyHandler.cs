using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions.Criterions;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    [ItemType(ItemTypeEnum.CANDY)]
    public class CandyHandler : BaseItemHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override bool UseItem(Character character, PlayerItem item)
        {
            var criterion = item.Template.CriteriaExpression as HasItemCriterion;

            if (criterion == null)
            {
                logger.Error(string.Format("Candy {0} has no boost item associated, add criteria PO!#id",
                    item.Template.Id));
                return false;
            }

            var boostItem = ItemManager.Instance.TryGetTemplate(criterion.Item);
                        
            if (boostItem == null)
            {
                logger.Error(string.Format("Candy {0} has boostItem {1} but it doesn't exist",
                    item.Template.Id, criterion.Item));
                return false;
            }

            character.Inventory.MoveItem(character.Inventory.AddItem(boostItem), CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD);

            return true;
        }
    }
}