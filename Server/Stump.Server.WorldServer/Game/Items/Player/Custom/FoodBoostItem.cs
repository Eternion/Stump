using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.NOURRITURE_BOOST)]
    public class FoodBoostItem : BasePlayerItem
    {
        public FoodBoostItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (!unequip)
                return base.OnEquipItem(false);

            Owner.Inventory.RemoveItem(this);
            return false;
        }
    }
}