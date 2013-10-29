using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    [ItemType(ItemTypeEnum.BOOST_FOOD)]
    public class FoodBoostHandler : BaseItemHandler
    {
        public FoodBoostHandler(PlayerItem item) : base(item)
        {
        }

        public override bool EquipItem(bool unequip)
        {
            if (unequip)
            {
                Character.Inventory.RemoveItem(Item);
                return false;
            }

            return base.EquipItem(false);
        }
    }
}