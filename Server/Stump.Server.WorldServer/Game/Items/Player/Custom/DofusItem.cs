using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.DOFUS)]
    [ItemType(ItemTypeEnum.TROPHÉE)]
    public class DofusItem : BasePlayerItem
    {
        public DofusItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }


        public override bool OnEquipItem(bool unequip)
        {
            if (unequip)
                return base.OnEquipItem(true);

            if (Owner.Inventory.Any(x => x.IsEquiped() && x.Template.Id == Template.Id && x.Guid != Guid))
                return false;
            
            return base.OnEquipItem(false);
        }
    }
}
