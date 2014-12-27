using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(14553)]
    public class SpawnTaxCollectorItem : BasePlayerItem
    {
        public SpawnTaxCollectorItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Guild == null)
                return 0;

            return TaxCollectorManager.Instance.AddTaxCollectorSpawn(Owner) ? (uint)1 : 0;
        }
    }
}
