using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Compass;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_Seek)]
    public class SeekPlayer : UsableEffectHandler
    {
        public SeekPlayer(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var stringEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectString;

            if (stringEffect == null)
                return false;

            var player = World.Instance.GetCharacter(stringEffect.Text);
            if (player == null)
            {
                Target.SendServerMessage("Votre cible n’est pas connectée.");
                return false;
            }

            if (!player.PvPEnabled)
            {
                Target.SendServerMessage("Votre cible n’a pas le mode Joueur contre Joueur d’activé.");
                return false;
            }

            UsedItems = NumberOfUses;
            CompassHandler.SendCompassUpdatePvpSeekMessage(Target.Client, player);

            return true;
        }
    }
}
