using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_AddSpellPoints)]
    public class SpellPoint : UsableEffectHandler
    {
        public SpellPoint(EffectBase effect, Character target, BasePlayerItem item) 
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var effect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (effect == null)
                return false;

            if (effect.Value < 1)
                return false;

            UsedItems = NumberOfUses;
            Target.SpellsPoints += (ushort)(effect.Value * NumberOfUses);
            Target.RefreshStats();

            return true;
        }
    }
}
