using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions.Criterions;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    [ItemType(ItemTypeEnum.CANDY)]
    public class CandyHandler : BaseItemHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CandyHandler(PlayerItem item)
            : base(item)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            var criterion = Item.Template.CriteriaExpression as HasItemCriterion;

            if (criterion == null)
            {
                return base.UseItem(amount, targetCell, target);
            }

            var boostItem = ItemManager.Instance.TryGetTemplate(criterion.Item);
                        
            if (boostItem == null)
            {
                logger.Error(string.Format("Candy {0} has boostItem {1} but it doesn't exist",
                    Item.Template.Id, criterion.Item));
                return 0;
            }

            Character.Inventory.MoveItem(Character.Inventory.AddItem(boostItem), CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD);

            return 1;
        }
    }
}