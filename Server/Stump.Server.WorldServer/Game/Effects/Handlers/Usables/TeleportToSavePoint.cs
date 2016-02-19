using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_TeleportToSavePoint)]
    public class TeleportToSavePoint : UsableEffectHandler
    {
        public TeleportToSavePoint(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var dest = Target.GetSpawnPoint();
            Target.Teleport(dest);

            UsedItems = 1;

            return true;
        }
    }
}
