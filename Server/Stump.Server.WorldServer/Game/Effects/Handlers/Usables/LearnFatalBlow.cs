using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_LearnFatalBlow)]
    public class LearnFatalBlow : UsableEffectHandler
    {
        public LearnFatalBlow(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            var fatal = SpellIdEnum.COUP_FATAL_BASE;

            switch (integerEffect.Value)
            {
                case 2:
                    fatal = SpellIdEnum.COUP_FATAL_IOP;
                    break;
                case 5:
                    fatal = SpellIdEnum.COUP_FATAL_GROUGALORAGRAN;
                    break;
                case 6:
                    fatal = SpellIdEnum.COUP_FATAL_JORIS;
                    break;
                case 7:
                    fatal = SpellIdEnum.COUP_FATAL_GOULTARD;
                    break;
                case 8:
                    fatal = SpellIdEnum.COUP_FATAL_OTOMAÏ;
                    break;
            }

            if (Target.HasFatalBlow(fatal))
                return false;

            UsedItems = 1;

            Target.AddFatalBlow(fatal);

            return true;
        }
    }
}