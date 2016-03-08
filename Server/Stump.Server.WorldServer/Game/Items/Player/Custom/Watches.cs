using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.MONTRE_EN_AVANCE_15910)]
    [ItemId(ItemIdEnum.MONTRE_EN_RETARD_15909)]
    public class Watches : BasePlayerItem
    {
        public Watches(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Area.Id != 55)
            {
                //Téléportation impossible, il n'existe pas de destination possible.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 493);
                return 0;
            }

            var posY = Template.Id == (int)ItemIdEnum.MONTRE_EN_AVANCE_15910 ? (Owner.Map.Position.Y - 5) : (Owner.Map.Position.Y + 5);
            var map = World.Instance.GetMaps(Owner.Map, Owner.Map.Position.X, posY, true).FirstOrDefault(x => x.Area.Id == Owner.Area.Id);
            var cell = map?.Cells[Owner.Cell.Id];

            if (map == null || cell == null || (!cell.Walkable))
            {
                //Téléportation impossible, il n'existe pas de destination possible. (493)
                //Téléportation impossible, la destination n'est pas libre. (492)
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)(map == null ? 493 : 492));
                return 0;
            }

            Owner.Teleport(map, cell);

            return 0;
        }
    }
}
