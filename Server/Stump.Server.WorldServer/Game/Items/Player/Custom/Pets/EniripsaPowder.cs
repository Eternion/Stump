using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.POUDRE_DENIRIPSA_2239)]
    public class EniripsaPowder : BasePlayerItem
    {
        public EniripsaPowder(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool CanDrop(BasePlayerItem item)
        {
            return item is PetItem;
        }

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            var pet = dropOnItem as PetItem;

            if (pet == null)
                return false;

            pet.LifePoints++;
            Owner.Inventory.RefreshItem(pet);

            return true;
        }
    }
}