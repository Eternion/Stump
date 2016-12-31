using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemHasEffect(EffectsEnum.Effect_Apparence_Wrapper)]
    public sealed class BoundWrapperObjectItem : BasePlayerItem
    {
        public BoundWrapperObjectItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool IsLinkedToAccount() => true;

        public override bool CanFeed(BasePlayerItem item) => false;
    }
}
