using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.HARNACHEMENT)]
    public class HarnessItem : BasePlayerItem
    {
        public HarnessItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            HarnessTemplate = MountManager.Instance.GetHarness(Template.Id);
        }

        public HarnessRecord HarnessTemplate
        {
            get;
            private set;
        }

        public override bool CanEquip() => false;
        public override bool CanDrop(BasePlayerItem onBasePlayerItem) => HarnessTemplate != null && Owner.EquippedMount != null
            && onBasePlayerItem.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT;

        public override bool OnEquipItem(bool unequip)
        {
            return base.OnEquipItem(unequip);
        }

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            if (HarnessTemplate == null)
                return false;

            Owner.Inventory.MoveItem(this, CharacterInventoryPositionEnum.ACCESSORY_POSITION_RIDE_HARNESS);

            return base.Drop(dropOnItem);
        }
    }
}