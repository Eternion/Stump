using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_AddSpellPoints)]
    public class SpellPoint : UsableEffectHandler
    {
        public SpellPoint(EffectBase effect, Character target, BasePlayerItem item) 
            : base(effect, target, item)
        {
        }

        public override bool Apply()
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
