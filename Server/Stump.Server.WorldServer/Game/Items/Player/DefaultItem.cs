using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class DefaultItem : BasePlayerItem
    {
        public DefaultItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }
    }
}