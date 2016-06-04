using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_ForgetSpell)]
    public class GuildPotionForgetSpell : UsableEffectHandler
    {
        public GuildPotionForgetSpell(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            if (Target.GuildMember == null)
                return false;

            if (!Target.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_GUILD_BOOSTS))
                return false;

            if (!Target.Guild.UnBoostSpell(integerEffect.Value))
                return false;

            UsedItems = 1;

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_ChangeGuildName)]
    public class GuildPotionName : UsableEffectHandler
    {
        public GuildPotionName(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            if (Target.GuildMember == null)
                return false;

            if (!Target.GuildMember.IsBoss)
                return false;

            var panel = new GuildModificationPanel(Target) { ChangeName = true, ChangeEmblem = false };
            panel.Open();

            UsedItems = 0;

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_ChangeGuildBlazon)]
    public class GuildPotionBlazon : UsableEffectHandler
    {
        public GuildPotionBlazon(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            if (Target.GuildMember == null)
                return false;

            if (!Target.GuildMember.IsBoss)
                return false;

            var panel = new GuildModificationPanel(Target) { ChangeName = false, ChangeEmblem = true };
            panel.Open();

            UsedItems = 0;

            return true;
        }
    }
}