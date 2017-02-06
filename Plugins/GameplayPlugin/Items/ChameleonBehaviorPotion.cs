using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace GameplayPlugin.Items
{
    [ItemId(30110)]
    public class ChameleonBehaviorPotion : BasePlayerItem
    {
        public ChameleonBehaviorPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        [Initialization(typeof(ItemManager), Silent = true)]
        public static void Initialize()
        {
            ItemManager.Instance.AddItemIdConstructor(typeof(ChameleonBehaviorPotion), (ItemIdEnum)30110);
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (!Owner.HasEquippedMount())
                return 0;

            if (Owner.EquippedMount.Behaviors.Contains((int)MountBehaviorEnum.Caméléone))
                return 0;

            Owner.EquippedMount.AddBehavior(MountBehaviorEnum.Caméléone);

            MountHandler.SendMountSetMessage(Owner.Client, Owner.EquippedMount.GetMountClientData());

            return 1;
        }
    }
}
