using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.TokenScroll)]
    public class TokenScroll : BasePlayerItem
    {
        public TokenScroll(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            if (!Effects.Any(x => x.EffectId == EffectsEnum.Effect_AddOgrines))
            {
                Effects.Add(new EffectInteger(EffectsEnum.Effect_AddOgrines, (short)Stack));
                Stack = 1;
            }
        }
    }
}
