using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Arena;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.SHIELD)]
    public class ShieldItem : BasePlayerItem
    {
        public ShieldItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (unequip)
                return base.OnEquipItem(true);

            if (!(Owner.Fight is ArenaFight))
                return true;

            Owner.Inventory.MoveItem(this, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            //Vous ne pouvez pas équiper cet objet dans un combat de Kolizéum.
            BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 298);

            return false;
        }
    }
}
