using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

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