using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_LearnSpell)]
    public class LearnSpell : UsableEffectHandler
    {
        public LearnSpell(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            var template = SpellManager.Instance.GetSpellTemplate((uint) integerEffect.Value);
            if (template == null)
                return false;

            if (Target.Spells.HasSpell(template.Spell.Id))
            {
                Target.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 7, template.Spell.Id);
                return false;
            }

            UsedItems = NumberOfUses;

            Target.Spells.LearnSpell(template.Spell.Id);
            Target.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 3, template.Spell.Id);

            return true;
        }
    }
}
