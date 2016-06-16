using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Usables;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    public class TeleportPotion : BasePlayerItem
    {
        public TeleportPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var record = TeleportPotionManager.Instance.GetRecord(Template.Id);

            if (record == null)
                return 0;

            var map = World.Instance.GetMap(record.MapId);
            var cell = map.Cells[record.CellId];

            Owner.Teleport(map, cell);

            if (record.Orientation.HasValue)
                Owner.Direction = (DirectionsEnum)record.Orientation;

            return 1;
        }
    }
}