using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.ROULETTE)]
    public class RouletteCastHandler : DefaultSpellCastHandler
    {
        public RouletteCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public SpellEffectHandler Handler
        {
            get;
            private set;
        }

        public override bool Initialize()
        {
            var effects = Critical ? SpellLevel.CriticalEffects : SpellLevel.Effects;
            Handlers = effects.Select(effect => EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell, TargetedCell, Critical)).ToArray();

            var random = new Random().Next(0, Handlers.Length);
            Handler = Handlers[random];

            m_initialized = true;

            return true;
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            Handler.Apply();
            Handlers[15].Apply();
        }
    }
}
