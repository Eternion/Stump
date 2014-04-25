using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions.Criterions;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.CANDY)]
    public class CandyItem : BasePlayerItem
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CandyItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            var criterion = Template.CriteriaExpression as HasItemCriterion;

            if (criterion == null)
            {
                return base.UseItem(amount, targetCell, target);
            }

            var boostItem = ItemManager.Instance.TryGetTemplate(criterion.Item);
                        
            if (boostItem == null)
            {
                logger.Error(string.Format("Candy {0} has boostItem {1} but it doesn't exist",
                    Template.Id, criterion.Item));
                return 0;
            }

            Owner.Inventory.MoveItem(Owner.Inventory.AddItem(boostItem), CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD);

            return 1;
        }
    }
}